using Microsoft.Extensions.DependencyInjection;
using W6_assignment_template.Data;
using W6_assignment_template.Services;

namespace W6_assignment_template
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var gameEngine = serviceProvider.GetService<GameEngine>();

            gameEngine?.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IContext, DataContext>();
            services.AddTransient<GameEngine>();
        }
    }
}
