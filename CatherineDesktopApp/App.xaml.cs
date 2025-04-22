using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CatherineDesktopApp.Data;
using CatherineDesktopApp.Services;
using CatherineDesktopApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CatherineDesktopApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {

        private Window _mainWindow;

        private Frame RootFrame;

        private static readonly IServiceCollection _serviceCollection = new ServiceCollection();
        private IServiceProvider _serviceProvider;

        private IConfiguration _configuration;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
#if DEBUG
            this.UnhandledException += (sender, args) =>
            {
                Debug.WriteLine($"Unhandled exception: {args.Exception}");
                Debug.WriteLine($"Message: {args.Message}");
                Debug.WriteLine($"StackTrace: {args.Exception.StackTrace}");
            };

            DebugSettings.BindingFailed += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine(args.Message);
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            };
#endif
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            try
            {
                InitializeApp();
                InitializeWindow();
                PerformInitialNavigation();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Exit();
            }
        }

        private void InitializeWindow()
        {
            _mainWindow = new MainWindow();
            RootFrame = new Frame()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
            _mainWindow.Content = RootFrame;
        }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider ServiceProvider { get => _serviceProvider; }

        /// <summary>
        /// Gets the <see cref="MainWindow"/> instance.
        /// </summary>
        public Window Window => _mainWindow ?? throw new InvalidOperationException("MainWindowNotSet");

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        private void InitializeApp()
        {
            LoadConfiguration();
            RegisterServices();
        }

        private void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        private void RegisterServices()
        {
            _serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            _serviceCollection.AddSingleton<MainWindow>();

            _serviceCollection.AddSingleton<IApplicationWindow, ApplicationWindow>();
            _serviceCollection.AddSingleton<IResourceLocationService, ResourceLocationService>();
            _serviceCollection.AddSingleton<IDialogService, DialogService>();

            _serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            _serviceProvider = _serviceCollection.BuildServiceProvider();

        }

        private void PerformInitialNavigation()
        {
            NavigateToMain();
        }

        private void NavigateToMain()
        {
            RootFrame.Navigate(typeof(WorkInProgressPage));

            _mainWindow.Activate();
        }

    }
}
