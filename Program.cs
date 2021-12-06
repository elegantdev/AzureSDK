  "AzurePubSubSettings": {
    "connectionString": "Endpoint=https://pubsubinital.webpubsub.azure.com;AccessKey=E/QKZYo0iygWd2a+leRmu0FLI7DMps9Oqf/xjrV2GGo=;Version=1.0;"
  },





using Azure.Messaging.WebPubSub;
using FrontierCoreAPI.Application.Interfaces;
using FrontierCoreAPI.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FrontierCoreAPI.Infrastructure.Shared.Services
{
    public class AzurePubSubService : IAzurePubSubService
    {
        public AzurePubSubSettings _azurePubSubSettings { get; }
        public ILogger<AzurePubSubService> _logger { get; }

        public AzurePubSubService(IOptions<AzurePubSubSettings> azurePubSubSettings, ILogger<AzurePubSubService> logger)
        {
            _azurePubSubSettings = azurePubSubSettings.Value;
            _logger = logger;
        }
        /// <summary>
        /// Publish message to websocket stream of azure
        /// </summary>
        /// <param name="message"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task<bool> SendNotification(string message, string channel)
        {

            _logger.LogInformation($"SendNotification channel:{channel} message:{message} ");

            if (string.IsNullOrEmpty(channel))
                channel = "Hub"; // default channel name

            //Create the service client
            var serviceClient = new WebPubSubServiceClient(_azurePubSubSettings.connectionString, channel);

            try
            {
                //Publish message in websocket stream
                await serviceClient.SendToAllAsync(message);

            }
            catch (Exception ex)
            {
                _logger.LogError($"SendNotification channel:{channel} message:{message} Error:{ex.Message} stackTrace:{ex.StackTrace}");

                throw ex;

            }
            return true;

        }
    }
}








using FrontierCoreAPI.Application.Interfaces;
using FrontierCoreAPI.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FrontierCoreAPI.Application.Features.AzurePubSub.Commands.SendNotification
{
    public class SendNotificationCommand : IRequest<Response<bool>>
    {
        public string SenderName { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }

        public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Response<bool>>
        {
            private readonly IAzurePubSubService _azurePubSubService;

            //
            public SendNotificationCommandHandler(IAzurePubSubService azurePubSubService)
            {
                _azurePubSubService = azurePubSubService;
            }


            public async Task<Response<bool>> Handle(SendNotificationCommand command, CancellationToken cancellationToken)
            {
               await _azurePubSubService.SendNotification(command.Message,command.Channel);

                return new Response<bool>(true, $"Message broadcasted{(string.IsNullOrEmpty(command.Channel) ? "" :  " in " + command.Channel)} successfully!");
            }
        }

    }
}









using FrontierCoreAPI.Application.Features.AzurePubSub.Commands.SendNotification;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FrontierCoreAPI.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]

    public class AzurePubSubController : BaseApiController
    {
        /// <summary>
        /// Publish message 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SendMessage(SendNotificationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
