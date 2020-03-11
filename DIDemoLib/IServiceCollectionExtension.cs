using DIDemoLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIDemoLib
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDIDemoLib(this IServiceCollection services)
        {
            services.AddSingleton<IApiClient, ApiClient>();
            return services;
        }
    }
}
