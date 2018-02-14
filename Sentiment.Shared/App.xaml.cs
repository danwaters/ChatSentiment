using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Sentiment
{
    public partial class App : Application
    {
        public event EventHandler AppResumed;
        public event EventHandler AppBackgrounded;
        public event EventHandler<NotificationEventArgs> AppNotificationReceived;

        public Size ScreenSize { get; set; } = new Size(380, 540);

        static App _instance;
        public static App Instance => _instance;
        public bool IsDesignMode { get; set; }
        public string Assemblies { get; set; }

        public App()
        {
            _instance = this;
            IsDesignMode = Type.GetType("MonoTouch.Design.Parser,MonoTouch.Design") != null;
            if (IsDesignMode)
            {
                //Instance.CurrentGame = Mocker.GetGame(5, 4, true, true, true, true, true);
                //Instance.CurrentGame.StartDate = null;

                ////Has game started
                ////Instance.CurrentGame.StartDate = DateTime.Now;

                ////Has game ended
                //Instance.CurrentGame.EndDate = DateTime.Now;
                //Instance.CurrentGame.StartDate = DateTime.Now;
                //Instance.CurrentGame.WinnningTeamId = Instance.CurrentGame.Teams[1].Id;

                ////Are you a player
                //Player = Instance.CurrentGame.Teams[1].Players[0];

                ////Are you the coordinator
                //Player = Instance.CurrentGame.Coordinator.Clone(); //Jon
                //Instance.CurrentGame.Coordinator = Player;

            }

            InitializeComponent();
        }

        protected override void OnStart()
        {
            base.OnStart();
            MainPage = new UsernamePage().ToNav();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
