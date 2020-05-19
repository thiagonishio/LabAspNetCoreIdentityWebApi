using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Exemplo.Clientes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Thiago Nishio", "Miguel Nishio" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Thiago Nishio - {id}";
        }
    }
}
