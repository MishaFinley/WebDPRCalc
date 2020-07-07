using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Threading.Tasks;
using WebDPRCalc.Models;

namespace WebDPRCalc.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserRestController : ControllerBase
    {
        //Authentication / Authorization
        private UserDatabaseInterface dbInterface = new UserDatabaseInterface();
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
        [HttpGet("{username}")]
        public async Task<string> ReadUser(string username)
        {
            return new JavaScriptSerializer().Serialize(await dbInterface.readUser(username));
        }
        [HttpPatch]
        public void UpdateUser([FromBody] object user)
        {
            User parsed;
            try
            {
                parsed = (User)user;
                dbInterface.updateUser(parsed);
            }
            catch (InvalidCastException)
            {
                Response.StatusCode = 400;
            }
        }
        [HttpDelete("{username}")]
        public void DeleteUser(string username)
        {
            dbInterface.deleteUser(username);
        }
        [HttpPost("attack/{username}")]
        public void CreateAttack(string username, [FromBody] object attack)
        {
            Attack parsed = null;
            try
            {
                parsed = (Attack)attack;
                dbInterface.createAttack(username, parsed);
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
        [HttpGet("attack/{username}/{id}")]
        public async Task<string> ReadAttack(string username, int id)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(dbInterface.readAttack(username, id));
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return null;
            }
        }
        [HttpGet("attack/{username}")]
        public string ReadAttacks(string username)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(dbInterface.readAttacks(username));
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return null;
            }
        }
        [HttpPatch("attack/{username}")]
        public void UpdateAttack(string username, [FromBody] object attack)
        {
            Attack parsed = null;
            try
            {
                parsed = (Attack)attack;
                dbInterface.updateAttack(username, parsed);
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
        [HttpDelete("attack/{username}/{id}")]
        public void DeleteAttack(string username, int id)
        {
            try
            {
                dbInterface.deleteAttack(username, id);
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
            }
        }
        [HttpGet("attack/{username}/{id}/caclulation")]
        public async Task<string> getDPRCalculationForAttack(string username, int id)
        {
            try
            {
                var attack = await dbInterface.readAttack(username, id);
                return new JavaScriptSerializer().Serialize(attack.DPRCaclulation());
            }
            catch (ArgumentException ex)
            {
                Response.StatusCode = 400;
                return null;
            }
        }
    }
}
