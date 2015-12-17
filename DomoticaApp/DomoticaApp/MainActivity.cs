using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DomoticaApp
{
    [Activity(Label = "DomoticaApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Switch Adatper1 = FindViewById<Switch>(Resource.Id.Ch1);
            Switch Adatper2 = FindViewById<Switch>(Resource.Id.Ch2);
            Switch Adatper3 = FindViewById<Switch>(Resource.Id.Ch3);
            Switch Adatper4 = FindViewById<Switch>(Resource.Id.Ch4);
            Switch Adatper5 = FindViewById<Switch>(Resource.Id.ChAll);
        }
    }
}

