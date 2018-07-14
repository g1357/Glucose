using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glucose.Models
{
    /// <summary>
    /// Список типов
    /// </summary>
    public static class TypeList
    {
        private static List<MeasurementType> _defTypelist = new List<MeasurementType>
        {
            new MeasurementType { TypeId = 1, TypeName = "Сахар",
                TypeDescription = "Уровень сахара (глюкозы) в крови" },
            new MeasurementType { TypeId = 2, TypeName = "Вес",
                TypeDescription = "Вес" },
            new MeasurementType { TypeId = 3, TypeName = "Холестерин",
                TypeDescription = "Уровень холестерин в крови" },
            new MeasurementType { TypeId = 4, TypeName = "Мочевая кислота",
                TypeDescription = "Уровень мочевой кислоты в крови" },
            new MeasurementType { TypeId = 5, TypeName = "Гемоглобин",
                TypeDescription = "Уровень гемоглобина в крови" }
        };

        private static List<MeasurementType> typelist = _defTypelist;

        public static void SetList(List<MeasurementType> list)
        {
            typelist = list;
        }

        public static string GetName(int id)
        { 
            return typelist.Find(i =>  i.TypeId == id ).TypeName;
        }
        public static string GetDescription(int id)
        {
            return typelist.Find(i => i.TypeId == id).TypeDescription;
        }
    }
}
