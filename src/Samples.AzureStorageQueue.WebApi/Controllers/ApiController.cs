#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Samples.AzureStorageQueue.Services.Interfaces;
using Samples.AzureStorageQueue.WebApi.Models;
#endregion

namespace Samples.AzureStorageQueue.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;
        private readonly IPublisherService _publisherService;
        private readonly ISubscriberService _subscriberService;

        public ApiController(
            ILogger<ApiController> logger,
            IPublisherService publisherService,
            ISubscriberService subscriberService
            )
        {
            _logger = logger;
            _publisherService = publisherService;
            _subscriberService = subscriberService;
        }

        [HttpPost]
        [Route("publisher", Name = "Publisher")]
        public async Task<IActionResult> Publisher(PublisherRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Message))
            {
                return BadRequest();
            }

            PublisherResponse response = new PublisherResponse();
            var item = Services.Implementations.Utilities.GetQueueItemForApi(request.Message);
            await _publisherService.Publish(item);
            if (_publisherService.HasError)
            { 
                response.ErrorMessage = _publisherService.ErrorMessage;
                return Ok(response);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet]
        [Route("subscriber", Name = "Subscriber")]
        public async Task<IActionResult> Subscriber()
        {
            SubscriberResponse response = new SubscriberResponse();
            var message = await _subscriberService.PeekNextMessage();
            if(message != null)
            {
                response.Message = message;
            }
            else
            {
                response.ErrorMessage = "No messages found.";
            }

            return Ok(response);
        }

    }
}
