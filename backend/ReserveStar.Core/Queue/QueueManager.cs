using System.Text.Json;
using Microsoft.Extensions.Logging;
using ReserveStar.Core.Exeptions;
using ReserveStar.Core.Queue.Models;
using ReserveStar.Helper.Constants;
using ReserveStar.Helper.Extensions;

namespace ReserveStar.Core.Queue;

public sealed class QueueManager : IQueueManager
{
   private readonly IMessageTemplateManager _messageTemplateManager;
   private readonly IQueueTransportManager _queueTransportManager;
   private readonly ILogger<QueueManager> _logger;

   public QueueManager(
      IMessageTemplateManager messageTemplateManager,
      IQueueTransportManager queueTransportManager,
      ILogger<QueueManager> logger)
   {
      _messageTemplateManager = messageTemplateManager;
      _queueTransportManager = queueTransportManager;
      _logger = logger;
   }

   public async Task EnqueueTemplateMessageAsync(QueueTemplateMessageRequest request, CancellationToken cancellationToken = default)
   {
      if (request is null || string.IsNullOrWhiteSpace(request.QueueName) || string.IsNullOrWhiteSpace(request.TemplateKey) || string.IsNullOrWhiteSpace(request.Recipient)) return;

      var languageId = request.LanguageId.IsNullOrEmpty()
         ? Guid.Parse(ApplicationContants.DefaultLanguageId)
         : request.LanguageId;

      var renderedTemplate = await _messageTemplateManager.RenderAsync(request.TemplateKey, languageId, request.TemplateValues, cancellationToken);

      var payload = JsonSerializer.Serialize(new OutboundQueueMessage(
         request.Recipient,
         renderedTemplate.Subject,
         renderedTemplate.Body,
         request.TemplateKey,
         DateTimeOffset.UtcNow));

      await _queueTransportManager.PublishAsync(request.QueueName, payload, cancellationToken);


      _logger.LogInformation("Queue message scheduled. Queue: {QueueName}, Template: {TemplateKey}", request.QueueName, request.TemplateKey);
   }
}
