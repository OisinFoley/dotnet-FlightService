using Microsoft.AspNetCore.Builder;
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
                .AddScoped<IFlightRepository, FlightRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddScoped<IUnitOfWork, ApplicationContext>();

            services.AddSingleton<IHostedService, IncomingMessageService<Booking>>(
                serviceProvider =>
                {
                    string topicName = Configuration["MessageServiceCommon:TopicName"];
                    string messageBusConnectionString = Configuration["MessageServiceCommon:ConnectionString"];
                    string subscriptionName = Configuration["InboundMessageService:SubscriptionName"];
                    
                    var messageRepository = serviceProvider.GetService<IMessageRepository>();
                    var flightRepository = serviceProvider.GetService<IFlightRepository>();
                    var logger = serviceProvider.GetService<ILogger<IncomingMessageService<Booking>>>();
                    var eventReceiver = new EventReceiver<Booking>(logger, messageBusConnectionString, topicName, subscriptionName);

                    return new IncomingMessageService<Booking>(logger, messageRepository, eventReceiver, flightRepository);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
