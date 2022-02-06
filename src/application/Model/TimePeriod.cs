using System;

namespace Strover.Models
{
    /// <summary>
    /// A representation of a period in time. The period is formed by two moments in time.
    /// These moments are both included in the period. 
    /// </summary>
    public class TimePeriod
    {
        public override String ToString()
        {
            // Day + Month, from StartHour to EndHour
            var startDateSerialized = Start.ToString("d MMMM yyyy, Hu");
            var endDateSerialized = End.ToString("Hu");
            return $"{startDateSerialized} - {endDateSerialized}";
        }

        /// <summary>
        /// Moment at which the time period begins.  
        /// </summary>
        /// <value></value>
        public DateTime Start { get; set; }

        /// <summary>
        /// Moment at which the time period ends.  
        /// </summary>
        /// <value></value>
        public DateTime End { get; set; }
    }
}