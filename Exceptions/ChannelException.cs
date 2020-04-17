using System;

namespace RedditBot.Exceptions {
    public class ChannelException: Exception{
        public ChannelException():base("something went wrong with a channel command"){}
        public ChannelException(string msg):base(msg) {}
        public ChannelException(string msg, Exception inner):base(msg,inner) {}
    }
}
