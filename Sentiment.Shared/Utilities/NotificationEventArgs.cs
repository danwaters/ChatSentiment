using System;
using System.Collections.Generic;
namespace Sentiment
{
    public class NotificationEventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Payload { get; set; }
    }
}
