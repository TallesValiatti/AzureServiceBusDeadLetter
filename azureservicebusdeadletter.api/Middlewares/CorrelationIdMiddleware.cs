using azureservicebusdeadletter.shared.CorrelationId;

namespace azureservicebusdeadletter.api.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICorrelationId _correlationId)
        {
            Guid correlationId;
            if(context.Request.Headers.TryGetValue("x-correlation-id", out var correlationIdValue))
                correlationId = Guid.Parse(correlationIdValue);
            else
            {
                correlationId = CorrelationIdGenerator.Generate();
            }

            _correlationId.Set(correlationId);

            await _next(context);
        }
    }
}