using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Producer.Model;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        /// <summary>
        /// Send a record to other service.
        /// </summary>
        /// <param name="record">A full record - no empty fields allowed</param>
        /// Example:
        /// {
        ///     "Name":"testy",
        ///     "Profession":"test",
        ///     "Date":"2020-07-09T21:40:13.1012058+03:00",
        ///     "Age":31
        /// }
        /// <returns>
        /// If succeeded, status 200 and the record
        /// if the record is invalid, status 400
        /// else status 500
        /// </returns>
        [HttpPost]
        public IActionResult PostRecord([FromBody] Record record)
        {
            if (record == null)
                return BadRequest();
            try
            {
                RecordProducerService.SendRecord(record);
                return Ok(record);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return Problem();
            }
        }
    }
}