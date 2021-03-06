﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FlightsService.Data;
using FlightsService.Data.Abstract;
using FlightsService.Data.Concrete;
using Microsoft.Extensions.Hosting;
using FlightsService.BackgroundWorker;
using FlightsService.Models;
using EventDispatcher.Azure;
using Microsoft.Extensions.Logging;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.IO;
using System;
using System.Reflection;
using Swashbuckle.AspNetCore.Swagger;

namespace FlightsService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services
                .AddDbContext<ApplicationContext>(options => ApplicationContextConfigurator.SetContextOptions(options, Configuration))
                .AddTransient<IFlightRepository, FlightRepository>()
                .AddTransient<IUnitOfWork, ApplicationContext>();

            services.AddSingleton<IHostedService, IncomingMessageService<Booking>>(
                serviceProvider =>
                {
                    string topicName = Configuration["MessageServiceCommon:TopicName"];
                    string messageBusConnectionString = Configuration["MessageServiceCommon:ConnectionString"];
                    string subscriptionName = Configuration["InboundMessageService:SubscriptionName"];
                    
                    var flightRepository = serviceProvider.GetService<IFlightRepository>();
                    var logger = serviceProvider.GetService<ILogger<IncomingMessageService<Booking>>>();
                    var eventReceiver = new EventReceiver<Booking>(logger, messageBusConnectionString, topicName, subscriptionName);

                    return new IncomingMessageService<Booking>(logger, eventReceiver, flightRepository);
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Flight Service API",
                    Description = "Simple API to perform CRUDs on Flights",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.DescribeAllParametersInCamelCase();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Service API V1");
            });

            app.UseMvc();
        }
    }
}
