﻿using Microsoft.AspNetCore.Http;
using System.Net;

namespace AA.PMTOGO.Infrastructure.Middleware
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");//"*");
            context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:8080"); //"*");
            context.Response.Headers.Add("Access-Control-Max-Age", "86400"); // 24 hours in seconds
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");

            if (context.Request.Method == "OPTIONS")
            {
                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                return;
            }

            await _next(context);
        }
    }

}
