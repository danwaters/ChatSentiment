using System;
using System.Collections.Generic;
using System.Linq;

namespace Sentiment
{
    public class ChatViewModel : BaseViewModel
    {
        private List<ChatMessageViewModel> messages;
        public List<ChatMessageViewModel> Messages
        {
            get { return messages; }
            set { SetPropertyChanged(ref messages, value); }
        }

        public string AverageSentiment
        {
            get { return GetAverageSentiment().ToString(); }
        }

        private double GetAverageSentiment()
        {
            return messages.Average(m => m.Sentiment);
        }

        public ChatViewModel()
        {
            Messages = new List<ChatMessageViewModel>
            {
                new ChatMessageViewModel
                {
                    SenderName = "Dan Waters",
                    MessageText = "The quality of mercy is not strained.",
                    Sentiment = 0.5f
                },

                new ChatMessageViewModel
                {
                    SenderName = "Cersei Lannister",
                    MessageText = "This is repugnant to the entire realm.",
                    Sentiment = 0.1f
                },

                new ChatMessageViewModel
                {
                    SenderName = "Ned Stark",
                    MessageText = "I tend to agree.",
                    Sentiment = 0.9f
                }
            };
        }
    }
}
