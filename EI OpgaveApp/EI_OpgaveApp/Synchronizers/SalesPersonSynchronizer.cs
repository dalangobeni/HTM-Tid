using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Synchronizers
{
    public class SalesPersonSynchronizer
    {
        List<SalesPerson> onlineList = new List<SalesPerson>();
        List<SalesPerson> localList = new List<SalesPerson>();

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        public async void SyncDatabaseWithNAV()
        {
            try
            {
                bool done = false;
                while (!done)
                {
                    localList = await db.GetSalesPersons();
                    var s = await facade.SalesPersonService.GetSalesPersonsAsync();
                    foreach (var item in s)
                    {
                        onlineList.Add(item);
                    }
                    done = true;
                }
                foreach (var item in onlineList)
                {
                    try
                    {
                        await db.SaveSalesPersonAsync(item);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
