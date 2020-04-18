using RedditBot.Models.Domain;
using System.Collections.Generic;

namespace RedditBot.Models.Repositories {
    public interface IMessageRepository {
        List<Message> GetAllMessages();
        Message GetMessage(ulong id);
        void RemoveMessage(ulong id);
        void AddMessage(Message msg);
        bool Exist(ulong id);
    }
}
