using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class MaintenanceActivity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public Guid TaskGUID { get; set; }
        public int Task_Activity_No { get; set; }
        public string Activity_Description { get; set; }
        public double Reading { get; set; }
        public bool Done { get; set; }
        public bool Alarm { get; set; }
        public string Status { get; set; }
        //public string UniqueID { get; set; }
        public double Min_Reading { get; set; }
        public double Max_Reading { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime DoneTime { get; set; }
        public string ETag { get; set; }
    }
}


