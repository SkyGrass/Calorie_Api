using System.Collections.Generic;
using System.Web.Http;

namespace Calorie.Controllers
{
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IHttpActionResult Get()
        {

            return Json(new List<int>() { 1, 2, 3 });
        }

        [HttpGet]
        public IHttpActionResult Get1()
        {
            return Success("", new
            {
                a = 1,
                b = 2
            });
        }
    }
}
