using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Arya.Vis.Core.Config
{
    public class ConfigurationManager
    {
        private  IConfiguration _configuration;
        public ConfigurationManager(string fileName)
        {
            var builder = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile(fileName);

            _configuration = builder.Build();
        }

        public  IConfiguration Configuration
        {
            get
            {
                return _configuration;
            }
        }
    }
}
