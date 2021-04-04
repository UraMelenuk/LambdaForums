using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public interface IUpload                              // інтерфейс IUpload (загрузка зображення на хмару Azure)
    {
        CloudBlobContainer GetBlobContainer(string connectionString, string containerName);
    }
}
