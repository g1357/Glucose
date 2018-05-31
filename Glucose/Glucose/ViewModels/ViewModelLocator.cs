using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using Glucose.Services;
using Glucose.Views;

namespace Glucose.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<WebViewViewModel, WebViewPage>();
            Register<TelerikDataGridViewModel, TelerikDataGridPage>();
            Register<ChartViewModel, ChartPage>();
            Register<CameraViewModel, CameraPage>();
            Register<SettingsViewModel, SettingsPage>();
        }

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public CameraViewModel CameraViewModel => ServiceLocator.Current.GetInstance<CameraViewModel>();

        public ChartViewModel ChartViewModel => ServiceLocator.Current.GetInstance<ChartViewModel>();

        public TelerikDataGridViewModel TelerikDataGridViewModel => ServiceLocator.Current.GetInstance<TelerikDataGridViewModel>();

        public WebViewViewModel WebViewViewModel => ServiceLocator.Current.GetInstance<WebViewViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
