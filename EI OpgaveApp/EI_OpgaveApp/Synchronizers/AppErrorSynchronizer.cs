using EI_OpgaveApp.Database;
using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Synchronizers
{
    public class AppErrorSynchronizer
    {
        bool done;
        List<AppError> appList;
        List<AppError> onlineList;

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;

        public async void DeleteAndPopulateDb()
        {
            List<AppError> onlineList = new List<AppError>();
            List<AppError> oldlist = await App.Database.GetAppErrorsAsync();
            await App.Database.DeleteAllTimeReg();
            List<AppError> taskList = await App.Database.GetAppErrorsAsync();
            if (!taskList.Any())
            {
                var es = await facade.AppErrorService.GetAppErrors();

                foreach (var item in es)
                {
                    await App.Database.SaveAppErrorAsync(item);
                    onlineList.Add(item);
                }
            }
        }
        public async void SyncDatabaseWithNAV()
        {
            done = false;
            appList = await App.Database.GetAppErrorsAsync();
            try
            {
                while (!done)
                {
                    var es = await facade.AppErrorService.GetAppErrors();
                    onlineList = new List<AppError>();

                    foreach (var item in es)
                    {
                        onlineList.Add(item);
                    }

                    CheckIfDeleted();
                    CreateAppError();
                    CheckForConflicts();
                    GetAppErrors();
                    SyncPictures();
                }
            }
            catch
            {
                done = true;
            }
        }

        private async void SyncPictures()
        {
            try
            {
                List<AppErrorPicture> pictureList = await db.GetAppErrorPictures();
                foreach (AppErrorPicture item in pictureList)
                {
                    await facade.PDFService.PostPictureAppError(item, item.AppErrorGuid);
                    await db.DeleteAppErrorPicture(item);
                }
            }
            catch
            {
                Debug.WriteLine("error no stuff");
            }
        }

        private void GetAppErrors()
        {
            foreach (var onlineAppError in onlineList)
            {
                onlineAppError.Sent = true;
                onlineAppError.New = false;
                db.SaveAppErrorAsync(onlineAppError);
            }
        }

        private async void CheckIfDeleted()
        {
            foreach (var appError in appList)
            {
                int matches = 0;
                foreach (var onlineAppError in onlineList)
                {
                    if (appError.Guid == onlineAppError.Guid)
                    {
                        matches++;
                    }
                }
                if (matches == 0 && appError.Sent && !appError.New)
                {
                    await db.DeleteAppErrorAsync(appError);
                }
            }
        }

        private async void CreateAppError()
        {
            foreach (var appError in appList)
            {
                int matches = 0;
                foreach (var onlineAppError in onlineList)
                {
                    if (appError.Guid == onlineAppError.Guid)
                    {
                        matches++;
                    }
                }
                if (matches == 0 && !appError.Sent && appError.New)
                {
                    appError.Sent = true;
                    appError.New = false;
                    await db.UpdateAppErrorAsync(appError);
                    await facade.AppErrorService.CreateAppError(appError);

                }
            }
            done = true;
        }
        private async void CheckForConflicts()
        {
            foreach (AppError onlineAppError in onlineList)
            {
                foreach (AppError appError in appList)
                {
                    if ((appError.Guid == onlineAppError.Guid) && (appError.ETag != onlineAppError.ETag))
                    {
                        await App.Database.UpdateAppErrorAsync(onlineAppError);
                    }
                }
            }
        }
    }
}

