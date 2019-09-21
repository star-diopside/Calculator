using Calculator.Wpf.Module;
using Calculator.Wpf.Views;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;
using System.Windows.Threading;
using Unity;
using Unity.Microsoft.Logging;

namespace Calculator.Wpf
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CalculatorModule>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new NLogLoggerProvider());
            containerRegistry.GetContainer().AddExtension(new LoggingExtension(loggerFactory));
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Container.Resolve<ILogger<App>>().LogError(e.Exception, e.Exception.Message);
            MessageBox.Show(e.Exception.Message, "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
