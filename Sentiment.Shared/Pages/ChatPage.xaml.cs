using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Sentiment
{
    public partial class ChatPage : BaseContentPage<ChatViewModel>
    {
        public ChatPage()
        {
            InitializeComponent();
        }

        public ChatPage(string username)
        {
            ViewModel.Username = username;
            InitializeComponent();
        }
    }
}
