using System;
using Sentiment;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using XFGloss;

namespace Sentiment
{
    public class BaseContentPage<T> : ContentPage, IHudProvider where T : BaseViewModel, new()
    {
        #region Properties & Constructors

        AbsoluteLayout _rootLayout; //Root container for all controls on page, including HUD
        Grid _hudRoot; //Root container for all HUD related controls
        Label _hudLabel; //Text displayed to user when HUD is showing
        ContentView _hudView; //Holds a view, such as an animation or checkmark image

        Grid _toastRoot; //Root container for all Toast related controls
        Label _toastLabel; //Root container for all Toast related controls
        public Orientation Orientation { get; private set; }
        public bool IsDesignMode { get { return App.Instance.IsDesignMode; } }

        public string HudMessage
        {
            get { return _hudLabel?.Text; }
            set { if (_hudLabel == null) return; _hudLabel.Text = value; }
        }

        T _viewModel;
        public T ViewModel
        {
            get { return _viewModel; }
            protected set { _viewModel = value; }
        }

        View _contentView;
        public View RootContent
        {
            get { return _contentView; }
            set { _contentView = value; if (value != null) SetContent(); }
        }

        bool _debugLayout;
        public bool DebugLayout
        {
            get { return _debugLayout; }
            set
            {
                if (_debugLayout != value)
                {
                    _debugLayout = value;
                    this.DebugLayout();
                }
            }
        }

        #endregion

        #region Constructors

        public BaseContentPage()
        {
            _viewModel = new T();
            Initialize();
        }

        public BaseContentPage(T viewModel)
        {
            _viewModel = viewModel;
            Initialize();
        }

        ~BaseContentPage()
        {
            //Log.Instance.WriteLine($"Disposing {GetType().Name}");
        }

        #endregion

        #region Lifecycle

        void Initialize()
        {
            BindingContext = _viewModel;
            NavigationPage.SetHasNavigationBar(this, false);

            var bg = new Gradient()
            {
                Rotation = 150,
                Steps = new GradientStepCollection()
                {
                    new GradientStep((Color)Application.Current.Resources["topGradient"], 0),
                    new GradientStep((Color)Application.Current.Resources["bottomGradient"], 1)
                }
            };

            NavigationPage.SetHasNavigationBar(this, false);
            ContentPageGloss.SetBackgroundGradient(this, bg);


            if (Hud.Instance == null)
                Hud.Instance = this;
        }

        protected override void OnAppearing()
        {
            Hud.Instance = this;
            App.Instance.AppResumed += OnAppResumed;
            App.Instance.AppBackgrounded += OnAppBackgrounded;
            App.Instance.AppNotificationReceived += OnAppNotificationReceived;
            ViewModel.NotifyPropertiesChanged();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            App.Instance.AppResumed -= OnAppResumed;
            App.Instance.AppBackgrounded -= OnAppBackgrounded;
            App.Instance.AppNotificationReceived -= OnAppNotificationReceived;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Dismiss();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            base.OnDisappearing();
        }

        public virtual void OnBeforePoppedTo()
        {

        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (DebugLayout)
                this.DebugLayout();
        }

        void OnAppBackgrounded(object sender, EventArgs e)
        {
            OnSleep();
        }

        void OnAppResumed(object sender, EventArgs e)
        {
            OnResume();
        }

        void OnAppNotificationReceived(object sender, NotificationEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.Title) || !string.IsNullOrWhiteSpace(args.Message))
            {
                var msg = args.Message.Trim();

                //if(!string.IsNullOrWhiteSpace(args.Message))
                //msg = $"{{args.Message}";

                Hud.Instance.ShowToast(msg);
            }

