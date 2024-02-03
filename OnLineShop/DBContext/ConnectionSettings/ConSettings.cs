using Microsoft.Extensions.Configuration;
using System.IO;

namespace OnLineShop.DBContext.ConnectionSettings
{
    public class ConSettings
    {
        public static string GetConnectionString(string nameDB)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory() + "\\DBContext\\ConnectionSettings");
            builder.AddJsonFile("Config.json");
            var config = builder.Build();
            if (nameDB == "PSGSQL")
                return config.GetConnectionString("PSGSQL");
            return config.GetConnectionString("SQL");
        }
    }
}