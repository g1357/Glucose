using System;
using System.Diagnostics;
using System.IO;
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
                new string[] { "onedrive.readwrite", "wl.signin", "wl.offline_access" });
            await msaAuthProvider.AuthenticateUserAsync();

            var session = msaAuthProvider.CurrentAccountSession;
            Debug.WriteLine($"AccessToken: {session.AccessToken}");
            Debug.WriteLine($"CanRefresh: {session.CanRefresh}");
            Debug.WriteLine($"RefreshToken: {session.RefreshToken}");
            Debug.WriteLine($"ExpiresOnUtc: {session.ExpiresOnUtc}");

            var _refreshToken = session.RefreshToken;

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

            var item1 = await oneDriveClient.Drive.Items["A13352E228C6410D!145135"].Request().GetAsync();

            var builder = oneDriveClient.Drive.Root.ItemWithPath("file.txt");
            var file = await builder
              .Request()
              .GetAsync();
            var contentStream = await builder.Content
              .Request()
              .GetAsync();
            Debug.WriteLine($"Content for file {file.Name}:");
            using (var reader = new StreamReader(contentStream))
            {
                Debug.WriteLine(reader.ReadToEnd());
            }

            AccountSession accSession = new AccountSession();
            accSession.ClientId = ClientId;
            accSession.RefreshToken = _refreshToken;
            var _msaAuthProvider = new MsaAuthenticationProvider(
                ClientId,
                ReturnURI,
                new string[] { "onedrive.readwrite", "wl.signin", "wl.offline_access" });
            var _oneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", _msaAuthProvider);
            _msaAuthProvider.CurrentAccountSession = accSession;
            await _msaAuthProvider.AuthenticateUserAsync();
            var _builder = _oneDriveClient.Drive.Root.ItemWithPath("file.txt");
            var _file = await builder
              .Request()
              .GetAsync();
            var _contentStream = await _builder.Content
              .Request()
              .GetAsync();
            Debug.WriteLine($"Content for file {_file.Name}:");
            using (var _reader = new StreamReader(_contentStream))
            {
                Debug.WriteLine(_reader.ReadToEnd());
            }

        }

        private void DoTest2()
        {
            var scopes = new[] { "onedrive.readwrite", "onedrive.appfolder", "wl.signin" };
            //var _client = 
        }


    }
}
