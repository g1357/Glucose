using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using CommonServiceLocator;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Glucose.Helpers;
using Glucose.Services;
using Glucose.Views;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Glucose.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private NavigationView _navigationView;
        private NavigationViewItem _selected;
        private ICommand _itemInvokedCommand;

        /// <summary>
        /// Свойство: Возвращает экремпляр Расширенного сервиса навигации
        /// </summary>
        public NavigationServiceEx NavigationService =>
            ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        /// <summary>
        /// Свойство: Выбранный элемент представления навишации
        /// </summary>
        public NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemInvokedCommand =>
            _itemInvokedCommand ?? (_itemInvokedCommand =
                new RelayCommand<NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        /// <summary>
        /// Констрацтор Модели представления оболочки
        /// </summary>
        public ShellViewModel()
        {
        }

        /// <summary>
        /// Метод инициализации Модели представления оболочки
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="navigationView"></param>
        public void Initialize(Frame frame, NavigationView navigationView)
        {
            // Устанавливает поле Представление навигации
            _navigationView = navigationView;
            // Устанавливает фрейм Навигационного сервиса
            NavigationService.Frame = frame;
            // Устанавливает обработчик события "Навигация выполнена" на
            // метод выполненной еавигации фрейма
            NavigationService.Navigated += Frame_Navigated;
        }

        private void OnItemInvoked(NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsViewModel).FullName);
                return;
            }

            var item = _navigationView.MenuItems
                       .OfType<NavigationViewItem>()
                       .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
            NavigationService.Navigate(pageKey);
        }

        /// <summary>
        /// Обработчиксобытия "Навигация выполнена"
        /// </summary>
        /// <param name="sender">Источник</param>
        /// <param name="e">Аргументы события навигации</param>
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            // Если тип источника события является сиранице параметров, то ...
            if (e.SourcePageType == typeof(SettingsPage))
            {
                // Свойтово выбранного элемента представления навигации установить в элемент
                // параметров представления навм=игации
                Selected = _navigationView.SettingsItem as NavigationViewItem;
                // Вернуться
                return;
            }

            // Свойство выьранного \леента представления навигации устанавливаем в выбранный
            // пользователем элемент
            Selected = _navigationView.MenuItems // Выбираем элементы меню
                       .OfType<NavigationViewItem>() // Из них выбираем те у кого тип
                                                     // "Элемент представления навигации"
                       // Выбираем элемент выбранный из меню
                       .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        /// <summary>
        /// Метлод проверяет совпадает ли ключ страницы элемента меню с выбранным ключом страницы
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="sourcePageType"></param>
        /// <returns></returns>
        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            var navigatedPageKey = NavigationService.GetNameOfRegisteredPage(sourcePageType);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == navigatedPageKey;
        }
    }
}
