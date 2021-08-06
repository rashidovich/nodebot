using Microsoft.AspNetCore.Http;
using NodeBotServer.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace NodeBotServer.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, BotConfig config)
        {
            try
            {
                var secret = context.Request.Headers.FirstOrDefault(x => x.Key == "AuthSecret").Value.ToString();

                if (!string.IsNullOrWhiteSpace(secret)
                    && !string.IsNullOrWhiteSpace(config.Auth.Value)
                    && secret.Equals(config.Auth.Value)) 
                {
                    Log.Information("Authentication success.");

                    await _next(context);
                    
                    return;
                }
            }
            catch (Exception)
            {
                Log.Error("Authentication error.");
            }

            Log.Warning("Unauthorized call.");
        }
    }
}
