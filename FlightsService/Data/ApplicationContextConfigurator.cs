﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace FlightsService.Data
{
    public class ApplicationContextConfigurator
    {
        public static void SetContextOptions(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            switch (configuration["DatabaseHost"])
            {
                case "SQLServer":
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("SQLServer"));
                    break;
                default:
                    throw new ArgumentException("Specify a valid database provider in appsettings.json.");
            }
        }
    }
}
