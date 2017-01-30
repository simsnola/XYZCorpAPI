using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XYZCorpDAL;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using XYZCorpAPI.Controllers;
using System.Web.Http.Routing;
using System.Web.Http.Results;
using System.Web.Http.Hosting;

namespace XYZCorpAPI.Tests
{
    [TestClass]
    public class ValuesControllerTest
    {
        simsnola_db_nolaEntities sdne = new simsnola_db_nolaEntities();

        [TestMethod]
        public void ShouldGetWithID()
        {
            var controller = new ValuesController();

            var result = controller.Get(1) as OkNegotiatedContentResult<XYZUser>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Username, sdne.XYZUsers.Find(1).Username);
        }

        [TestMethod]
        public void ShouldPostRandom()
        {
            Random rand = new Random(1000);

            XYZUser user = new XYZUser();
            user.Username = "TestUser" + rand.Next();
            user.Points = rand.Next();

            ValuesController controller = new ValuesController()
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost/api/users")
                }
            };
            
            IHttpActionResult result = controller.Post(user);

            Assert.IsInstanceOfType(result, typeof(CreatedNegotiatedContentResult<String>));           

        }

        [TestMethod]
        public void ShouldSetPointsToRandom()
        {
            Random rand = new Random(1000);

            var controller = new ValuesController()
            {
                Configuration = new HttpConfiguration(),
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost/api/setpoints/")
                }
            };

            Point p = new Point();
            p.id = 1;
            p.points = rand.Next();

            IHttpActionResult result = controller.SetPoints(p);
            
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<String>));                 
        }
    }
}
