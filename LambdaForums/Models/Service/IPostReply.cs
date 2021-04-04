using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.Service
{
    public interface IPostReply                         // інтерфейс IPostReply
    {
        PostReply GetById(int id);                      // id
        Task Update(int id, string message);            // update редагувати відповідь по id
        Task Delete(int id);                            // delete видалити відповідь
    }
}
