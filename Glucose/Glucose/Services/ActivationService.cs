using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Glucose.Activation;
using Glucose.Helpers;
using Glucose.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Glucose.Services
{
    // For more information on application activation see
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    /// <summary>
    /// Сервис активации
    /// </summary>
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Lazy<UIElement> _shell;
        private readonly Type _defaultNavItem;

        private ViewModels.ViewModelLocator Locator => Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

        private NavigationServiceEx NavigationService => Locator.NavigationService;

        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        /// <summary>
        /// Конструктор Сервиса активации
        /// </summary>
        /// <param name="activationArgs">Аргументы активации</param>
        /// <returns>Нет</returns>
        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to
                    // the first page
                    Window.Current.Content = _shell?.Value ?? new Frame();
                    NavigationService.NavigationFailed += (sender, e) =>
                    {
                        throw e.Exception;
                    };
                    NavigationService.Navigated += Frame_Navigated;
                    if (SystemNavigationManager.GetForCurrentView() != null)
                    {
                        SystemNavigationManager.GetForCurrentView().BackRequested +=
                            ActivationService_BackRequested;
                    }
                }
            }

            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }

                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }

        /// <summary>
        /// Асинхронная инициализация таких вещей, как фоновые задачи
        /// до запуска приложения
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            // Создание Сервиса живых плиток
            await Singleton<LiveTileService>.Instance.EnableQueueAsync();
            // Создание и регистрация фоновых задач
            await Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasksAsync();
            // Инициализация Сервиса выбора темы приложенич
            await ThemeSelectorService.InitializeAsync();
            // Ожидание окончания задач (получение списка успешно завершённых задач)
            await Task.CompletedTask;
        }

        /// <summary>
        /// Асинхронный запуск приложения
        /// </summary>
        /// <returns>Нет</returns>
        private async Task StartupAsync()
        {
            // Установка выбранной темы приложения
            ThemeSelectorService.SetRequestedTheme();

            // TODO WTS: Configure and enable Azure Notification Hub integration.
            //  1. Go to the AzureNotificationsService class, in the InitializeAsync() method, provide the Hub Name and DefaultListenSharedAccessSignature.
            //  2. Uncomment the following line (an exception will be thrown if it is executed and the above information is not provided).
            // await Singleton<AzureNotificationsService>.Instance.InitializeAsync();
            // Обновление живвых плиток
            Singleton<LiveTileService>.Instance.SampleUpdate();
            //
            await FirstRunDisplayService.ShowIfAppropriateAsync();
            //
            await WhatsNewDisplayService.ShowIfAppropriateAsync();
            // Ожидание окончания задач (получение списка успешно завершённых задач)
            await Task.CompletedTask;
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<LiveTileService>.Instance;
            yield return Singleton<AzureNotificationsService>.Instance;
            yield return Singleton<ToastNotificationsService>.Instance;
            yield return Singleton<BackgroundTaskService>.Instance;
            yield return Singleton<SuspendAndResumeService>.Instance;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationService.CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        private void ActivationService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                e.Handled = true;
            }
        }
    }
}