            OnNotificationReceived(args);
        }

        protected virtual void OnSleep()
        {
            ViewModel?.OnSleep();
        }

        protected virtual void OnResume()
        {
            ViewModel?.OnResume();
        }

        protected virtual void OnNotificationReceived(NotificationEventArgs args)
        {
            ViewModel?.OnNotificationReceived(args);
        }

        #endregion

        #region IHudProvider

        void SetContent()
        {
            if (_contentView == null)
                return;

            _rootLayout = new AbsoluteLayout();
            _hudRoot = new Grid();

            var bg = new ContentView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
            };

            var stack = new StackLayout
            {
                Padding = 30,
                Spacing = 30,
                VerticalOptions = LayoutOptions.Center,
            };

            _hudLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            _hudView = new ContentView
            {
                HorizontalOptions = LayoutOptions.Center,
            };

            stack.Children.Add(_hudView);
            stack.Children.Add(_hudLabel);
            bg.Content = stack;

            _hudRoot.Children.Add(bg);
            _hudRoot.BackgroundColor = (Color)Application.Current.Resources["hudBackgroundColor"];
            _hudRoot.IsVisible = false;

            _toastRoot = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0),
                BackgroundColor = (Color)Application.Current.Resources["toastBackgroundColor"],
                IsVisible = false,
                HeightRequest = 50,
            };

            var separatorBottom = new ContentView { Style = (Style)Application.Current.Resources["separator"] };
            var separatorTop = new ContentView { Style = (Style)Application.Current.Resources["separator"] };
            separatorTop.VerticalOptions = LayoutOptions.Start;

            _toastLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(20, 10 + _toastTopMargin, 20, 10),
            };

            _toastRoot.Children.Add(separatorTop);
            _toastRoot.Children.Add(separatorBottom);
            _toastRoot.Children.Add(_toastLabel);

            if (IsDesignMode)
            {
                var label = new Label
                {
                    Text = $"DESIGN MODE",
                    FontSize = 10,
                    LineBreakMode = LineBreakMode.WordWrap,
                    Margin = new Thickness(20),
                };

                AbsoluteLayout.SetLayoutFlags(label, AbsoluteLayoutFlags.PositionProportional);
                AbsoluteLayout.SetLayoutBounds(label, new Rectangle(.5, 0, -1, -1));
                _rootLayout.Children.Add(label);
            }

            AbsoluteLayout.SetLayoutFlags(_contentView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_contentView, new Rectangle(0, 0, 1, 1));

            AbsoluteLayout.SetLayoutFlags(_hudRoot, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_hudRoot, new Rectangle(0, 0, 1, 1));

            AbsoluteLayout.SetLayoutFlags(_toastRoot, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(_toastRoot, new Rectangle(0, 0, 1, 1));
            _toastRoot.TranslationY = NavigationBar.YOffset * -1;

            _rootLayout.Children.Add(_contentView);
            _rootLayout.Children.Add(_toastRoot);
            _rootLayout.Children.Add(_hudRoot);

            Content = _rootLayout;
        }

        public void Show(string message, View view = null)
        {
            if (_hudRoot == null)
                return;

            _hudView.IsVisible = view != null;
            _hudLabel.Text = message;
            _hudLabel.IsVisible = !string.IsNullOrWhiteSpace(message);

            if (view != null)
                _hudView.Content = view;

            _hudRoot.IsVisible = true;
        }

        public void ShowProgress(string message = null)
        {
            if (_hudRoot == null)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                _hudLabel.Text = message;
                _hudRoot.IsVisible = true;
                _hudLabel.IsVisible = !string.IsNullOrWhiteSpace(message);
                _hudView.IsVisible = true;
            });
        }

        double _toastTopMargin = Device.RuntimePlatform == Device.iOS ? 20 : 0;
        double _toastHeight = NavigationBar.YOffset;
        async public void ShowToast(string message, NoticationType type, int timeout = 3500)
        {
            if (_toastRoot == null || _toastRoot.IsVisible)
                return;

            if (_hudRoot != null && _hudRoot.IsVisible)
                await Dismiss();

            Device.BeginInvokeOnMainThread(() =>
            {
                _toastLabel.Text = message;
                _toastRoot.IsVisible = true;
            });

            await _toastRoot.TranslateTo(0, 0, 250);
            await Task.Delay(timeout).ConfigureAwait(false);
            await _toastRoot.TranslateTo(0, NavigationBar.YOffset * -1, 250);

            Device.BeginInvokeOnMainThread(() =>
            {
                _toastRoot.IsVisible = false;
            });
        }

        async public Task Dismiss(bool animate = false)
        {
            if (_hudRoot == null)
                return;

            if (animate)
            {
                await Task.Delay(300);
                await _hudRoot.FadeTo(0, 300);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                _hudRoot.IsVisible = false;
            });
        }

        #endregion

        #region Orientation Change

        double _width;
        double _height; //Orientation detection by NMkay
        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = _width;
            const double sizenotallocated = -1;

            base.OnSizeAllocated(width, height);
            if (Equals(_width, width) && Equals(_height, height))
                return;

            _width = width;
            _height = height;

            if (Equals(oldWidth, sizenotallocated))
                return;

            if (!Equals(width, oldWidth))
            {
                OnOrientationChanged((width < height) ? Orientation.Portrait : Orientation.Landscape);
            }
        }

        protected virtual void OnOrientationChanged(Orientation orientation)
        {
            Orientation = orientation;
            //Log.Instance.WriteLine($"Page orientation changed to {orientation}");
        }

        #endregion
    }

    public enum Orientation
    {
        Portrait,
        Landscape,
        All,
    }
}

