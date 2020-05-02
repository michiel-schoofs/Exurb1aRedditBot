using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using RedditBot.Models.Domain;
using System;
using System.Collections.Generic;

namespace RedditBot.Services {
    public class RedditService {
        private readonly RedditClient _client;
        private readonly Subreddit exurb1a;

        public delegate PostsUpdateEventArgs PostUpdated(PostsUpdateEventArgs eve);
        public event PostUpdated PostsUpdated;

        public RedditService(Secrets secrets) {
            _client = new RedditClient(secrets.RedditAppID, secrets.RefreshToken, secrets.RedditAppSecret, secrets.AccessToken);
            exurb1a = _client.Subreddit("Exurb1a").About();
            Configure();
        }

        private void Configure() {
            exurb1a.Posts.GetNew();
            exurb1a.Posts.NewUpdated += Posts_NewUpdated;
            exurb1a.Posts.MonitorNew(breakOnFailure: false,monitoringDelayMs: 10000);
        }

        private void Posts_NewUpdated(object sender, PostsUpdateEventArgs e) {
            try {
                PostsUpdated.Invoke(e);
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        public SubredditPosts GetAllPosts() {
            return exurb1a.Posts;
        }
    }
}
