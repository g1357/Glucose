using System;

namespace Glucose.Services
{
    /// <summary>
    /// Состояние приостановки
    /// </summary>
    public class SuspensionState
    {
        public object Data { get; set; }

        public DateTime SuspensionDate { get; set; }
    }
}
