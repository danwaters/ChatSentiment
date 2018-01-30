using System;
namespace Sentiment
{
    public class ChatMessageViewModel : BaseViewModel
    {
        private string messageText;
        public string MessageText
        {
            get { return messageText; }
            set { SetPropertyChanged(ref messageText, value); }
        }

        private string senderName;
        public string SenderName
        {
            get { return senderName; }
            set { SetPropertyChanged(ref senderName, value); }
        }

        private double sentiment;
        public double Sentiment
        {
            get { return sentiment; }
            set { SetPropertyChanged(ref sentiment, value );}
        }

        public ChatMessageViewModel()
        {
        }


    }
}
