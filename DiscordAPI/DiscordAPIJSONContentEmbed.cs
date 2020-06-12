﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlenBotLogUploader.DiscordAPI
{
    /// <summary>
    /// Discord embedded rich content
    /// </summary>
    public class DiscordAPIJSONContentEmbed
    {
        /// <summary>
        /// title of the embed
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// description of the embed
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// url of the embed
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// color code of the embed
        /// </summary>
        [JsonProperty("color")]
        public int Color { get; set; }

        /// <summary>
        /// timestamp of embed content (in ISO8601)
        /// </summary>
        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; }

        /// <summary>
        /// thumbnail information
        /// </summary>
        [JsonProperty("thumbnail")]
        public DiscordAPIJSONContentEmbedThumbnail Thumbnail { get; set; }

        /// <summary>
        /// footer information
        /// </summary>
        [JsonProperty("footer")]
        public DiscordAPIJSONContentEmbedFooter Footer { get; set; } = new DiscordAPIJSONContentEmbedFooter();

        /// <summary>
        /// fields information
        /// </summary>
        [JsonProperty("fields")]
        public List<DiscordAPIJSONContentEmbedField> Fields { get; set; }
    }
}
