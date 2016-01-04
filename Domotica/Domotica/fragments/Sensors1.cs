
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Timers;

namespace Domotica
{
	public class Sensors1 : Android.Support.V4.App.Fragment
	{
		//TextViews
		TextView Sensor1;
		TextView Sensor2;

		//Interactive elements
		Button refreshButton;
		Switch refreshToggleSwitch;

		//Timer
		Timer mTimer;

		ConnectionProtocol connect = new ConnectionProtocol();

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			mTimer = new Timer ();
			mTimer.Interval = 1000;
			mTimer.Elapsed += new ElapsedEventHandler (getValues);
			//mTimer.Enabled = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View view = inflater.Inflate (Resource.Layout.Sensors1, container, false);



			//Instantiate Textviews
			Sensor1 = view.FindViewById<TextView> (Resource.Id.sensor1Text);
			Sensor2 = view.FindViewById<TextView> (Resource.Id.sensor2Text);
			refreshButton = view.FindViewById<Button> (Resource.Id.Refresh_Sensors);
			refreshToggleSwitch = view.FindViewById<Switch> (Resource.Id.Toggle_SensorRefresh);

			refreshButton.Click += delegate {
				getValues();
			};

			refreshToggleSwitch.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e) {
				if(GlobalVariables.IpAvailable)
					mTimer.Enabled = e.IsChecked;
				else
				{
					refreshToggleSwitch.Checked = false;
					noConnectionAlert();
				}
			};

			return view;
		}

		public void getValues()
		{
			if (GlobalVariables.IpAvailable)
			{
				Log.Debug ("myApp", "getValues");
				string[] tempString = connect.ask ("getVal").Split (',');
				Log.Debug ("myApp", tempString [0] + ", " + tempString [1]);
				if (tempString.Length == 2)
				{
					Activity.RunOnUiThread (() => {
						Sensor1.Text = tempString [0];
						Sensor2.Text = tempString [1];
					});
				}
			} else
			{
				noConnectionAlert ();
			}
		}

		public void getValues(object sender, ElapsedEventArgs e)
		{
			if (GlobalVariables.IpAvailable)
			{
				Log.Debug ("myApp", "getValues");
				string[] tempString = connect.ask ("getVal").Split (',');
				Log.Debug ("myApp", tempString [0] + ", " + tempString [1]);
				if (tempString.Length == 2)
				{
					Activity.RunOnUiThread (() => {
						Sensor1.Text = tempString [0];
						Sensor2.Text = tempString [1];
					});
				}
			} else
			{
				noConnectionAlert ();
			}
		}
		public void noConnectionAlert()
		{
			AlertDialog.Builder alert = new AlertDialog.Builder (this.Activity);
			alert.SetTitle ("No Connection");
			alert.SetMessage ("The app could not connect to the arduino.\nPlease check if a valid IP is entered");
			alert.SetNeutralButton ("OK", (senderAlert, EventArgs) => {
				alert.Dispose ();
			});
			Activity.RunOnUiThread (() => {
				alert.Show ();
			});
		}
	}
}

