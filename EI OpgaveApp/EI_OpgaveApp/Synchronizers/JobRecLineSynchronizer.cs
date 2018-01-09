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
    public class JobRecLineSynchronizer
    {
        List<JobRecLine> onlineList = new List<JobRecLine>();
        List<JobRecLine> jobList = new List<JobRecLine>();

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase db = App.Database;
        public async void SyncDatabaseWithNAV()
        {
            try
            {
                jobList = new List<JobRecLine>();
                onlineList = new List<JobRecLine>();
                jobList = await db.GetJobRecLinesAsync();
                bool done = false;
                while (!done)
                {
                    var s = await facade.JobRecLineService.GetJobRecLines();
                    if (s != null)
                    {
                        foreach (var item in s)
                        {
                            onlineList.Add(item);
                        }
                    }
                    done = true;
                }

                foreach (var item in onlineList)
                {
                    item.New = false;
                    try
                    {
                        await db.SaveJobRecLineAsync(item);
                    }
                    catch
                    {

                    }
                }
                CreateNewJobRecLines();
                UpdateJobRecLines();
                RemoveDoneJobRecLines();
                CheckIfDeleted();
            }
            catch { }
        }

        private void CheckIfDeleted()
        {
            foreach (var item in jobList)
            {
                int i = 0;

                foreach (var s in onlineList)
                {
                    if (item.TimeRegGUID == s.TimeRegGUID)
                    {
                        i++;
                    }
                }

                if (i == 0 && !item.New)
                {
                    db.DeleteJobRecLineAsync(item);
                }
            }
        }

        private void RemoveDoneJobRecLines()
        {
            foreach (var item in jobList)
            {
                foreach (var s in onlineList)
                {
                    if (item.Sent && item.SentFromApp)
                    {
                        db.DeleteJobRecLineAsync(item);
                    }
                }
            }
        }

        private async void UpdateJobRecLines()
        {
            foreach (var item in jobList)
            {
                if (item.Edited)
                {

                    await facade.JobRecLineService.UpdateJobRecLine(item);
                    item.Edited = false;
                    if (item.Sent)
                    {
                        item.SentFromApp = true;
                    }
                    await db.UpdateJobRecLineAsync(item);
                }
            }
        }

        private async void CreateNewJobRecLines()
        {
            foreach (var item in jobList)
            {
                if (item.New && !item.IsRunning)
                {
                    if (await facade.JobRecLineService.CreateJobRecLine(item))
                    {
                        item.New = false;
                        await db.UpdateJobRecLineAsync(item);
                    }
                }
            }
        }
    }
}
