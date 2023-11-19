using Calculator.Wpf.Module.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Calculator.Wpf.Module;

public class CalculatorModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion("MainContent", typeof(CalcView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }
}
