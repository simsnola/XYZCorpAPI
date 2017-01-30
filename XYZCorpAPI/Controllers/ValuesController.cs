using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYZCorpDAL;

namespace XYZCorpAPI.Controllers
{
    [Route("api/users")]
    public class ValuesController : ApiController
    {
        simsnola_db_nolaEntities sdne = new simsnola_db_nolaEntities();
                
        // GET api/users
        [HttpGet]
        public IEnumerable<XYZUser> Get()
        {
            return sdne.XYZUsers;
        }

        // GET api/users/{id}
        [Route("api/users/{id}")]
        public IHttpActionResult Get([FromUri]int id)
        {
            XYZUser user = sdne.XYZUsers.Find(id);

            if (user == null)
                return NotFound(); 

            return Ok(user);
        }

        // POST api/users
        public IHttpActionResult Post([FromBody]XYZUser user)
        {
            int id = 0;
            bool isNotUnique = sdne.XYZUsers.Where(u => u.Username == user.Username).Any();

            if (isNotUnique)
                return BadRequest("This Username already exists in the system");

            sdne.XYZUsers.Add(user);
            sdne.SaveChanges();

            id = sdne.XYZUsers.Where(u => u.Username == user.Username).Single().UserID;

            return Created("The UserID is: " + id, user);
        }
                
        // POST api/setpoints
        [Route("api/setpoints")]
        [HttpPost]
        public IHttpActionResult SetPoints([FromBody]Point p)
        {            
            XYZUser user = sdne.XYZUsers.Find(p.id);           

            if (user == null)
                return NotFound();

            user.Points = p.points;

            sdne.Entry(user).State = EntityState.Modified;
            sdne.SaveChanges();

            return Ok("The points were assigned to user " + p.id);
        }
    }

    public class Point
    {
        public int id { get; set; }
        public int points { get; set; }

    }
}
