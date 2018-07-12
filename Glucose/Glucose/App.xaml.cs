using System;

using Glucose.Services;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Glucose
{
    public sealed partial class App : Application
    {
        /// <summary>
        /// Поле: Сервис активации
        /// </summary>
        private Lazy<ActivationService> _activationService;

        /// <summary>
        /// Свойство: Сервис астивации
        /// </summary>
        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        /// <summary>
        /// Конструктор класса приложения
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Задаём обработчик события "Переход в фоновый режим"
            EnteredBackground += App_EnteredBackground;

            // TODO WTS: Add your app in the app center and set your secret here.
            // More at https://docs.microsoft.com/en-us/appcenter/sdk/getting-started/uwp
            AppCenter.Start("{Your App Secret}", typeof(Analytics));

            // Deferred execution until used.
            // Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further
            // info on Lazy<T> class.
            // Отложенное выполнеиение Сервиса активации, пока не будет использовано
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        /// <summary>
        /// Обработчик события "При запуске"
        /// </summary>
        /// <param name="args">Аргументы запуска</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Проверка предварительного запуска приложения
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        /// <summary>
        /// Обработка события "Активация приложения"
        /// Активация может быть: файлом, поиском и т.п.
        /// </summary>
        /// <param name="args">Аргументы активации</param>
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        /// <summary>
        /// Создание Сервиса активации
        /// </summary>
        /// <returns>Созданный сервис активации</returns>
        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.MainViewModel),
                new Lazy<UIElement>(CreateShell));
        }

        /// <summary>
        /// Создаёт графическую оболочку приложения
        /// </summary>
        /// <returns>Корневое представление приложения</returns>
        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }

        /// <summary>
        /// Обработчик события перехода в фоновый режим
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события перехода в фоновый режим</param>
        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            // Получить отсрочку
            var deferral = e.GetDeferral();
            // Сохранить состояние приложения
            await Helpers.Singleton<SuspendAndResumeService>.Instance.SaveStateAsync();
            // Завершить отсрочку
            deferral.Complete();
        }

        /// <summary>
        /// Обработчик события: Активация из фонового режима
        /// </summary>
        /// <param name="args">Аргументы активации из фонового режима</param>
        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }
    }
}
