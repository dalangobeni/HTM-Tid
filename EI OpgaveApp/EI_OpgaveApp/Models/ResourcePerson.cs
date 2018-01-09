using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class ResourcePerson
    {
        [PrimaryKey]
        public string No { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Search_Name { get; set; }
        public string ETag { get; set; }
    }
}
