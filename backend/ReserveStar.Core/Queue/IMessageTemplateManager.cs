using ReserveStar.Core.Queue.Models;

namespace ReserveStar.Core.Queue;

public interface IMessageTemplateManager
{
   Task<RenderedTemplateMessage> RenderAsync(string templateKey, Guid languageId, IReadOnlyDictionary<string, string> templateValues, CancellationToken cancellationToken = default);
}
