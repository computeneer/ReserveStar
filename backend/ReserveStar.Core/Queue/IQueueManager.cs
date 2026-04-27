using ReserveStar.Core.Queue.Models;

namespace ReserveStar.Core.Queue;

public interface IQueueManager
{
   Task EnqueueTemplateMessageAsync(QueueTemplateMessageRequest request, CancellationToken cancellationToken = default);
}
