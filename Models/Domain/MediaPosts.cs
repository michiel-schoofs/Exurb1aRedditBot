using Reddit.Controllers;

namespace RedditBot.Models.Domain {
    public class MediaPosts : GeneralPost {
        public MediaPostType Type { get; set; }
        public string MediaUrl { get; set; }
        public string Content { get; set; }

        public MediaPosts(Post post): base(post) {
            var postTrue = post.Listing;

            if (postTrue.PostHint.Contains("video")) {
                Type = MediaPostType.video;
                MediaUrl = postTrue.Thumbnail;
            } else {
                Type = MediaPostType.image;
                MediaUrl = postTrue.URL;
            }
        }
    }
}
