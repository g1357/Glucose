using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Glucose.Helpers;
using Glucose.Models;
using Windows.Storage;

namespace Glucose.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // TODO WTS: Delete this file once your app is using real data.
    public class DataService
    {
        private const string dataFilename = "Data";

        private static DataExchange data = null;

        /// <summary>
        /// Метод: Асинхронное сохранение состояния
        /// </summary>
        /// <returns>Нет</returns>
        public async Task SaveDataAsync()
        {
            await ApplicationData.Current.LocalFolder.SaveAsync(dataFilename, data);
        }

        /// <summary>
        /// Метод: Асинхронное восстановление состояния
        /// </summary>
        /// <returns>Нет</returns>
        //private async Task RestoreDataAsync()
        private static void RestoreData()
        {
            //data = await ApplicationData.Current.LocalFolder.ReadAsync<DataExchange>(dataFilename);
            if (data == null)
            {
                data = new DataExchange
                {
                    Version = "1.0",
                    Timestamp = "20180713210900",
                    TypeList = new List<MeasurementType>
                    {
                        new MeasurementType() {TypeId = 1, TypeName = "Сахар",
                            TypeDescription = "Уровень сахара (глюкозы) в крови" },
                        new MeasurementType() {TypeId = 2, TypeName = "Вес",
                            TypeDescription = "Вес" },
                        new MeasurementType() {TypeId = 3, TypeName = "Холестерин",
                            TypeDescription = "Уровень холестерин в крови" },
                        new MeasurementType() {TypeId = 4, TypeName = "Мочевая кислота",
                            TypeDescription = "Уровень мочевой кислоты в крови" },
                        new MeasurementType() {TypeId = 5, TypeName = "Гемоглобин",
                            TypeDescription = "Уровень гемоглобина в крови" },
                    },
                    KindList = new List<MeasurementKind>
                    {
                        new MeasurementKind() {KindId = 1, KindName = "Утро",
                            KindDescription = "После подъёма, перед едой" },
                        new MeasurementKind() {KindId = 2, KindName = "После фитнеса",
                            KindDescription = "Сразу после занятий фитнесов, спортом" },
                        new MeasurementKind() {KindId = 3, KindName = "После завтрака",
                            KindDescription = "Через 2 часа после завтрака" },
                        new MeasurementKind() {KindId = 4, KindName = "После обеда",
                            KindDescription = "Через 2 часа после обеда" },
                        new MeasurementKind() {KindId = 5, KindName = "После ужина",
                            KindDescription = "Через 2 часа после ужина" },
                        new MeasurementKind() {KindId = 6, KindName = "После бани",
                            KindDescription = "Сразу после бани" },
                        new MeasurementKind() {KindId = 7, KindName = "После кардионагрузки",
                            KindDescription = "Сразу после велотренажёра или беговой дорожки" },
                        new MeasurementKind() {KindId = 8, KindName = "После работы",
                            KindDescription = "После приезда с работы, если на работе на обедал" },
                        new MeasurementKind() {KindId = 9, KindName = "Без вида",
                            KindDescription = "Не задано значение вида измерения" },
                        new MeasurementKind() {KindId = 10, KindName = "Перед обедом",
                            KindDescription = "Перед обедом, после большого перерыва" },
                    },
                    Data = new List<Record>
                    {
                        new Record { Date_Time = new DateTime(2018, 07, 08, 17, 15, 00), Value =  9.3m, TypeId = 1, KindId =10 },
                        new Record { Date_Time = new DateTime(2018, 07, 08, 21, 30, 00), Value =  7.7m, TypeId = 1, KindId =7 },
                        new Record { Date_Time = new DateTime(2018, 07, 09, 06, 10, 00), Value = 11.7m, TypeId = 1, KindId =1 },
                        new Record { Date_Time = new DateTime(2018, 07, 10, 17, 10, 00), Value =  8.7m, TypeId = 1, KindId =8 },
                        new Record { Date_Time = new DateTime(2018, 07, 11, 06, 10, 00), Value = 11.2m, TypeId = 1, KindId =1 },
                        new Record { Date_Time = new DateTime(2018, 07, 11, 18, 40, 00), Value =  8.1m, TypeId = 1, KindId =8 }
                    }
                };
            }
        }


        // TODO WTS: Remove this once your chart page is displaying real data
        public static ObservableCollection<DataPoint> GetChartData()
        {
            //var data = AllOrders().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
            //                      .OrderBy(dp => dp.Category);

            //return new ObservableCollection<DataPoint>(data);
            return null;
        }

        // TODO WTS: Remove this once your grid page is displaying real data
        /// <summary>
        /// Метод возвращает данные для таблицы
        /// </summary>
        /// <returns>Коллекция значений замеров</returns>
        public static ObservableCollection<Record2> GetGridData()
        {
            if (data == null)
            {
                    RestoreData();
            }
            var result = data.Data.FindAll(r => r.TypeId == 1)
                .Join(data.TypeList, r => r.TypeId, t => t.TypeId,
                    (r, t) => new { Date_Time = r.Date_Time, Value = r.Value, Type = t.TypeName, KindId = r.KindId, Remark = r.Remark })
                 .Join(data.KindList, r => r.KindId, k => k.KindId,
                     (r, k) => new Record2{ Date_Time = r.Date_Time, Value = r.Value, Type = r.Type, Kind = k.KindName, Remark = r.Remark }) ;


            return new ObservableCollection<Record2>(result);
        }
    }
}
