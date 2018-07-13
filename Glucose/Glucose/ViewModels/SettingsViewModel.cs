using System;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Glucose.Helpers;
using Glucose.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Glucose.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help
    // see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsViewModel : ViewModelBase
    {
        public Visibility FeedbackLinkVisibility =>
            Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported() ?
                Visibility.Visible : Visibility.Collapsed;

        private ICommand _launchFeedbackHubCommand;

        public ICommand LaunchFeedbackHubCommand
        {
            get
            {
                if (_launchFeedbackHubCommand == null)
                {
                    _launchFeedbackHubCommand = new RelayCommand(
                        async () =>
                        {
                            // This launcher is part of the Store Services SDK
                            //https://docs.microsoft.com/en-us/windows/uwp/monetize/microsoft-store-services-sdk
                            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
                            await launcher.LaunchAsync();
                        });
                }

                return _launchFeedbackHubCommand;
            }
        }

        /// <summary>
        /// Поле: Элемент Тема
        /// </summary>
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        /// <summary>
        /// Свойство: Элемен Тема
        /// </summary>
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }
            set { Set(ref _elementTheme, value); }
        }

        /// <summary>
        /// Поле: описание версии
        /// </summary>
        private string _versionDescription;

        /// <summary>
        /// Свойство: Описание версии
        /// </summary>
        public string VersionDescription
        {
            get { return _versionDescription; }
            set { Set(ref _versionDescription, value); }
        }

        /// <summary>
        /// Поле: Команда переключения темы
        /// </summary>
        private ICommand _switchThemeCommand;

        /// <summary>
        /// Свойство: Команда переклбчения темы
        /// </summary>
        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }
                return _switchThemeCommand;
            }
        }

        /// <summary>
        /// Конструктор Модели представления параметров
        /// </summary>
        public SettingsViewModel()
        {
        }

        /// <summary>
        /// Метод инициализации модели представления параметров
        /// </summary>
        public void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }

        /// <summary>
        /// Метод формирования описания версии
        /// </summary>
        /// <returns></returns>
        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
