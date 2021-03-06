﻿using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Sentiment
{
    public interface IHudProvider
    {
        string HudMessage { get; set; }
        void ShowToast(string message, NoticationType type = NoticationType.None, int timeout = 2000);
        void ShowProgress(string message);
        void Show(string message, View view = null);
        Task Dismiss(bool animate = false);
    }

    public static class Hud
    {
        public static IHudProvider Instance { get; set; }
    }

    public interface ISentimentPage
    {
        void OnBeforePoppedTo();
    }

    public enum NoticationType
    {
        None,
        Info,
        Success,
        Error,
    }
}
