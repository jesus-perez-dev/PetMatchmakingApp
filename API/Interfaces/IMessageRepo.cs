using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IMessageRepo
    {
        void AddMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<IEnumerable<MessageDto>> GetMessages(string currentUsername);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string receiverUsername);
        Task<bool> SaveChanges();
    }
}