namespace RedditBot.Models.Domain {
    public class Prefix {
        public Guild Guild { get; set; }
        public char PrefixCommand { get; set; }
        public int ID { get; set; }
        public ulong GuildID { get; set; }
    }
}
