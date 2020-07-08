using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using WebDPRCalc.Models;

namespace WebDPRCalc.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserRestController : ControllerBase
    {
        public async Task<User> GetContextUser()
        { return await Authenticate(HttpContext); }
        private UserDatabaseInterface dbInterface = new UserDatabaseInterface();
        public async Task<User> Authenticate(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                string username = usernamePassword.Substring(0, seperatorIndex);
                string password = usernamePassword.Substring(seperatorIndex + 1);
                User user = await dbInterface.readUser(username);
                if (!(user is null))
                {
                    if (user.validPassword(password))
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        [HttpPost]
        public void CreateUser([FromBody] object user)
        {
            User parsed;
            try
            {
                parsed = (User)user;
                dbInterface.createUser(parsed);
            }
            catch (InvalidCastException)
            {
                Response.StatusCode = 400;
            }
        }
        [HttpGet]
        public async Task<string> ReadUser()
        {
            User context = await GetContextUser();
            if ((context is null))
            {
                Response.StatusCode = 403;
                return null;
            }
            return new JavaScriptSerializer().Serialize(await dbInterface.readUser(context.username));
        }
        [HttpPatch]
        public async void UpdateUser([FromBody] object user)
        {
            User context = await GetContextUser();

            User parsed;
            try
            {
                parsed = (User)user;
                if ((context is null) || !context.username.Equals(parsed.username))
                {
                    Response.StatusCode = 403;
                    return;
                }
                parsed.attacks = context.attacks;
                dbInterface.updateUser(parsed);
            }
            catch (InvalidCastException)
            {
                Response.StatusCode = 400;
            }
        }
        [HttpDelete]
        public async void DeleteUser()
        {
            User context = await GetContextUser();
            if ((context is null))
            {
                Response.StatusCode = 403;
                return;
            }
            dbInterface.deleteUser(context.username);
        }
        [HttpPost("attack")]
        public async void CreateAttack([FromBody] object attack)
        {
            Attack parsed = null;
            User context = await GetContextUser();
            if (context is null)
            {
                Response.StatusCode = 403;
                return;
            }
            try
            {
                parsed = (Attack)attack;
                dbInterface.createAttack(context.username, parsed);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is InvalidCastException)
                {
                    Response.StatusCode = 400;
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpGet("attack/{id}")]
        public async Task<string> ReadAttack(int id)
        {
            User context = await GetContextUser();
            if (context is null)
            {
                Response.StatusCode = 403;
                return null;
            }
            try
            {
                return new JavaScriptSerializer().Serialize(dbInterface.readAttack(context.username, id));
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return null;
            }
        }
        [HttpGet("attack")]
        public async Task<string> ReadAttacks()
        {
            User context = await GetContextUser();
            if (context is null)
            {
                Response.StatusCode = 403;
                return null;
            }
            try
            {
                return new JavaScriptSerializer().Serialize(dbInterface.readAttacks(context.username));
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return null;
            }
        }
        [HttpPatch("attack")]
        public async void UpdateAttack([FromBody] object attack)
        {
            Attack parsed = null;
            User context = await GetContextUser();
            if (context is null)
            {
                Response.StatusCode = 403;
                return;
            }
            try
            {
                parsed = (Attack)attack;
                dbInterface.updateAttack(context.username, parsed);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is InvalidCastException)
                {
                    Response.StatusCode = 400;
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpDelete("attack/{id}")]
        public async void DeleteAttack(int id)
        {
            User context = await GetContextUser();
            if (context is null)
            {
                Response.StatusCode = 403;
                return;
            }
            try
            {
                dbInterface.deleteAttack(context.username, id);
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
            }
        }
        [HttpGet("attack/caclulation/{id}")]
        public async Task<string> getDPRCalculationForAttack(int id)
        {
            User context = await GetContextUser();
            if (context is null)
            {
                Response.StatusCode = 403;
                return null;
            }
            try
            {
                var attack = await dbInterface.readAttack(context.username, id);
                return new JavaScriptSerializer().Serialize(attack.DPRCaclulation());
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return null;
            }
        }
        [HttpGet("attack/caclulation")]
        public string getDPRCalculationForAttack([FromBody] object attack)
        {
            Attack parsed = null;
            try
            {
                parsed = (Attack)attack;
                return new JavaScriptSerializer().Serialize(parsed.DPRCaclulation());
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is InvalidCastException)
                {
                    Response.StatusCode = 400;
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
