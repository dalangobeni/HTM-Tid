using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class AppErrorPicture
    {
        [PrimaryKey]
        public Guid AppErrorGuid { get; set; }
        public string Picture { get; set; }

    }
}
