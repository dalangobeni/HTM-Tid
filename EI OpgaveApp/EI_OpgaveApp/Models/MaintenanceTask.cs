using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class MaintenanceTask
    {
        [PrimaryKey]
        public Guid TaskGUID { get; set; }
        public string no { get; set; }
        public string text { get; set; }
        public string etag { get; set; }
        public string status { get; set; }
        public DateTime planned_Date { get; set; }
        public string responsible { get; set; }
        public byte[] image { get; set; }
        public string AppNotes { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime Shipment_Date { get; set; }
        public string Sales_Order_No { get; set; }
        public bool New { get; set; }
        public bool Sent { get; set; }
        public string CustomerNameLocal { get; set; }
        public string Sales_Order_Or_Text
        {
            get
            {
                if (Sales_Order_No == "")
                {
                    return text;
                }
                else
                {
                    return Sales_Order_No;
                }
            }
        }
    }
}
