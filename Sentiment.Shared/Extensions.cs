using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Sentiment
{
    public static class Extensions
    {
        static string[] _allColors;
        public static void DebugLayout(this ContentPage page)
        {
            if (_allColors == null)
            {
                _allColors = new string[] { "#ffc0cb", "#008080", "#ffe4e1", "#ff0000", "#ffd700", "#d3ffce", "#00ffff", "#40e0d0", "#ff7373", "#0000ff", "#e6e6fa", "#ffa500", "#eeeeee", "#f0f8ff", "#b0e0e6", "#cccccc", "#7fffd4", "#333333", "#faebd7", "#c0c0c0", "#003366", "#fa8072", "#20b2aa", "#ffb6c1", "#800080", "#00ff00", "#f6546a", "#c6e2ff", "#666666", "#f08080", "#ffff00", "#468499", "#fff68f", "#088da5", "#ff6666", "#ffc3a0", "#00ced1", "#66cdaa", "#800000", "#f5f5f5", "#660066", "#ff00ff", "#008000", "#c39797", "#ff7f50", "#c0d6e4", "#ffdab9", "#990000", "#cbbeb5", "#dddddd", "#0e2f44", "#daa520", "#808080", "#8b0000", "#b4eeb4", "#afeeee", "#ffff66", "#f5f5dc", "#81d8d0", "#66cccc", "#00ff7f", "#ff4040", "#999999", "#b6fcd5", "#cc0000", "#8a2be2", "#ccff00", "#3399ff", "#a0db8e", "#794044", "#3b5998", "#f7f7f7", "#0099cc", "#ff4444", "#6897bb", "#31698a", "#6dc066", "#000080", "#191970", "#191919", "#404040", "#4169e1" };
            }

            ColorView(page.Content);
        }

        static int _index;
        static void ColorView(View view)
        {
            if (view == null)
                return;

            try
            {
                _index++;

                if (_index >= _allColors.Length)
                    _index = 0;

                var color = Color.FromHex(_allColors[_index]);
                //view.BackgroundColor = color.MultiplyAlpha(.1);

                if (view.BackgroundColor == Color.Default)
                    view.BackgroundColor = Color.FromHex("#11FFFFFF");

                var layout = view as ILayoutController;
                if (layout == null)
                    return;

                foreach (var child in layout.Children)
                {
                    var childView = child as View;
                    if (childView == null)
                        continue;

                    ColorView(childView);
                }
            }
            catch { }
        }

        async public static Task PopAsyncAndNotify(this INavigation nav)
        {
            if (nav.NavigationStack.Count >= 2)
            {
                var prevPage = nav.NavigationStack[nav.NavigationStack.Count - 2];

                if (prevPage is ISentimentPage)
                {
                    var bcp = prevPage as ISentimentPage;
                    bcp.OnBeforePoppedTo();
                }
            }

            await nav.PopAsync();
        }

        public static string GetFileContents(this string fileName)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var name = assembly.ManifestModule.Name.Replace(".dll", string.Empty);
            var stream = assembly.GetManifestResourceStream($"{name}.Resources.{fileName}");

            if (stream == null)
                return null;

            string content;
            using (var reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }

        public static NavigationPage ToNav(this ContentPage page)
        {
            return new NavigationPage(page)
            {
                BarTextColor = Color.White,
            };
        }
    }
}
