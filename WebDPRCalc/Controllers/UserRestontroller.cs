using Microsoft.AspNetCore.Mvc;

namespace WebDPRCalc.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserRestontroller : ControllerBase
    {
        [HttpPost]
        public void CreateUser([FromBody] object user)
        {

        }
        [HttpGet("{username}")]
        public string ReadUser(string username)
        {

        }
        [HttpPatch]
        public void UpdateUser([FromBody] object user)
        {

        }
        [HttpDelete("{username}")]
        public void DeleteUser(string username)
        {

        }
        [HttpPost("attack/{username}")]
        public void CreateAttack(string username, [FromBody] object attack)
        {

        }
        [HttpGet("attack/{username}/{id}")]
        public string ReadAttack(string username, int id)
        {

        }
        [HttpGet("attack/{username}")]
        public string ReadAttacks(string username)
        {

        }
        [HttpPatch("attack/{username}")]
        public void UpdateAttack(string username, [FromBody] object attack)
        {

        }
        [HttpDelete("attack/{username}/{id}")]
        public void DeleteAttack(string username, int id)
        {

        }
    }
}
