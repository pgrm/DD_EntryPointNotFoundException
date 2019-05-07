using System.Net;
using Not.Working.Common.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Not.Working.Api
{
    public class Startup
    {
#pragma warning disable CC0091 // Use static method

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            Mapper.Initialize(configuration =>
            {
                configuration.ConfigureAutoMapperForApplication();
            });

            Mapper.Configuration.AssertConfigurationIsValid();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "text/plain";

                    // TODO: consider enhancing this similarly as described here: https://stackoverflow.com/a/40615253/621366
                    await context.Response.WriteAsync("Internal Server Error");
                });
            });

            app.UseCors("default");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto,
                ForwardedProtoHeaderName = "Not-Forwarded-Proto"
            });


            app.UseAuthentication();

            app.UseMvc();

            app.UseRewriter(new RewriteOptions()
                .AddRedirect("^$", "swagger"));
        }
    }
}