using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RedditBot.Models.Domain;
using RedditBot.Models.Repositories;
using System.Linq;
using System;

namespace RedditBot.Data.Repositories {
    public class MessageRepository : IMessageRepository {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Message> _messages;

        public MessageRepository(ApplicationDBContext context) {
            _context = context;
            _messages = context.Messages;
        }

        public void AddMessage(Message msg) {
            if (Exist(msg.MessageID))
                throw new Exception("this message is already in the database");

            _messages.Add(msg);
            _context.SaveChanges();
        }

        public bool Exist(ulong id) => _messages.FirstOrDefault(m => m.MessageID == id) != null;

        public List<Message> GetAllMessages() => _messages.Include(m=>m.Channel).ThenInclude(c=>c.Guild).ToList();

        public Message GetMessage(ulong id) {
            if (!Exist(id))
                throw new Exception("this message doesn't exist");

            return _messages.Include(c=>c.Channel).ThenInclude(g=>g.Guild).FirstOrDefault(m => m.MessageID == id);
        }

        public void RemoveMessage(ulong id) {
            if (!Exist(id))
                throw new Exception("this message doesn't exist");

            Message msg = GetMessage(id);
            _messages.Remove(msg);
            _context.SaveChanges();
        }
    }
}
