using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class PictureModel
    {
        [PrimaryKey,AutoIncrement]
        public int PrimaryId { get; set; }
        public string id { get; set; }
        public string Picture { get; set; }
        public int ActivityNo { get; set; }
        public Guid TaskGuid { get; set; }
    }
}
