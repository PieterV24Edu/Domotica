using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        const int port = 32545;
        private bool backgroundChange = false;
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
            EditText IpField = FindViewById<EditText>(Resource.Id.editTextIP);

            List<Switch> Adapters = new List<Switch>() { Adatper1, Adatper2, Adatper3, Adatper4, Adatper5 };

            //updateSwitches(IpField.Text, Adapters);

            Adatper1.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (!backgroundChange)
                {
                    tell(IpField.Text, port, (e.IsChecked ? "Ch1ON" : "Ch1OFF"));
                    //updateSwitches(IpField.Text, Adapters);
                }
            };
            Adatper2.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (!backgroundChange)
                {
                    tell(IpField.Text, port, (e.IsChecked ? "Ch2ON" : "Ch2OFF"));
                    //updateSwitches(IpField.Text, Adapters);
                }
            };
            Adatper3.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (!backgroundChange)
                {
                    tell(IpField.Text, port, (e.IsChecked ? "Ch3ON" : "Ch3OFF"));
                    //updateSwitches(IpField.Text, Adapters);
                }
            };
            Adatper4.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (!backgroundChange)
                {
                    tell(IpField.Text, port, (e.IsChecked ? "Ch4ON" : "Ch4OFF"));
                    //updateSwitches(IpField.Text, Adapters);
                }
            };
            Adatper5.CheckedChange += delegate (object sender, CompoundButton.CheckedChangeEventArgs e)
            {
                if (!backgroundChange)
                {
                    tell(IpField.Text, port, (e.IsChecked ? "ChAllON" : "ChAllOFF"));
                    //updateSwitches(IpField.Text, Adapters);
                }
            };
        }

        public Socket open(string ipaddress, int portnr)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(ipaddress);
            IPEndPoint endpoint = new IPEndPoint(ip, portnr);
            socket.Connect(endpoint);
            return socket;
        }

        public void write(Socket socket, string text)
        {
            socket.Send(Encoding.ASCII.GetBytes(text));
        }

        public string read(Socket socket)
        {
            byte[] bytes = new byte[4096];
            int bytesRec = socket.Receive(bytes);
            string text = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            return text;
        }

        public void close(Socket socket)
        {
            socket.Close();
        }

        // datagram like conversation with server
        public void tell(string ipaddress, int portnr, string message)
        {
            Socket s = open(ipaddress, portnr);
            write(s, message);
            close(s);
        }

        public string ask(string ipaddress, int portnr, string message)
        {
            Socket s = open(ipaddress, portnr);
            write(s, message);
            string awnser = read(s);
            close(s);
            return awnser;
        }

        public void updateSwitches(string ipaddress, List<Switch> switches )
        {
            backgroundChange = true;
            int count = 0;
            int trueCount = 0;
            string[] states = ask(ipaddress, port, "States").Split(',');
            for(int i = 0; i < 4; i++)
            {
                if (states[count] == "1")
                {
                    switches[i].Checked = true;
                    trueCount++;
                }
                else if (states[count] == "0") switches[i].Checked = false;
            }
            if (trueCount == 4 || trueCount == 0) switches[5].Checked = trueCount == 4 ? true : false;
            backgroundChange = false;
        }
    }
}

