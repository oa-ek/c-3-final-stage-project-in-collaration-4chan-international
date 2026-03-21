using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Models_Context.Context;
using Models_Context.Seed_Data;
using Program.ViewModels;
using Repositories.Implementations;
using Repositories.Interfaces;
using Services.Implementations;
using Services.Interfaces;

namespace Program
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<RPGDarkSoulsDbContext>();

            // Репозиторії
            services.AddTransient<IWeaponRepository, WeaponRepository>();
            services.AddTransient<IArmorRepository, ArmorRepository>();
            // Репозиторій для білдів
            services.AddTransient<ICharacterBuildRepository, CharacterBuildRepository>();

            //Сервіси
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IBuildCalculatorService, BuildCalculatorService>();
            services.AddTransient<IValidationService, ValidationService>();
            // Сервіс для білдів
            services.AddTransient<ICharacterBuildService, CharacterBuildService>();

            
            services.AddTransient<MainViewModel>();
            services.AddTransient<MenuWindow>();
            services.AddTransient<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RPGDarkSoulsDbContext>();
                Initializer.Initialize(context);
            }

            var menuWindow = _serviceProvider.GetRequiredService<MenuWindow>();
            menuWindow.Show();
        }
    }
}