using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Exemplo.Identidade.API.ViewModels;

namespace Exemplo.Identidade.API.Controllers
{
    [ApiController]
    [Route("api/identidade")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
        //[HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginUsuarioViewModel loginUsuarioVM)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(loginUsuarioVM.Email, loginUsuarioVM.Senha,
                false, true);

            if (result.Succeeded)
            {
                return Ok(loginUsuarioVM);
            }

            if (result.IsLockedOut)
            {   
                return BadRequest("Usuário temporariamente bloqueado por tentativas inválidas");
            }

            return BadRequest("Usuário ou Senha incorretos");
        }
    }
}
