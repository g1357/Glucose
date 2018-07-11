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
    public class Record
    {
        /// <summary>
        /// Дата и время измерения
        /// </summary>
        public DateTime Data_Time;
        /// <summary>
        /// Значение параметра
        /// </summary>
        public decimal Value;
        /// <summary>
        /// Измеряемый параметр
        /// </summary>
        public int TypeId;
        /// <summary>
        /// Вид измерения
        /// </summary>
        public int KindId;
        /// <summary>
        /// Примечания
        /// </summary>
        public string Remark;
    }
}
