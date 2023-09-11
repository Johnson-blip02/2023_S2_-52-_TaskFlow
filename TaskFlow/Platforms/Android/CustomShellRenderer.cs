using Android.Content;
using Android.Views;
using Android.Content.Res;
using Android.Graphics;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Color = Android.Graphics.Color;

namespace TaskFlow.Platforms.Android
{
    public class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context)
        {
        }
        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new CustomBottomNavView();
        }
    }

    public class CustomBottomNavView : IShellBottomNavViewAppearanceTracker
    {
        public void Dispose()
        {
        }

        public void ResetAppearance(BottomNavigationView bottomView)
        {
            // Reset any appearance changes here if needed.
        }

        public void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            // Set the label visibility mode
            bottomView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityUnlabeled;
        }
    }
}