﻿using Hardstuck.Http;
using Newtonsoft.Json;
using PlenBotLogUploader.Teams;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlenBotLogUploader.DiscordApi
{
    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class DiscordWebhookData
    {
        /// <summary>
        /// Indicates whether the webhook is currently active
        /// </summary>
        [JsonProperty("isActive")]
        internal bool Active { get; set; } = false;

        /// <summary>
        /// Name of the webhook
        /// </summary>
        [JsonProperty("name")]
        internal string Name { get; set; }

        /// <summary>
        /// URL of the webhook
        /// </summary>
        [JsonProperty("url")]
        internal string Url { get; set; }

        /// <summary>
        /// Indicates whether the webhook is executed only if the ecounter is a success
        /// </summary>
        [JsonProperty("successFailToggle")]
        internal DiscordWebhookDataSuccessToggle SuccessFailToggle { get; set; } = DiscordWebhookDataSuccessToggle.OnSuccessAndFailure;

        /// <summary>
        /// Indicates whether players are showed in the webhook
        /// </summary>
        [JsonProperty("showPlayers")]
        internal bool ShowPlayers { get; set; } = true;

        /// <summary>
        /// A list containing boss ids which are omitted to be posted via webhook
        /// </summary>
        [JsonProperty("disabledBosses")]
        internal int[] BossesDisable { get; set; }

        [JsonProperty("allowUnknownBossIds")]
        internal bool AllowUnknownBossIds { get; set; } = false;

        [JsonProperty("teamId")]
        internal int TeamId { get; set; } = 0;

        /// <summary>
        /// A selected webhook team, with which the webhook should evaluate itself
        /// </summary>
        internal Team Team
        {
            get
            {
                if ((_team is null) && Teams.Teams.All.TryGetValue(TeamId, out var team))
                {
                    _team = team;
                }
                return _team;
            }
            set
            {
                _team = value;
                TeamId = value.Id;
            }
        }

        private Team _team;

        /// <summary>
        /// Tests whether webhook is valid
        /// </summary>
        /// <param name="httpController">HttpClientController class used for using http connection</param>
        /// <returns>True if webhook is valid, false otherwise</returns>
        internal async Task<bool> TestWebhookAsync(HttpClientController httpController)
        {
            try
            {
                var response = await httpController.DownloadFileToStringAsync(Url);
                var pingTest = JsonConvert.DeserializeObject<DiscordApiJsonWebhookResponse>(response);
                return pingTest?.Success ?? false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// True if boss is enabled for webhook broadcast, false otherwise; default: true
        /// </summary>
        /// <param name="bossId">Queried boss ID</param>
        internal bool IsBossEnabled(int bossId) => !BossesDisable.Contains(bossId);

        internal static IDictionary<int, DiscordWebhookData> FromJsonString(string jsonString)
        {
            var webhookId = 1;

            var parsedData = JsonConvert.DeserializeObject<IEnumerable<DiscordWebhookData>>(jsonString)
                             ?? throw new JsonException("Could not parse json to WebhookData");

            return parsedData.Select(x => (Key: webhookId++, DiscordWebhookData: x))
                .ToDictionary(x => x.Key, x => x.DiscordWebhookData);
        }
    }
}