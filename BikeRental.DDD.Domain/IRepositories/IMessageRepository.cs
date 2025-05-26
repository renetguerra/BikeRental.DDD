using BikeRental.DDD.Domain.DTOs.Message;
using BikeRental.DDD.Domain.Entities;
using BikeRental.DDD.Domain.Helpers;
using Newtonsoft.Json.Linq;

namespace BikeRental.DDD.Domain.IRepositories
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);
        void Update(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PagedList<MessageDTO>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUserName, string recipientUserName); 
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
    }
}