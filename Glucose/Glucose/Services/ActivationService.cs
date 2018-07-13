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
        // Поле: Объект текущего приложения
        private readonly App _app;
        // Поле: Корневое Представление - Обработчик оболочки (меню и т.п.)
        private readonly Lazy<UIElement> _shell;
        // Поле: Модель представление навигации по-умолчанию
        private readonly Type _defaultNavItem;

        // Свойство: Локатор модели представления
        private ViewModels.ViewModelLocator Locator =>
            Application.Current.Resources["Locator"] as ViewModels.ViewModelLocator;

        private NavigationServiceEx NavigationService => Locator.NavigationService;

        /// <summary>
        /// Конструктор Сервиса активации
        /// </summary>
        /// <param name="app">Приложение</param>
        /// <param name="defaultNavItem">Модель представления по-умолчанию</param>
        /// <param name="shell">Корневое Представление - Обработчик оболочки (меню и т.п.)</param>
        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        /// <summary>
        /// Метод: Активировать Сервиса активации
        /// </summary>
        /// <param name="activationArgs">Аргументы активации</param>
        /// <returns>Нет</returns>
        public async Task ActivateAsync(object activationArgs)
        {
            // Если запуск интерактивный, то ... 
            if (IsInteractive(activationArgs))
            {
                // Initialize things like registering background task before the app is loaded
                // Инициализирует такие вещи, как регистрация фоновых задач,
                // перед тем как приложение загружно
                await InitializeAsync();

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                // Если текущее окно ещё не инициализировано, то выполнить инициализацию
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to
                    // the first page
                    Window.Current.Content = _shell?.Value ?? new Frame();
                    // Задать обработчик ошибок навигации
                    NavigationService.NavigationFailed += (sender, e) =>
                    {
                        throw e.Exception;
                    };
                    // Задать обработчик события "после выполнения навигации"
                    NavigationService.Navigated += Frame_Navigated;
                    // Если задано текущее представление, то ...
                    if (SystemNavigationManager.GetForCurrentView() != null)
                    {
                        // Задать обработчик события "Запрос возврата назад"
                        SystemNavigationManager.GetForCurrentView().BackRequested +=
                            ActivationService_BackRequested;
                    }
                }
            }

            // Получает обработчик Сервиса "сохранить и восстановить"
            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            /// Если обработчик Сервиса "сохранить и восстановить" существует, то ...
            if (activationHandler != null)
            {
                // Обработать 
                await activationHandler.HandleAsync(activationArgs);
            }

            // Если запуск интерактивный, то ... 
            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }

                // Ensure the current window is active
                // Активировать текущее окно
                Window.Current.Activate();

                // Tasks after activation
                // Выполнить задачи после активации
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
            // Показ сообщения при первом запуске, если задано
            await FirstRunDisplayService.ShowIfAppropriateAsync();
            // Показ собщения об обновлениях, если задано
            await WhatsNewDisplayService.ShowIfAppropriateAsync();
            // Ожидание окончания задач (получение списка успешно завершённых задач)
            await Task.CompletedTask;
        }

        /// <summary>
        /// Метод возвращает список экземпляров сервисов
        /// </summary>
        /// <returns>Один экземпляр сервиса при каждом обращении</returns>
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<LiveTileService>.Instance;
            yield return Singleton<AzureNotificationsService>.Instance;
            yield return Singleton<ToastNotificationsService>.Instance;
            yield return Singleton<BackgroundTaskService>.Instance;
            yield return Singleton<SuspendAndResumeService>.Instance;
        }

        /// <summary>
        /// Метод проверки аргумента на использование Интерфейса активации
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool IsInteractive(object args)
        {
            // Возвращает "истина", если объект реализует интерфейс 
            return args is IActivatedEventArgs;
        }

        /// <summary>
        /// Обработчик события "Навигация выполнена"
        /// </summary>
        /// <param name="sender">Инициатор события</param>
        /// <param name="e">Аргументы события навигации</param>
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            // Задание видимости/скрытости кнопки "Назад"
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = NavigationService.CanGoBack ?
                AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// Обработчик события "Запрос возврата назад" 
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="e">Аргументы события запроса возврата</param>
        private void ActivationService_BackRequested(object sender, BackRequestedEventArgs e)
        {
            // Если Сервис навигации может выполниь возврат, то ...
            if (NavigationService.CanGoBack)
            {
                // Вкрнутся назад
                NavigationService.GoBack();
                // Установить, сто запрос обработан
                e.Handled = true;
            }
        }
    }
}
