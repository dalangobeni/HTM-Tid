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
    public class FixedAssetSynchronizer
    {
        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        bool done;
        List<FixedAsset> appList;
        List<FixedAsset> onlineList;
        public async void SyncDatabaseWithNAV()
        {
            done = false;
            appList = await App.Database.GetFixedAssets();
            try
            {
                while (!done)
                {
                    var es = await facade.FixedAssetService.GetFixedAssets();
                    onlineList = new List<FixedAsset>();

                    foreach (var item in es)
                    {
                        onlineList.Add(item);
                    }
                    GetAppErrors();
                }
            }
            catch
            {
                done = true;
            }
        }

        private void GetAppErrors()
        {
            foreach (var onlineAppError in onlineList)
            {
                db.SaveFixedAssetAsync(onlineAppError);
            }
        }
    }
}
