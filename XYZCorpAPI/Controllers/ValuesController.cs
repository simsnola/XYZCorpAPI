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

        /// <summary>
        /// This is the Get operation, it lists all of the users in the database
        /// GET api/users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        public IEnumerable<XYZUser> Get()
        {
            return sdne.XYZUsers;
        }
                
        /// <summary>
        /// This Get operation provides the specified User
        /// GET api/users/{id}
        /// </summary>
        /// <param name="id">An integer of the UserID</param>
        /// <returns></returns>
        [Route("api/users/{id}")]
        public IHttpActionResult Get([FromUri]int id)
        {
            XYZUser user = sdne.XYZUsers.Find(id);

            if (user == null)
                return NotFound(); 

            return Ok(user);
        }

        /// <summary>
        /// Create new user 
        /// POST api/users
        /// </summary>
        /// <param name="user">{"Username": {"username"}","Points": {points}}</param>
        /// <returns>Success message with the user object</returns>
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

        /// <summary>
        /// Sets the points of the provided user using a JSON object
        /// POST api/setpoints
        /// </summary>
        /// <param name="p">{"id": {id},"points": {points}}</param>
        /// <returns>Success message with the UserID of the updated User </returns>
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
