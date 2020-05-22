using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Exemplo.Identidade.API.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Exemplo.Identidade.API.Controllers
{
    [ApiController]
    [Route("api/identidade")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private IOptions<Audience> _settings;

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<Audience> settings
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _settings = settings;
        }

        // Somente para testar
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<string[]>> Index()
        {   
            return Ok("thiagonishio@gmail.com");
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar([FromBody] RegistrarUsuarioViewModel registrarUsuarioVM)
        {
            if(!ModelState.IsValid) return BadRequest();

            var user = new IdentityUser
            {
                UserName = registrarUsuarioVM.Email,
                Email = registrarUsuarioVM.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registrarUsuarioVM.Senha);

            if (result.Succeeded)
            {
                return Ok(registrarUsuarioVM);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(LoginUsuarioViewModel loginUsuarioVM)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(loginUsuarioVM.Email, loginUsuarioVM.Senha,
                false, true);

            if (result.Succeeded)
            {
                //return Ok(loginUsuarioVM);
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginUsuarioVM.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Secret));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = _settings.Value.Iss,
                    ValidateAudience = true,
                    ValidAudience = _settings.Value.Aud,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,

                };

                var jwt = new JwtSecurityToken(
                    issuer: _settings.Value.Iss,
                    audience: _settings.Value.Aud,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
                };

                return Json(responseJson);
            }

            if (result.IsLockedOut)
            {   
                return BadRequest("Usuário temporariamente bloqueado por tentativas inválidas");
            }

            return BadRequest("Usuário ou Senha incorretos");
        }
    }

    public class Audience
    {
        public string Secret { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
