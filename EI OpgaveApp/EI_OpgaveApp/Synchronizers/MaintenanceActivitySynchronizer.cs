using EI_OpgaveApp.Models;
using EI_OpgaveApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Synchronizers
{
    public class MaintenanceActivitySynchronizer
    {
        bool done;
        List<MaintenanceActivity> timeList;
        List<MaintenanceActivity> onlineList;
        ServiceFacade facade = ServiceFacade.GetInstance;

        int numberOfConflicts;
        int numberOfMatches;
        int numberOfNewTasks;

        public async void DeleteAndPopulateDb()
        {
            List<MaintenanceActivity> onlineList = new List<MaintenanceActivity>();
            List<MaintenanceActivity> oldlist = await App.Database.GetAcitivitiesAsync();
            await App.Database.DeleteAllActivities();
            List<MaintenanceActivity> taskList = await App.Database.GetAcitivitiesAsync();
            if (!taskList.Any())
            {
                var es = await facade.MaintenanceActivityService.GetMaintenanceActivitiesAsync();

                foreach (var item in es)
                {
                    await App.Database.SaveActivityASync(item);
                    onlineList.Add(item);
                }
            }
        }
        public async Task<bool> SyncDatabaseWithNAV()
        {
            done = false;
            timeList = await App.Database.GetAcitivitiesAsync();
            try
            {
                var es = await facade.MaintenanceActivityService.GetMaintenanceActivitiesAsync();
                onlineList = new List<MaintenanceActivity>();

                foreach (var item in es)
                {
                    onlineList.Add(item);
                }
                CheckIfFinishedOrDeleted();
                PutDoneTimeRegsToNAV();
                CheckForConflicts();
                CheckForNewTasks();
            }
            catch
            {
                done = true;
            }
            return done;
        }

        private async void PutDoneTimeRegsToNAV()
        {
            foreach (MaintenanceActivity onlineTimeReg in onlineList)
            {
                foreach (MaintenanceActivity timeReg in timeList)
                {
                    if ((timeReg.TaskGUID == onlineTimeReg.TaskGUID) && (timeReg.Task_Activity_No == onlineTimeReg.Task_Activity_No) && (timeReg.ETag == onlineTimeReg.ETag))
                    {
                        if (timeReg.Done && !onlineTimeReg.Done)
                        {
                            await facade.MaintenanceActivityService.UpdateTask(timeReg);
                        }
                    }
                }
            }
        }
        private async void CheckForNewTasks()
        {
            foreach (MaintenanceActivity onlineTask in onlineList)
            {
                foreach (MaintenanceActivity task in timeList)
                {
                    if (task.TaskGUID == onlineTask.TaskGUID && task.Task_Activity_No == onlineTask.Task_Activity_No)
                    {
                        numberOfMatches++;
                    }
                }
                if (numberOfMatches == 0)
                {
                    await App.Database.SaveActivityASync(onlineTask);
                    numberOfNewTasks++;
                }
                numberOfMatches = 0;
            }
        }
        private async void CheckForConflicts()
        {
            foreach (MaintenanceActivity onlineTask in onlineList)
            {
                foreach (MaintenanceActivity task in timeList)
                {
                    if ((task.TaskGUID == onlineTask.TaskGUID) && (task.Task_Activity_No == onlineTask.Task_Activity_No) && (task.ETag != onlineTask.ETag))
                    {
                        numberOfConflicts++;
                        await App.Database.UpdateActivityAsync(onlineTask);
                    }
                }
            }
        }

        private async void CheckIfFinishedOrDeleted()
        {
            foreach (MaintenanceActivity task in timeList)
            {
                int matches = 0;
                foreach (MaintenanceActivity onlineTask in onlineList)
                {
                    if ((task.TaskGUID == onlineTask.TaskGUID) && (task.Task_Activity_No == onlineTask.Task_Activity_No))
                    {
                        matches++;
                    }
                }
                if (matches == 0)
                {
                    await App.Database.DeleteActivity(task);
                }
            }
        }
    }
}