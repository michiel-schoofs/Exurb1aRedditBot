using Reddit.Controllers;
using System;

namespace RedditBot.Models.Domain {
    public class GeneralPost {
        public string Author { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string AuthorFlair { get; set; }
        public string PostFlair { get; set; }

        public GeneralPost(Post posts) {
            var post = posts.Listing;
            Author = posts.Author;
            Title = posts.Title ?? "";
            Created = posts.Created;
            AuthorFlair = post.AuthorFlairText ?? "";
            PostFlair = post.LinkFlairText ?? "";
            URL = $"http://reddit.com{posts.Permalink}";
        }
    }
}
