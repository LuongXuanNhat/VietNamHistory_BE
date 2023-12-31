﻿using Microsoft.Extensions.DependencyInjection;

namespace VNH.Application
{
    public static class DependencyInjectionApp
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjectionApp));
            return services;
        }
    }
}
