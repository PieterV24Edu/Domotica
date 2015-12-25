
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
using System.Threading;

namespace Domotica
{
	public class Switches1 : Android.Support.V4.App.Fragment
	{
		//Switch Variables
		private Switch Adapter1;
		private Switch Adapter2;
		private Switch Adapter3;
		private Switch Adapter4;
		private Switch Adapter5;
		private List<Switch> _Adapters;
		private bool backgroundChange = false;

		private ConnectionProtocol connect = new ConnectionProtocol();

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View view = inflater.Inflate (Resource.Layout.Switches1, container, false);

			//Assing ID's
			Adapter1 = view.FindViewById<Switch>(Resource.Id.Ch1);
			Adapter2 = view.FindViewById<Switch>(Resource.Id.Ch2);
			Adapter3 = view.FindViewById<Switch>(Resource.Id.Ch3);
			Adapter4 = view.FindViewById<Switch>(Resource.Id.Ch4);
			Adapter5 = view.FindViewById<Switch>(Resource.Id.ChAll);
			_Adapters = new List<Switch>() { Adapter1, Adapter2, Adapter3, Adapter4, Adapter5 };


			//Switches Event Handler
			Adapter1.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e) 
			{
				if (!backgroundChange)
				{
					ThreadPool.QueueUserWorkItem (o => switchControl (1, e.IsChecked));
				}
			};
			Adapter2.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e) 
			{
				if (!backgroundChange)
				{
					ThreadPool.QueueUserWorkItem (o => switchControl (2, e.IsChecked));
				}
			};
			Adapter3.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) 
			{
				if (!backgroundChange)
				{
					ThreadPool.QueueUserWorkItem (o => switchControl (3, e.IsChecked));
				}
			};
			Adapter4.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) 
			{
				if (!backgroundChange)
				{
					ThreadPool.QueueUserWorkItem (o => switchControl (4, e.IsChecked));
				}
			};
			Adapter5.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e) 
			{
				if (!backgroundChange)
				{
					ThreadPool.QueueUserWorkItem (o => switchControl (5, e.IsChecked));
				}
			};
			return view;
		}

		//Switches Methods
		public void switchControl(int switchNr, bool state)
		{
			if (GlobalVariables.IpAvailable)
			{
				switch (switchNr)
				{
					case 1:
						connect.tell (state ? "Ch1ON" : "Ch1OFF");
						break;
					case 2:
						connect.tell (state ? "Ch2ON" : "Ch2OFF");
						break;
					case 3:
						connect.tell (state ? "Ch3ON" : "Ch3OFF");
						break;
					case 4:
						connect.tell (state ? "Ch4ON" : "Ch4OFF");
						break;
					case 5:
						connect.tell (state ? "ChAllON" : "ChAllOFF");
						break;
				}
				checkSwitches ();
			}
		}

		public void checkSwitches()
		{
			backgroundChange = true;
			string[] states = connect.ask("States").Split(',');
			List<bool> boolStates = new List<bool>();
			foreach (string s in states)
			{
				if (s == "true") 
					boolStates.Add(true);
				else 
					boolStates.Add(false);
			}
			if (_Adapters.Count == 5)
			{
				Activity.RunOnUiThread (() => {
					for (int i = 0; i < 4; i++)
					{
						if(_Adapters[i].Checked != boolStates[i])
						{
							_Adapters [i].Checked = boolStates [i];
						}
					}
					if (boolStates.Contains (!boolStates [0])) 
						_Adapters [4].Checked = false;
					else 
						_Adapters [4].Checked = boolStates [0];
					backgroundChange = false;
				});
			}
			//backgroundChange = false;
		}
	}
}

