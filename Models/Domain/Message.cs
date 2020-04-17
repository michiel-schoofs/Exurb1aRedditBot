namespace RedditBot.Models.Domain {
    public class Message {
        public ulong MessageID { get; set; }
        public Channel Channel { get; set; }
        public ulong ChannelID { get; set; }
    }
}
