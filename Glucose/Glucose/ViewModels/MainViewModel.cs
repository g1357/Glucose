using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;

namespace Glucose.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
        }

        /// <summary>
        /// Поле: Команда проверки работы с OneDrive
        /// </summary>
        private ICommand _testOneDrive;

        /// <summary>
        /// Свойство: Команда проверки работы с OneDrive
        /// </summary>
        public ICommand TestOneDrive
        {
            get
            {
                if (_testOneDrive == null)
                {
                    _testOneDrive = new RelayCommand(
                        async () =>
                        {
                            await DoTestOneDrive();
                        });
                }
                return _testOneDrive;
            }
        }

        // https://apps.dev.microsoft.com/
        private static string ClientId = "0bc1162e-e51f-415e-b723-79d0ba6b0208";
        private static string ReturnURI = "msal0bc1162e-e51f-415e-b723-79d0ba6b0208://auth";
        private async Task DoTestOneDrive()
        {
            var msaAuthProvider = new MsaAuthenticationProvider(
                ClientId,
                ReturnURI,
                new string[] { "onedrive.readwrite", "wl.signin" });
            await msaAuthProvider.AuthenticateUserAsync();
            var oneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", msaAuthProvider);

            var drive = await oneDriveClient
                         .Drive
                         .Request()
                         .GetAsync();

            var rootItem = await oneDriveClient
                            .Drive
                            .Root
                            .Request()
                            .GetAsync();

            var items = await oneDriveClient.Drive.Root.Children.Request().GetAsync();

            var item1 = await oneDriveClient.Drive.Items["Документ1.docx"].Request().GetAsync();
        }

    }
}
