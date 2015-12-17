using System;
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

            Adatper1.CheckedChange += delegate (object sender, EventHandler e) { ask(IpField.Text, 32545, Adatper1.IsCheck)}
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
        public string ask(string ipaddress, int portnr, string message)
        {
            Socket s = open(ipaddress, portnr);
            write(s, message);
            string reply = read(s);
            close(s);
            return reply;
        }
    }
}

