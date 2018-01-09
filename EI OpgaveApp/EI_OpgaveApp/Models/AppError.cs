using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class AppError
    {
        [PrimaryKey]
        public Guid Guid { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTime Time_Logged { get; set; }
        public DateTime Time_Saved { get; set; }
        public string Fixed_Asset { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public string ETag { get; set; }
        public bool New { get; set; }
        public bool Sent { get; set; }

    }
}
