using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glucose.Models
{
    /// <summary>
    /// Тип измерения.
    /// (Измеряемый параметр)
    /// 1. Сахар (глюкоза)
    /// 2. Вес
    /// 3. Холестерин
    /// 4. Мочевая кислота
    /// 5. Гемоглобин
    /// </summary>
    public class MeasurementType
    {
        /// <summary>
        /// Идентификатор типа измерения
        /// </summary>
        public int TypeId;
        /// <summary>
        /// Наименование типа измерения
        /// </summary>
        public string TypeName;
        /// <summary>
        /// Описание типа измерения
        /// </summary>
        public string TypeDescription;
        static public List<MeasurementType> Data =
            new List<MeasurementType>
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
            };
    }
}
