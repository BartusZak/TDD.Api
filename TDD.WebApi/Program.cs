using System;

namespace TDD.WebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildWebHost(args);
        }

        public static IWebHost BuildWebHost (string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
