using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class TimeRegStat
    {
        public string User { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public double MonthValue { get; set; }
        public double WeekValue { get; set; }
        public double MondayValue { get; set; }
        public double TuesdayValue { get; set; }
        public double WednesdayValue { get; set; }
        public double ThursdayValue { get; set; }
        public double FridayValue { get; set; }
        public double SaturdayValue { get; set; }
        public double SundayValue { get; set; }
        public double BillableHoursValue { get; set; }
    }
}
