namespace RedditBot.Models.Domain {
    public class Channel {
        public ulong ChanelID { get; set; }
        public string Name { get; set; }
        public ChannelType Type { get; set; }
        public Guild Guild { get; set; }
        public ulong GuildID { get; set; }
    }
}
