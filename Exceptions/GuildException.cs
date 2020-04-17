using System;

namespace RedditBot.Exceptions {
    public class GuildException:Exception {
        public GuildException():base("something went wrong involving a guild command"){}
        public GuildException(string msg) : base(msg) { }
    }
}
