using EI_OpgaveApp.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class JobRecLine
    {
        [PrimaryKey]
        public Guid TimeRegGUID { get; set; }
        public string UniqueID { get; set; }
        public string TaskNo { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public string Status { get; set; }
        public bool Sent { get; set; }
        public bool Transferred { get; set; }
        public string Resource_Person { get; set; }
        public string Resource_Machine { get; set; }
        //public int Time_Spent { get; set; }
        public string Work_Type { get; set; }
        public Guid TaskGUID { get; set; }
        public string ETag { get; set; }
        public bool Edited { get; set; }
        public bool SentFromApp { get; set; }
        public bool New { get; set; }
        public bool IsRunning { get; set; }
        public DateTime Posting_Date { get; set; }
        public string Work_Type_And_Machine
        {
            get
            {
                MaintenanceDatabase db = App.Database;
                WorkType wt = db.GetWorkTypeAsync(Work_Type).Result;

                if (Resource_Machine == "" || Resource_Machine == null)
                {
                    return wt.Description;
                }
                else
                {
                    Resources rm = db.GetResourceAsync(Resource_Machine).Result;
                    return wt.Description + " på " + rm.Name;
                }
            }
        }
    }
}