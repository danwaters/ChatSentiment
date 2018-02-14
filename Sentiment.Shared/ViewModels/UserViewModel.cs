using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace Sentiment
{
    public class UserViewModel : BaseViewModel
    {
        private string userName = "";
        public string Username 
        {
            get { return userName; }
            set { SetPropertyChanged(ref userName, value); }
        }

        public UserViewModel()
        {
            Username = "";
        }
    }
}
