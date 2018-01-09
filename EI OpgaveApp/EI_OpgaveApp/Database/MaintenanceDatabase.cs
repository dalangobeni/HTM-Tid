using EI_OpgaveApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Database
{
    public class MaintenanceDatabase
    {
        readonly SQLiteAsyncConnection database;

        public MaintenanceDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<MaintenanceTask>().Wait();
            database.CreateTableAsync<TimeRegistrationModel>().Wait();
            database.CreateTableAsync<MaintenanceActivity>().Wait();
            database.CreateTableAsync<ConnectionSettings>().Wait();
            database.CreateTableAsync<PictureModel>().Wait();
            database.CreateTableAsync<JobRecLine>().Wait();
            database.CreateTableAsync<Resources>().Wait();
            database.CreateTableAsync<Customer>().Wait();
            database.CreateTableAsync<Job>().Wait();
            database.CreateTableAsync<JobTask>().Wait();
            database.CreateTableAsync<RegLinePicture>().Wait();
            database.CreateTableAsync<SalesPerson>().Wait();
            database.CreateTableAsync<AppError>().Wait();
            database.CreateTableAsync<FixedAsset>().Wait();
            database.CreateTableAsync<AppErrorPicture>().Wait();
            database.CreateTableAsync<ResourcePerson>().Wait();
            database.CreateTableAsync<WorkType>().Wait();
        }

        public Task<List<MaintenanceTask>> GetTasksAsync()
        {
            return database.Table<MaintenanceTask>().ToListAsync();
        }

        public Task<MaintenanceTask> GetTaskAsync(string id)
        {
            return database.Table<MaintenanceTask>().Where(i => i.no == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveTaskAsync(MaintenanceTask task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateTaskAsync(MaintenanceTask task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteTaskAsync(MaintenanceTask task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeleteAll()
        {
            return database.ExecuteAsync("delete from " + "MaintenanceTask");
        }
        public Task<List<TimeRegistrationModel>> GetTimeRegsAsync()
        {
            return database.Table<TimeRegistrationModel>().ToListAsync();
        }

        public Task<TimeRegistrationModel> GetTimeRegAsync(int id)
        {
            return database.Table<TimeRegistrationModel>().Where(i => i.No == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveTimeRegAsync(TimeRegistrationModel task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateTimeRegAsync(TimeRegistrationModel task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteTimeRegAsync(TimeRegistrationModel task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeleteAllTimeReg()
        {
            return database.ExecuteAsync("delete from " + "TimeRegistrationModel");
        }

        public Task<List<MaintenanceActivity>> GetAcitivitiesAsync()
        {
            return database.Table<MaintenanceActivity>().ToListAsync();
        }

        public Task<MaintenanceActivity> GetAcitivitiyAsync(int id)
        {
            return database.Table<MaintenanceActivity>().Where(i => i.Task_Activity_No == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateActivityAsync(MaintenanceActivity act)
        {
            // return database.ExecuteAsync("Update MaintenanceActivity SET done=true WHERE Maint_Activity_No=" + act.Maint_Activity_No);
            //database.ExecuteAsync("delete from MaintenanceActivity WHERE Maint_Activity_No =" + act.Maint_Activity_No);
            //return database.InsertAsync(act);
            return database.UpdateAsync(act);
        }
        public Task<int> SaveActivityASync(MaintenanceActivity task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> DeleteAllActivities()
        {
            return database.ExecuteAsync("delete from " + "MaintenanceActivity");
        }
        public Task<int> DeleteActivity(MaintenanceActivity act)
        {
            return database.DeleteAsync(act);
        }
        public Task<ConnectionSettings> GetConnectionSetting(int id)
        {
            return database.Table<ConnectionSettings>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> UpdateConnectionSetting(ConnectionSettings act)
        {
            return database.UpdateAsync(act);
        }
        public Task<int> SaveConnectionSetting(ConnectionSettings task)
        {
            return database.InsertAsync(task);
        }
        public Task<List<PictureModel>> GetPicturesAsync()
        {
            return database.Table<PictureModel>().ToListAsync();
        }

        public Task<PictureModel> GetPictureAsync(string id)
        {
            return database.Table<PictureModel>().Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public Task<int> SavePictureAsync(PictureModel task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdatePictureAsync(PictureModel task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeletePictureAsync(PictureModel task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeletePictureAsync()
        {
            return database.ExecuteAsync("delete from " + "PictureModel");
        }

        public Task<List<JobRecLine>> GetJobRecLinesAsync()
        {
            return database.Table<JobRecLine>().ToListAsync();
        }
      
        public Task<JobRecLine> GetJobRecLineAsync(Guid id)
        {
            return database.Table<JobRecLine>().Where(i => i.TimeRegGUID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveJobRecLineAsync(JobRecLine task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateJobRecLineAsync(JobRecLine task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteJobRecLineAsync(JobRecLine task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeleteJobRecLinesAsync()
        {
            return database.ExecuteAsync("delete from " + "JobRecLine");
        }
        public Task<List<Resources>> GetResourcesAsync()
        {
            return database.Table<Resources>().ToListAsync();
        }

        public Task<Resources> GetResourceAsync(string id)
        {
            return database.Table<Resources>().Where(i => i.No == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveResourcesAsync(Resources task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateResourcesAsync(Resources task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteResourcesAsync(Resources task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeleteResourcesAsync()
        {
            return database.ExecuteAsync("delete from " + "Resources");
        }
        public Task<List<Customer>> GetCustomersAsync()
        {
            return database.Table<Customer>().ToListAsync();
        }

        public Task<Customer> GetCustomerAsync(string id)
        {
            return database.Table<Customer>().Where(i => i.No == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveCustomerAsync(Customer task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateCustomerAsync(Customer task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteCustomerAsync(Customer task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeleteCustomersAsync()
        {
            return database.ExecuteAsync("delete from " + "Customer");
        }
        public Task<List<Job>> GetJobsAsync()
        {
            return database.Table<Job>().ToListAsync();
        }

        public Task<Job> GetJobAsync(string id)
        {
            return database.Table<Job>().Where(i => i.No == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveJobAsync(Job task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateJobAsync(Job task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteJobAsync(Job task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeleteAllJobs()
        {
            return database.ExecuteAsync("delete from " + "Job");
        }
        public Task<List<JobTask>> GetJobTasksAsync()
        {
            return database.Table<JobTask>().ToListAsync();
        }

        public Task<JobTask> GetJobTask(string id)
        {
            return database.Table<JobTask>().Where(i => i.Job_Task_No == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveJobTaskAsync(JobTask task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateJobTaskAsync(JobTask task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteJobTaskAsync(JobTask task)
        {
            return database.DeleteAsync(task);
        }

        public Task<int> DeleteAllJobTasks()
        {
            return database.ExecuteAsync("delete from " + "JobTask");
        }
        public Task<List<SalesPerson>> GetSalesPersons()
        {
            return database.Table<SalesPerson>().ToListAsync();
        }
        public Task<SalesPerson> GetSalesPerson(string id)
        {
            return database.Table<SalesPerson>().Where(i => i.Code == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveSalesPersonAsync(SalesPerson task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateSalesPersonAsync(SalesPerson task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteSalesPersonAsync(SalesPerson task)
        {
            return database.DeleteAsync(task);
        }
        public Task<List<AppError>> GetAppErrorsAsync()
        {
            return database.Table<AppError>().ToListAsync();
        }
        public Task<AppError> GetAppErrorAsync(Guid id)
        {
            return database.Table<AppError>().Where(i => i.Guid == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveAppErrorAsync(AppError task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateAppErrorAsync(AppError task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteAppErrorAsync(AppError task)
        {
            return database.DeleteAsync(task);
        }
        public Task<List<FixedAsset>> GetFixedAssets()
        {
            return database.Table<FixedAsset>().ToListAsync();
        }
        public Task<FixedAsset> GetAFixedAssetAsync(string id)
        {
            return database.Table<FixedAsset>().Where(i => i.No == id).FirstOrDefaultAsync();
        }
        public Task<int> SaveFixedAssetAsync(FixedAsset task)
        {
            return database.InsertAsync(task);
        }
        public Task<int> UpdateFixedAssetAsync(FixedAsset task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteFixedAssetAsync(FixedAsset task)
        {
            return database.DeleteAsync(task);
        }
        public Task<List<AppErrorPicture>> GetAppErrorPictures()
        {
            return database.Table<AppErrorPicture>().ToListAsync();
        }
        public Task<int> SaveAppErrorPicture(AppErrorPicture pic)
        {
            return database.InsertAsync(pic);
        }
        public Task<int> DeleteAppErrorPicture(AppErrorPicture pic)
        {
            return database.DeleteAsync(pic);
        }

        public Task<List<ResourcePerson>> GetResourcePersonsAsync()
        {
            return database.Table<ResourcePerson>().ToListAsync();
        }

        public Task<ResourcePerson> GetResourcePersonAsync(string id)
        {
            return database.Table<ResourcePerson>().Where(i => i.No == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveResourcePersonAsync(ResourcePerson task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateResourcePersonAsync(ResourcePerson task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteResourcePersonAsync(ResourcePerson task)
        {
            return database.DeleteAsync(task);
        }
        public Task<List<WorkType>> GetWorkTypesAsync()
        {
            return database.Table<WorkType>().ToListAsync();
        }

        public Task<WorkType> GetWorkTypeAsync(string id)
        {
            return database.Table<WorkType>().Where(i => i.Code == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveWorkTypeAsync(WorkType task)
        {
            return database.InsertAsync(task);
        }

        public Task<int> UpdateWorkTypeAsync(WorkType task)
        {
            return database.UpdateAsync(task);
        }
        public Task<int> DeleteWorkTypeAsync(WorkType task)
        {
            return database.DeleteAsync(task);
        }
    }
}
