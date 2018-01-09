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
    public class PictureSynchronizer
    {
        List<PictureModel> pictureList;

        ServiceFacade facade = ServiceFacade.GetInstance;
        MaintenanceDatabase database = App.Database;

        public async void PutPicturesToNAV()
        {
            try
            {
                pictureList = await database.GetPicturesAsync();
                foreach (PictureModel item in pictureList)
                {
                    await facade.PDFService.PostPicture(item);
                    Debug.WriteLine(item.Picture);
                    await facade.PDFService.PostPictureJobReg(item, item.id);
                    await database.DeletePictureAsync(item);
                }
            }
            catch
            {
                Debug.WriteLine("error no stuff");
            }
        }
    }
}