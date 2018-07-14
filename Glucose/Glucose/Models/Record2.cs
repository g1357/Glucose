using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glucose.Models
{
    /// <summary>
    /// Запись об единичном измерении одного параметра
    /// </summary>
    public class Record2
    {
        /// <summary>
        /// Дата и время измерения
        /// </summary>
        public DateTime Date_Time { get; set; }
        /// <summary>
        /// Значение параметра
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Измеряемый параметр
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Вид измерения
        /// </summary>
        public string Kind { get; set; }
        /// <summary>
        /// Примечания
        /// </summary>
        public string Remark { get; set; }
    }
}
