using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Sentiment
{
    public partial class UsernamePage : BaseContentPage<UserViewModel>
    {
        public UsernamePage()
        {
            InitializeComponent();
        }

        private async void Handle_EnterChatClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChatPage(ViewModel.Username));
        }
    }
}
