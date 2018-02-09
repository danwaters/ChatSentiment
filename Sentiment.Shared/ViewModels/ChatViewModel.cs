using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Sentiment.Services;
using Xamarin.Forms;

namespace Sentiment
{
    public class ChatViewModel : BaseViewModel
    {
        private ObservableCollection<ChatMessageViewModel> messages;
        public ObservableCollection<ChatMessageViewModel> Messages
        {
            get { return messages; }
            set { SetPropertyChanged(ref messages, value); }
        }

        private string chatMessage = "";
        public string ChatMessage
        {
            get { return chatMessage; }
            set { SetPropertyChanged(ref chatMessage, value);}
        }

        public double AverageSentiment
        {
            get { if (!messages.Any()) return 0.0f; else return GetAverageSentiment(); }
        }

        public string AverageSentimentString
        {
            get { return Math.Round(AverageSentiment, 0).ToString() + "%"; }
        }

        private double GetAverageSentiment()
        {
            return messages.Average(m => m.Sentiment) * 100;
        }

        public ICommand SendChatCommand
        {
            get { return new Command(HandleChatItemAdded); }
        }

        public ChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessageViewModel>
            {

            };
        }

        private async void HandleChatItemAdded()
        {
            var analytics = new AzureTextAnalyzer();
            var result = await analytics.AnalyzeSentiment(ChatMessage);
            var sentiment = result.HasValue ? result.Value : 0.0f;
            Messages.Insert(0, new ChatMessageViewModel() {MessageText = ChatMessage, Sentiment = sentiment});
            NotifyPropertiesChanged();
            ChatMessage = "";
        }

        public override void NotifyPropertiesChanged()
        {
            base.NotifyPropertiesChanged();
            SetPropertyChanged(nameof(AverageSentiment));
            SetPropertyChanged(nameof(AverageSentimentString));
        }
    }
}
