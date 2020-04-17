using RedditBot.Models.Domain;
using System.Collections.Generic;

namespace RedditBot.Models.Repositories {
    public interface IMessageRepository {
        public IEnumerable<Message> GetAllMessages();
        public Message GetMessage(ulong id);
        public void RemoveMessage(ulong id);
        public void AddMessage(Message msg);
        public bool Exist(ulong id);
    }
}
