
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
using System.Net.Sockets;
using System.Net;
using Java.Net;
using SystemSocket = System.Net.Sockets.Socket;
using System.Net.NetworkInformation;

namespace Domotica
{
	public class Connection1 : Android.Support.V4.App.Fragment
	{
		//Connection Variables
		private Button mConnectionButton;
		private EditText mIpField;
		private EditText mPortField;
		private TextView mConnection_Text;
		private ConnectionProtocol connect = new ConnectionProtocol();

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View view = inflater.Inflate (Resource.Layout.Connection, container, false);
			//Connection Items
			mConnectionButton = view.FindViewById<Button>(Resource.Id.ConnectionButton);
			mIpField = view.FindViewById<EditText>(Resource.Id.editTextIP);
			mPortField = view.FindViewById<EditText>(Resource.Id.editTextPort);
			mConnection_Text = view.FindViewById<TextView>(Resource.Id.Connection_Text);

			//Connection Event Handlers
			mConnectionButton.Click += delegate {
				int tempIntContainer;
				GlobalVariables.IPAddress = mIpField.Text;
				int.TryParse(mPortField.Text, out tempIntContainer);
				GlobalVariables.PortAddress = tempIntContainer;
				connect.TestConnection();
				mConnection_Text.Text = GlobalVariables.IpAvailable ? "Connection Succesfull" : "Connection Failed";
			};
			return view;
		}
	}
}

