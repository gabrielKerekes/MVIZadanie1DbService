using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVIZadanie1DbService.Controllers
{
    public class EventController : ApiController
    {
        public string Get()
        {
            return EventDb.GetAllEventsFromDb();
        }
    }
}
