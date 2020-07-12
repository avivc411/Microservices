using System.Collections.Generic;
using Consumer.Model;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Record> GetRecords()
        {
            try
            {
                return RecordConsumerService.Records;
            }
            catch
            {
                return null;
            }
        }
    }
}