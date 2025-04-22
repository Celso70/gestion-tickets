using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using GestionTickets.Models.Usuario;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GestionTickets.Controllers
{
    
    [Route("api/[controller]")] //API PARA FRONT SEPARADO
    [ApiController] 
    public class AuthController : ControllerBase
    {
        //DEPENDENCIAS NECESARIAS 
        private readonly UserManager<ApplicationUser> _userManager; //Manejo de usuarios
        private readonly SignInManager<ApplicationUser> _signInManager; //Validar credenciales,logout,etc
        private readonly IConfiguration _configuration; //Acceso a las config, cadenas de conexión, keys de jwt

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // 1. Validamos que los datos del modelo sean correctos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 2. Chequeamos si ya hay un usuario con ese email
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest("El email ya está registrado.");

            // 3. Creamos un nuevo ApplicationUser con los datos recibidos
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto
            };

            // 4. Intentamos crear el usuario en la base de datos
            var result = await _userManager.CreateAsync(user, model.Password);

            // 5. Si el registro fue exitoso
            if (result.Succeeded)
                return Ok("Usuario registrado correctamente.");

            // 6. Si hubo errores, los devolvemos
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            //Valida que el modelo cuente con los datos requeridos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            //Busca si existe un usuario en la base de datos con el email que se pasó
            var user = await _userManager.FindByEmailAsync(model.Email);
            //Si no existe:
            if (user == null)
                return Unauthorized("Credenciales inválidas");
            
            //Verificación de la contraseña(el false es para bloquear la cuenta tras 3 intentos fallidos)
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            //Si no es válida
            if (!result.Succeeded)
                return Unauthorized("Credenciales inválidas");


            
            var claims = ObtenerClaims(user);
            var token = GenerarJwtToken(claims);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            //REFRESH TOKEN
            var refreshToken = GenerarRefreshToken();
            await _userManager.SetAuthenticationTokenAsync(user, "GestionTickets", "RefreshToken", refreshToken);

            //DEVUELVE
            return Ok(new
            {
                //TOKEN
                token = jwt,
                //EXPIRACIÓN
                expiration = token.ValidTo,
                refreshToken = refreshToken
            });

        }


        //ENDPOINT QUE: PERMITE SOLICITAR UN NUEVO JWT LUEGO DE SU EXPIRACIÓN
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {

            //BUSCAMOS AL USUARIO POR EL EMAIL
            var user = await _userManager.FindByEmailAsync(model.Email);
            //SI NO LO ENCUENTRA
            if (user == null)
                return Unauthorized();

            //OBTENER REFRESH TOKEN GUARDADO
            var savedToken = await _userManager.GetAuthenticationTokenAsync(user, "GestionTickets", "RefreshToken");
            //SI EL TOKEN NO COINCIDE
            if (savedToken != model.RefreshToken)
                return Unauthorized("Refresh token inválido");


            var claims = ObtenerClaims(user);
            var newToken = GenerarJwtToken(claims);
            var jwt = new JwtSecurityTokenHandler().WriteToken(newToken);


            //CREAR Y GUARDAR UN NUEVO REFRESH TOKEN
            var newRefreshToken = GenerarRefreshToken();
            await _userManager.SetAuthenticationTokenAsync(user, "GestionTickets", "RefreshToken", newRefreshToken);

            return Ok(new
            {
                token = jwt,
                expiration = newToken.ValidTo,
                refreshToken = newRefreshToken,
                tokenExpired = true
            });
        }


        //ENDPOINT DONDE: SE INVALIDA EL "REFRESH TOKEN DEL USUARIO",
        //  PARA QUE NO PUEDA VOLVER A GENERAR UNO NUEVO SIN LOGEARSE
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest model)
        {

            //RECIBIMOS EL EMAIL ASOCIADO AL USUARIO, PARA ELIMINAR SU TOKEN
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Usuario no encontrado." });
            }

            //BORRAR TOKEN ALMACENADO
            await _userManager.RemoveAuthenticationTokenAsync(user, "GestionTickets", "RefreshToken");

            return Ok(new { message = "Sesión cerrada correctamente." });
        }


        //Métodos para evitar duplicar código (claims, key, credenciales y creación de token)

        // 🔽 Método nuevo para generar claims(Pedacitos de info que van dentro del JWT)
        private Claim[] ObtenerClaims(ApplicationUser user)
        {
            return new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        // 🔽 Método nuevo para generar el JWT
        private JwtSecurityToken GenerarJwtToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );
        }

        private string GenerarRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

    }
}
