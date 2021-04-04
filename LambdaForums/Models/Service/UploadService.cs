using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LambdaForums.Models.Service
{
    public class UploadService : IUpload                         // клас UploadService наслідує інтерфейс IUpload
    {
        public CloudBlobContainer GetBlobContainer(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);   // сховище Microsoft Azure
            var blobClient = storageAccount.CreateCloudBlobClient();            // обліковий запис клієнта
            return blobClient.GetContainerReference(containerName);             // посилання на контейнер де будуть зображення (контейнер з зображеннями на Azure)

        }
    }
}
