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
    public class CustomerSynchronizer
    {
        List<Customer> onlineList = new List<Customer>();
        List<Customer> customerList = new List<Customer>();

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        public async void SyncDatabaseWithNAV()
        {
            try
            {
                bool done = false;
                while (!done)
                {
                    customerList = await db.GetCustomersAsync();
                    var s = await facade.CustomerService.GetCustomersAsync();
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
                        await db.SaveCustomerAsync(item);
                    }
                    catch
                    {

                    }
                }
            }
            catch { }
        }
    }
}
