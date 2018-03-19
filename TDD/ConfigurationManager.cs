using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using TDD.Interface;

namespace TDD
{
    public class ConfigurationManager:IConfigurationManager
    {
        private readonly IConfiguration _configuration;

        public ConfigurationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key)
        {
            //to jest to samo co w startup
            return _configuration[key];
        }


    }
}
