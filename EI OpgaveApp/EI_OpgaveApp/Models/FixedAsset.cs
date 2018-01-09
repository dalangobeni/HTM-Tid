using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class FixedAsset
    {
        [PrimaryKey]
        public string No { get; set; }
        public string Description { get; set; }
        public string ETag { get; set; }
    }
}
