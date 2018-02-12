using EI_OpgaveApp.Synchronizers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EI_OpgaveApp.Services
{
    public class ThreadManager
    {
        SynchronizerFacade facade = SynchronizerFacade.GetInstance;
        int i;
        public async void StartSynchronizationThread()
        {
            //i = 0;
            //if (!GlobalData.GetInstance.Done)
            //{
            //    Device.StartTimer(TimeSpan.FromMinutes(5), () =>
            //    {
            //        if (!GlobalData.GetInstance.Done)
            //        {
            //            sync();
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    });
            //}
            while (true)
            {
                if (!GlobalData.GetInstance.Done)
                {
                    Sync();
                }
                await Task.Delay(180000);

            }
        }

        public async void Sync()
        {
            await facade.MaintenanceTaskSynchronizer.SyncDatabaseWithNAV();
            await facade.TimeRegistrationSynchronizer.SyncDatabaseWithNAV();
            await facade.MaintenanceActivitySynchronizer.SyncDatabaseWithNAV();
            facade.JobRecLineSynchronizer.SyncDatabaseWithNAV();
            facade.PictureSynchronizer.PutPicturesToNAV();
            facade.ResourcesSynchronizer.SyncDatabaseWithNAV();
            facade.CustomerSynchronizer.SyncDatabaseWithNAV();
            facade.JobSynchronizer.SyncDatabaseWithNAV();
            facade.JobTaskSynchronizer.SyncDatabaseWithNAV();
            facade.SalesPersonSynchronizer.SyncDatabaseWithNAV();
            facade.AppErrorSynchronizer.SyncDatabaseWithNAV();
            facade.FixedAssetSynchronizer.SyncDatabaseWithNAV();
            facade.WorkTypeSynchronizer.SyncDatabaseWithNAV();
            facade.ResourcePersonSynchronizer.SyncDatabaseWithNAV();
            Debug.WriteLine("SYNCED!!!!!!!!!!!!!!!!!!!!!!" + i);
            i++;
        }
    }
}
