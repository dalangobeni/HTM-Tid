using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class WorkType
    {
        [PrimaryKey]
        public string Code { get; set; }
        public string Description { get; set; }
        public string Unit_of_Measure_Code { get; set; }
        public string ETag { get; set; }
        public string Code_And_Description
        {
            get
            {
                if (Code != "" && Description != "")
                {
                    return string.Format("{0} - {1}", Code, Description);
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
