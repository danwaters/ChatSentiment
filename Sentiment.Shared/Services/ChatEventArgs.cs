using System;

namespace Sentiment.Shared.Services
{
    public class ChatEventArgs : EventArgs
    {
        public string Username { get; private set; }
        public string Message { get; private set; }
        public double Sentiment { get; private set; }

        public ChatEventArgs(string username, string message, double sentiment)
        {
            Username = username;
            Message = message;
            Sentiment = sentiment;
        }
    }

}
