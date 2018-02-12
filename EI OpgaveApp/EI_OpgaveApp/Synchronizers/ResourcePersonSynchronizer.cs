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
    public class ResourcePersonSynchronizer
    {

        List<ResourcePerson> onlineList;
        List<ResourcePerson> localList;

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        public async void SyncDatabaseWithNAV()
        {
            localList = new List<ResourcePerson>();
            onlineList = new List<ResourcePerson>();

            try
            {
                var res = await facade.ResourcePersonService.GetResroucePersons();
                foreach (var item in res)
                {
                    onlineList.Add(item);
                }
                localList = await db.GetResourcePersonsAsync();
                if (localList == null || localList.Count() == 0)
                {
                    PopulateWorkTypes();
                }

                else
                {
                    if (onlineList == null || onlineList.Count() == 0)
                    {
                        foreach (var item in localList)
                        {
                            await db.DeleteResourcePersonAsync(item);
                        }
                    }
                    else
                    {
                        SaveNewWorkTypes();
                        RemoveDeletedWorkTypes();
                    }
                }
            }
            catch { }
        }

        private void SaveNewWorkTypes()
        {

            foreach (var item in onlineList)
            {
                bool match = false;
                foreach (var item2 in localList)
                {
                    if (item.No == item2.No && item.ETag != item2.ETag)
                    {
                        db.UpdateResourcePersonAsync(item);
                    }
                    else if (item.No == item2.No)
                    {
                        match = true;
                    }
                }
                if (!match)
                {
                    db.SaveResourcePersonAsync(item);
                }
            }
        }

        private async void RemoveDeletedWorkTypes()
        {
            foreach (var item in localList)
            {
                int matches = 0;
                foreach (var onlineItem in onlineList)
                {
                    if (item.No == onlineItem.No)
                    {
                        matches++;
                    }
                }
                if (matches == 0)
                {
                    await App.Database.DeleteResourcePersonAsync(item);
                }
            }
        }

        private async void PopulateWorkTypes()
        {
            var onlineResourcePersons = await facade.ResourcePersonService.GetResroucePersons();
            foreach (var item in onlineResourcePersons)
            {
                await db.SaveResourcePersonAsync(item);
            }
        }
    }
}
