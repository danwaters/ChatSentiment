﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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

        public string AverageSentiment
        {
            get { return GetAverageSentiment().ToString(); }
        }

        private double GetAverageSentiment()
        {
            return messages.Average(m => m.Sentiment);
        }

        public ICommand SendChatCommand
        {
            get { return new Command(HandleChatItemAdded); }
        }

        public ChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessageViewModel>
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

        private void HandleChatItemAdded()
        {
            Messages.Add(new ChatMessageViewModel() {MessageText = ChatMessage, Sentiment = 0.5f});
            ChatMessage = "";

        }
    }
}
