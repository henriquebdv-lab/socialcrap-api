using Microsoft.AspNetCore.Mvc;
using SocialCrap.DTOs;
using SocialCrap.Service;

namespace SocialCrap.Controllers
{
    // Endpoints de autenticacao.
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        // Realiza login basico por email e senha.
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            var result = await _service.LoginAsync(request);

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }
    }
}
