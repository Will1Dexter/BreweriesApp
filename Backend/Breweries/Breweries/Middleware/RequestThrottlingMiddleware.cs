using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Breweries.Middleware
{
    public class RequestThrottlingMiddleware
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly RequestDelegate _next;
        private readonly int _requestsPerMinuteThreshold;
        private readonly string _globalConsumerKey;

        public RequestThrottlingMiddleware(
            RequestDelegate next,
            IConnectionMultiplexer connection,
            int requestsPerMinuteThreshold
        )
        {
            _next = next;
            _connection = connection;
            _requestsPerMinuteThreshold = requestsPerMinuteThreshold;
            _globalConsumerKey = Guid.NewGuid().ToString();
        }

        public string GetConsumerKey(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.ToLower() == "/beers")
            {
                return _globalConsumerKey;
            }
            else
            {
                return null;
            }
        }

        public static string GetMessageToConsumer(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.ToLower() == "/beers")
            {
                return "Vai com calma, você já bebeu 3 cervejas só nesse minuto!";
            }
            else
            {
                return null;
            }
        }

        public async Task Invoke(HttpContext context)
        {
            var cache = _connection.GetDatabase();

            var consumerKey = GetConsumerKey(context);

            if (!string.IsNullOrWhiteSpace(consumerKey))
            {
                var consumerCacheKey = $"consumer.throttle#{consumerKey}";

                var cacheResult = cache.HashIncrement(consumerCacheKey, 1);

                if (cacheResult == 1)
                {
                    cache.KeyExpire(
                        consumerCacheKey,
                        TimeSpan.FromSeconds(60),
                        CommandFlags.FireAndForget
                    );
                }
                else if (cacheResult > _requestsPerMinuteThreshold)
                {
                    context.Response.StatusCode = 429;

                    using (var writer = new StreamWriter(context.Response.Body))
                    {
                        var message = GetMessageToConsumer(context);
                        await writer.WriteAsync(message);
                    }

                    return;
                }
            }

            await _next(context);
        }
    }
}
