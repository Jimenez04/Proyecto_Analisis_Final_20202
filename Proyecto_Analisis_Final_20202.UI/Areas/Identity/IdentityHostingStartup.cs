using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_Analisis_Final_20202.UI.Data;

[assembly: HostingStartup(typeof(Proyecto_Analisis_Final_20202.UI.Areas.Identity.IdentityHostingStartup))]
namespace Proyecto_Analisis_Final_20202.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}