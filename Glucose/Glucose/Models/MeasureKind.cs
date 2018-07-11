using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glucose.Models
{
    /// <summary>
    /// Вид измерения.
    /// (В каких условия произведено измерение)
    /// 1. Утро - после подъёма, перед едой
    /// 2. После фитнеса - сразу после занятий фитнесом, спортом
    /// 3. После завтрака - через 2 часа после завтрака
    /// 4. После обеда - через 2 часа после обеда
    /// 5. Поле ужина - через 2 часа после ужина
    /// 6. После бани - сразу после бани
    /// 7. После кардионагрузки - сразу после велотренажёра или беговой дорожки
    /// 8. После работы - после приезда с работы, если не обедал
    /// 9. Без вида - не задано значение вида измерения
    /// </summary>
    public class MeasurementKind
    {
        /// <summary>
        /// Идентификатор вида измерения
        /// </summary>
        public int KindId;
        /// <summary>
        /// Наименование вида измерения
        /// </summary>
        public string KindName;
        /// <summary>
        /// Описание вида измерения
        /// </summary>
        public string KindDescription;
        static public List<MeasurementKind> Data =
            new List<MeasurementKind>
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
            };
    }
}
