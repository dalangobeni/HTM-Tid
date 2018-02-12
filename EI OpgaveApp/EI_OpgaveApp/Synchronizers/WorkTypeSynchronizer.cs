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
    public class WorkTypeSynchronizer
    {
        List<WorkType> onlineList;
        List<WorkType> localList;

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        public async void SyncDatabaseWithNAV()
        {
            localList = new List<WorkType>();
            onlineList = new List<WorkType>();

            try
            {
                var res = await facade.WorkTypeService.GetWorkTypesAsync();
                foreach (var item in res)
                {
                    onlineList.Add(item);
                }
                localList = await db.GetWorkTypesAsync();
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
                            await db.DeleteWorkTypeAsync(item);
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
                    if (item.Code == item2.Code && item.ETag != item2.ETag)
                    {
                        db.UpdateWorkTypeAsync(item);
                    }
                    else if (item.Code == item2.Code)
                    {
                        match = true;
                    }
                }
                if (!match)
                {
                    db.SaveWorkTypeAsync(item);
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
                    if (item.Code == onlineItem.Code)
                    {
                        matches++;
                    }
                }
                if (matches == 0)
                {
                    await App.Database.DeleteWorkTypeAsync(item);
                }
            }
        }

        private async void PopulateWorkTypes()
        {
            var onlineWorkTypes = await facade.WorkTypeService.GetWorkTypesAsync();
            foreach (var item in onlineWorkTypes)
            {
                await db.SaveWorkTypeAsync(item);
            }
        }
    }
}
