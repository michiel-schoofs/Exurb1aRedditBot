using System;

namespace RedditBot.Exceptions {
    public class PrefixException : Exception {
        public PrefixException():base() {}
        public PrefixException(string msg):base(msg) { }
    }
}
