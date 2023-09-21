using System;
using Microsoft.AspNetCore.Mvc;
using IdempotentAPI.Filters;
using Idempotency.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Newtonsoft.Json;

namespace Idempotency.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class PaymentController : ControllerBase
    {

        private readonly IDistributedCache _db;

        public PaymentController(IDistributedCache db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest($"The id should not be null");

            var response = GetPayment(id);

            return Ok(response);
        }

        [HttpPost]
        [Idempotent(ExpireHours = 24)]
        public IActionResult Post([FromBody] PaymentRequest request)
        {
            if (request is null)
                return BadRequest($"The request should not be null");

            if (request.CardNumber.Length != 16)
                return BadRequest("The card number should be 16 characters");

            var id = Guid.NewGuid().ToString();

            var response = new PaymentResponse
            {
                Id = id.ToString(),
                Amount = request.Amount,
                Currency = request.Currency,
                CardNumber = request.CardNumber.Substring(12, 4).PadLeft(16, '*'),
                Created = DateTime.UtcNow
            };

            SavePayment(id, response);

            return Ok(response);
        }

        private void SavePayment(string id, PaymentResponse payment)
        {
            var options = new DistributedCacheEntryOptions();

            // I am using the cache as a makeshift database here.
            // Obviously in a real system you would be saving this to an actual database.
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payment));
            _db.Set(id, data, options);
        }

        private PaymentResponse GetPayment(string id)
        {
            var data = _db.Get(id);

            if (data is null)
                return null;

            return JsonConvert.DeserializeObject<PaymentResponse>(Encoding.UTF8.GetString(data));
        }
    }
}
