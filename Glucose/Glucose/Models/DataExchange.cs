using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glucose.Models
{
    /// <summary>
    /// Объект для обмена данными приложения
    /// </summary>
    public class DataExchange
    {
        /// <summary>
        /// Версия структуры файла
        /// </summary>
        public string Version;
        /// <summary>
        /// Штамп времени
        /// </summary>
        public string Timespamp;
        /// <summary>
        /// Список типов измерений (параметров)
        /// </summary>
        public List<MeasurementType> TypeList;
        /// <summary>
        /// Список видов измерений (уловий измерения)
        /// </summary>
        public List<MeasurementKind> KindList;
        /// <summary>
        /// Список данных измерения
        /// </summary>
        public List<Record> Data;
    }
}
