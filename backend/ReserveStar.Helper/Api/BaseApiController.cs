
using ReserveStar.Helper.Constants;
using ReserveStar.Helper.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using MediatR;

namespace ReserveStar.Helper.Api;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
   protected readonly IMediator _mediator;
   private readonly IServiceProvider _serviceProvider;
   protected readonly IHttpContextAccessor _httpContextAccessor;
   protected Guid LanguageId;
   protected Guid UserId;

   public BaseApiController(
      IHttpContextAccessor httpContextAccessor,
      IMediator mediator,
      IServiceProvider serviceProvider)
   {
      _httpContextAccessor = httpContextAccessor;
      LanguageId = SetLanguageId();
      _mediator = mediator;
      _serviceProvider = serviceProvider;
   }

   protected Guid SetLanguageId()
   {
      if (_httpContextAccessor is not null && _httpContextAccessor.HttpContext is not null)
      {
         var isExists = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("LanguageId", out var languageId);
         if (isExists)
         {
            Guid.TryParse(languageId, out var parsedLanguageId);

            if (parsedLanguageId.IsNullOrEmpty()) return Guid.Parse(ApplicationContants.DefaultLanguageId);
            return parsedLanguageId;
         }
         return Guid.Parse(ApplicationContants.DefaultLanguageId);
      }
      return Guid.Parse(ApplicationContants.DefaultLanguageId);
   }

   protected async Task<object> Forward<T>(BaseRequest<T> baseRequest)
   {
      baseRequest.LanguageId = LanguageId;
      await ValidateRequestAsync(baseRequest);
      return await _mediator.Send(baseRequest) ?? new object();
   }

   protected async Task<object> ForwardAuth<T>(BaseRequest<T> baseRequest)
   {
      baseRequest.LanguageId = LanguageId;
      baseRequest.UserId = User.GetUserId();
      await ValidateRequestAsync(baseRequest);
      return await _mediator.Send(baseRequest) ?? new object();
   }


   private async Task ValidateRequestAsync<T>(T request) where T : class
   {
      var validators = _serviceProvider.GetServices<IValidator<T>>().ToList();
      if (validators.Count <= 0)
      {
         return;
      }

      var context = new ValidationContext<T>(request);
      var results = await Task.WhenAll(validators.Select(q => q.ValidateAsync(context)));
      var errors = results
         .SelectMany(q => q.Errors)
         .Where(q => q != null)
         .Select(q => $"{q.PropertyName}: {q.ErrorMessage}")
         .ToList();

      if (errors.Count > 0)
      {
         throw new ValidationException(string.Join(" | ", errors));
      }
   }
}
