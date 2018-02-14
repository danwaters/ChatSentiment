using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Sentiment.Shared.Services
{
    public class SignalRClient
    {
        private HubConnection hub;

        public event EventHandler<ChatEventArgs> ChatMessageReceived;

        public SignalRClient()
        {
            SignalRInit();
        }

        private async Task SignalRInit()
        {
            hub = new HubConnectionBuilder().WithUrl(Constants.HUB_URL).Build();

            hub.On<string, string, double>("NewAnalyzedMessage", 
                 (username, text, sentiment) => 
                 ChatMessageReceived?.Invoke(this, new ChatEventArgs(username, text, sentiment)));

            await hub.StartAsync();
        }

        public async Task SendChatMessage(string username, string message, double sentiment)
        {
            await hub.InvokeAsync("NewAnalyzedMessage", new object[] { username, message, sentiment });
        }
    }
}
