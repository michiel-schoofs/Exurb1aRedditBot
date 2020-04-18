using Discord;
using Discord.WebSocket;
using Reddit.Controllers;
using RedditBot.Models.Domain;
using System;
using System.Collections.Generic;

namespace RedditBot.Services {
    public class EmbedService {
        private static readonly ulong sandraId = 401452008957280257;

        public static EmbedBuilder BuildErrorEmbed(string errormsg, SocketGuild guild) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.DarkRed,
                Description = "Something went wrong",
                Title = "🤯 Oh nooooo 🤯",
                ThumbnailUrl = "https://hdwallpapers.cat/wallpaper/windows-error-women-chibi-anime-girl-female-SUYy.jpg",
                Footer = BuildFooter(guild)
            };

            emb.Fields.Add(new EmbedFieldBuilder() {
                Name = "Reason",
                Value = errormsg
            });

            return emb;
        }

        public static EmbedBuilder BuildMediaRedditPost(Post post, SocketGuild guild) {
            MediaPosts display = new MediaPosts(post);

            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Description = display.Content,
                Footer = BuildFooter(guild),
                ImageUrl = display.MediaUrl,
                Title = String.Format("{0} posted {1} on the subreddit.", display.Author, display.Type == MediaPostType.image ? "an image" : "a video"),
                Url = display.URL,
            };

            emb.Fields = BuildFields(display);

            return emb;
        }

        public static EmbedBuilder BuildCommandEmbed(SocketGuild guild) {
            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Purple,
                Description = "Please see the provided list for all available commands." +
                " Everything in the config section has to be prefaced with config." +
                "Everything in bold must be replaced by the parameter itself.\nFor example: **config prefix ?**\n",
                Title = "Commands",
                Footer = BuildFooter(guild),
                ThumbnailUrl = "https://static.tvtropes.org/pmwiki/pub/images/winry_transparent.png"
            };


            List<EmbedFieldBuilder> embedFields = new List<EmbedFieldBuilder>();
            
            string[] configCommands = new string[] { "add channel **channel type (either publicfeed or approval)**", "remove channel","prefix **prefix**" };
            string[] generalCommands = new string[] { "ping","pong","commands","help"};
            
            embedFields.Add(new EmbedFieldBuilder() { IsInline = false, Name = "Config commands (require privileges)", Value = string.Join('\n', configCommands) });
            embedFields.Add(new EmbedFieldBuilder() { IsInline = false, Name = "General commands", Value = string.Join('\n', generalCommands) });
            emb.Fields = embedFields;
            return emb;
        }

        public static EmbedBuilder BuildRedditPost(Post posts, SocketGuild guild) {
            PostDisplay display = new PostDisplay(posts);

            EmbedBuilder emb = new EmbedBuilder() {
                Color = Color.Teal,
                Description = display.Content,
                Footer = BuildFooter(guild),
                ThumbnailUrl = display.Image,
                Title = $"{display.Author} made a post on the subreddit.",
                Url = display.URL,
            };

            emb.Fields = BuildFields(display);

            return emb;
        }

        private static List<EmbedFieldBuilder> BuildFields(GeneralPost display) {
            List<EmbedFieldBuilder> embedFields = new List<EmbedFieldBuilder>();
            embedFields.Add(new EmbedFieldBuilder() { IsInline = true, Name = "Author name", Value = display.Author });

            if (display.AuthorFlair != null && display.AuthorFlair.Trim().Length != 0)
                embedFields.Add(new EmbedFieldBuilder() { IsInline = false, Name = "Author flair", Value = display.AuthorFlair });

            embedFields.Add(new EmbedFieldBuilder() { IsInline = true, Name = "Created", Value = display.Created.ToShortDateString() });

            if (display.PostFlair != null && display.PostFlair.Trim().Length != 0)
                embedFields.Add(new EmbedFieldBuilder() { IsInline = false, Name = "Post flair", Value = display.PostFlair });

            if(display.Title != null && display.Title.Trim().Length != 0) 
                embedFields.Add(new EmbedFieldBuilder() { IsInline = false, Name = "Post Title", Value = display.Title});

            return embedFields;
        }

        public static EmbedFooterBuilder BuildFooter(SocketGuild guild) {
            SocketGuildUser user = guild.GetUser(sandraId);
            EmbedFooterBuilder footer = new EmbedFooterBuilder() {
                Text = $"Made by {user.Nickname ?? user.Username}",
                IconUrl = user.GetAvatarUrl()
            };
            return footer;
        }
    }
}
