using Reddit.Controllers;
using System;

namespace RedditBot.Models.Domain {
    public class PostDisplay : GeneralPost {
        public string Content { get; set; }
        public string Image { get; set; }

        public PostDisplay(Post posts):base(posts) {
            var post = posts.Listing;

            if (post.SelfText.Length > 200)
                Content = post.SelfText.Substring(0, 97) + "...";
            else
                Content = post.SelfText ?? "";

            Image = "https://styles.redditmedia.com/t5_3d03a/styles/communityIcon_2ty3floxdam41.png";
        }
    }
}
