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
    public class ResourcesSynchronizer
    {
        List<Resources> onlineList;
        List<Resources> localList;
        

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        public async void SyncDatabaseWithNAV()
        {
            localList = new List<Resources>();
            onlineList = new List<Resources>();

            try
            {
                var res = await facade.ResourcesService.GetResourcesAsync();
                foreach (var item in res)
                {
                    onlineList.Add(item);
                }
                localList = await db.GetResourcesAsync();
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
                            await db.DeleteResourcesAsync(item);
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
            bool match = false;
            foreach (var item in onlineList)
            {
                foreach (var item2 in localList)
                {
                    if (item.No == item2.No && item.ETag != item2.ETag)
                    {
                        db.UpdateResourcesAsync(item);
                    }
                    else if (item.No == item2.No)
                    {
                        match = true;
                    }
                }
                if (!match)
                {
                    db.SaveResourcesAsync(item);
                }
            }
        }

        private async void RemoveDeletedWorkTypes()
        {
            foreach (var item in localList)
            {
                if (facade.WorkTypeService.GetWorkTypeAsync(item.No) == null)
                {
                    await db.DeleteResourcesAsync(item);
                };
            }
        }

        private async void PopulateWorkTypes()
        {
            var onlineResources = await facade.ResourcesService.GetResourcesAsync();
            foreach (var item in onlineResources)
            {
                await db.SaveResourcesAsync(item);
            }
        }
    }
}
