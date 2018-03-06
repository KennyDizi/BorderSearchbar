using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Widget;
using BoderSearchBar;
using BoderSearchBar.Droid;
using Xamarin.Forms;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(handler: typeof(CrossSearchBar), target: typeof(PlatformSearchBarRenderer))]

namespace BoderSearchBar.Droid
{
    public class PlatformSearchBarRenderer : SearchBarRenderer
    {
        public PlatformSearchBarRenderer(Context context) : base(context: context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (this.Control == null) return;

            #region for edittext

            var editText = this.Control.GetChildrenOfType<EditText>().FirstOrDefault();
            if (editText != null)
            {
                var shape = new ShapeDrawable(new RectShape());
                shape.Paint.Color = Android.Graphics.Color.Transparent;
                shape.Paint.StrokeWidth = 0;
                shape.Paint.SetStyle(Paint.Style.Stroke);
                editText.Background = shape;
            }

            #endregion

            #region control searchbar

            var gradient = new GradientDrawable();
            gradient.SetCornerRadius(5.0f);
            int[][] states =
            {
                new[] {Android.Resource.Attribute.StateEnabled}, // enabled
                new[] {-Android.Resource.Attribute.StateEnabled} // disabled
            };

            int[] colors =
            {
                Xamarin.Forms.Color.Red.ToAndroid(),
                Xamarin.Forms.Color.Gray.ToAndroid()
            };
            var stateList = new ColorStateList(states: states, colors: colors);
            gradient.SetStroke((int) this.Context.ToPixels(1.0f), stateList);

            this.Control.SetBackground(gradient);

            #endregion
        }
    }

    internal static class ViewGroupExtensions
    {
        internal static IEnumerable<T> GetChildrenOfType<T>(this AViewGroup self) where T : AView
        {
            for (var i = 0; i < self.ChildCount; i++)
            {
                var child = self.GetChildAt(i);
                if (child is T typedChild)
                    yield return typedChild;

                if (!(child is AViewGroup)) continue;
                var myChildren = (child as AViewGroup).GetChildrenOfType<T>();
                foreach (var nextChild in myChildren)
                    yield return nextChild;
            }
        }
    }
}