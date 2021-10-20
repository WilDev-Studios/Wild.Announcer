using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SDG.Unturned;
using OpenMod.Core.Helpers;

[assembly: PluginMetadata("Wild.Announcer", DisplayName = "Wild.Announcer", Website = "https://discord.gg/4Ggybyy87d", Author = "WilDev Studios")]
namespace Announcer
{
    public class Announcer : OpenModUnturnedPlugin
    {
        private readonly IConfiguration m_Configuration;
        private readonly ILogger<Announcer> m_Logger;

        private bool SendAnnouncements = false;
        private int MessageCount = 0;

        public Announcer(
            IConfiguration configuration,
            ILogger<Announcer> logger,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            m_Configuration = configuration;
            m_Logger = logger;
        }

        protected override UniTask OnLoadAsync()
        {
            SendAnnouncements = true;
            if (Level.isLoaded)
            {
                AsyncHelper.Schedule("Starting Announcements", () => Announcements().AsTask());
            }
            m_Logger.LogInformation("+==========================================================+");
            m_Logger.LogInformation("| WILD.ANNOUNCER plugin has been loaded!                   |");
            m_Logger.LogInformation("| Made with <3 by WildKadeGaming @ WilDev Studios          |");
            m_Logger.LogInformation("| WilDev Discord: https://discord.com/invite/4Ggybyy87d    |");
            m_Logger.LogInformation("+==========================================================+");
            return UniTask.CompletedTask;
        }

        protected override UniTask OnUnloadAsync()
        {
            SendAnnouncements = false;
            m_Logger.LogInformation("+==========================================================+");
            m_Logger.LogInformation("| WILD.ANNOUNCER plugin has been unloaded!                 |");
            m_Logger.LogInformation("| Made with <3 by WildKadeGaming @ WilDev Studios          |");
            m_Logger.LogInformation("| WilDev Discord: https://discord.com/invite/4Ggybyy87d    |");
            m_Logger.LogInformation("+==========================================================+");
            return UniTask.CompletedTask;
        }

        private class AnnounceClass
        {
            public string URL { get; set; }
            public string Message { get; set; }
        }

        private async UniTask Announcements()
        {
            await UniTask.SwitchToMainThread();
            while (SendAnnouncements)
            {
                // Gets necessary information from configuration
                await Task.Delay(TimeSpan.FromSeconds(m_Configuration.GetSection("Interval").Get<int>()));
                var announceMessages = m_Configuration.GetSection("Announcements").Get<List<AnnounceClass>>();
                var RandomEnabled = m_Configuration.GetSection("Random-Enabled").Get<bool>();
                var PreventDuplicatesEnabled = m_Configuration.GetSection("Prevent-Duplicates").Get<bool>();

                int AnnouncementIndexCheck;

                if (RandomEnabled != true)
                { // Announcements in order
                    // Checks if the message pattern needs to be reset
                    if (MessageCount >= announceMessages.Count())
                    {
                        MessageCount = 0;
                    }

                    var selectedMessage = announceMessages[MessageCount];

                    // Sends the selected message
                    if (selectedMessage.Message != null)
                    {
                        ChatManager.serverSendMessage(selectedMessage.Message, UnityEngine.Color.green, null, null, EChatMode.GLOBAL, selectedMessage.URL, true);
                    }

                    MessageCount++;
                }
                else // Announcements at random
                {
                    // Generates random number
                    int RandomNumberNew = GenerateRandomNumberFunction(announceMessages);

                    // Checks if announcement has already been sent prior
                    if (PreventDuplicatesEnabled == true)
                    {
                        if (RandomNumberNew == AnnouncementIndexCheck)
                        {
                            RandomNumberNew = GenerateRandomNumberFunction(announceMessages);
                        }
                    }

                    var SelectedMessage = announceMessages[RandomNumberNew];

                    // Sends the selected message
                    if (SelectedMessage != null)
                    {
                        ChatManager.serverSendMessage(SelectedMessage.Message, UnityEngine.Color.green, null, null, EChatMode.GLOBAL, SelectedMessage.URL, true);
                        if (PreventDuplicatesEnabled == true)
                        {
                            AnnouncementIndexCheck = RandomNumberNew;
                        }
                    }
                }
            }
        }

        private int GenerateRandomNumberFunction(List<AnnounceClass> announceMessages)
        {
            Random RandomNumber = new Random();
            int GeneratedRandomNumber = RandomNumber.Next(0, announceMessages.Count());

            return GeneratedRandomNumber;
        }
    }
}
