using System;

using CommonServiceLocator;

using GalaSoft.MvvmLight.Ioc;

using Glucose.Services;
using Glucose.Views;

namespace Glucose.ViewModels
{
    /// <summary>
    /// Класс: Локатор модели представления
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Конструктор локатора модели представления
        /// </summary>
        public ViewModelLocator()
        {
            // Установить поставщика локатора
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Регистрация Расширения навигационного сервиса в контейнере Инверсии управления
            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            // Регистрация Модели представления оболочки в контейнере Инверсии управления
            SimpleIoc.Default.Register<ShellViewModel>();
            // Регистрация Моделеи представлений и Представлений
            Register<MainViewModel, MainPage>();
            Register<WebViewViewModel, WebViewPage>();
            Register<TelerikDataGridViewModel, TelerikDataGridPage>();
            Register<ChartViewModel, ChartPage>();
            Register<CameraViewModel, CameraPage>();
            Register<SettingsViewModel, SettingsPage>();
        }

        /// <summary>
        /// Свойство: Модель представления параметров
        /// </summary>
        public SettingsViewModel SettingsViewModel =>
            ServiceLocator.Current.GetInstance<SettingsViewModel>();

        /// <summary>
        /// Свойство: модель представления камеры
        /// </summary>
        public CameraViewModel CameraViewModel =>
            ServiceLocator.Current.GetInstance<CameraViewModel>();

        /// <summary>
        /// Свойство: Модель представления графика
        /// </summary>
        public ChartViewModel ChartViewModel =>
            ServiceLocator.Current.GetInstance<ChartViewModel>();

        /// <summary>
        /// Свойство: Модель представления таблицы
        /// </summary>
        public TelerikDataGridViewModel TelerikDataGridViewModel =>
            ServiceLocator.Current.GetInstance<TelerikDataGridViewModel>();

        /// <summary>
        /// Свойство: Млдель представления вёб страницы
        /// </summary>
        public WebViewViewModel WebViewViewModel =>
            ServiceLocator.Current.GetInstance<WebViewViewModel>();

        /// <summary>
        /// Свойство: Модель представления Главной страницы
        /// </summary>
        public MainViewModel MainViewModel =>
            ServiceLocator.Current.GetInstance<MainViewModel>();

        /// <summary>
        /// Свойство: Модель представления оболочки
        /// </summary>
        public ShellViewModel ShellViewModel =>
            ServiceLocator.Current.GetInstance<ShellViewModel>();

        /// <summary>
        /// Свойство: Модель представления навигации
        /// </summary>
        public NavigationServiceEx NavigationService =>
            ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        /// <summary>
        /// Метод: Регистратор Модели представления и модели
        /// </summary>
        /// <typeparam name="VM">Класс модели представления</typeparam>
        /// <typeparam name="V">Класс представления</typeparam>
        public void Register<VM, V>() where VM : class
        {
            // Регистрация Модели представления в контейнере Инверсии управления
            SimpleIoc.Default.Register<VM>();

            // Конфигурирование навигационного сервиса, задание соответсвия между
            // Моделью представления и Представленим
            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
