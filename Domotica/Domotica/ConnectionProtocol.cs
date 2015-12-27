using System;
using SystemSocket = System.Net.Sockets.Socket;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Domotica
{
	public class ConnectionProtocol
	{
		public ConnectionProtocol ()
		{
		}
		public void TestConnection()
		{
			Ping p = new Ping ();
			PingReply reply = p.Send (GlobalVariables.IPAddress);
			GlobalVariables.IpAvailable = (reply.Status == IPStatus.Success);
		}

		//Open Socket Connection
		public SystemSocket open()
		{
			SystemSocket socket = new SystemSocket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPAddress ip = IPAddress.Parse (GlobalVariables.IPAddress);
			IPEndPoint endpoint = new IPEndPoint (ip, GlobalVariables.PortAddress);
			socket.Connect (endpoint);
			return socket;
		}

		//Send Message to Socket
		public void write(SystemSocket socket, string text)
		{
			socket.Send(Encoding.ASCII.GetBytes(text));
		}
		//Read incomming socket messages
		public string read(SystemSocket socket)
		{
			byte[] bytes = new byte[4096];
			int bytesRec = socket.Receive(bytes);
			string text = Encoding.ASCII.GetString(bytes, 0, bytesRec);
			return text;
		}
		//Close Socket messages
		public void close(SystemSocket socket)
		{
			socket.Close();
		}

		//tell arduino what to do without expecting a response
		public void tell(string message)
		{
			try {
				SystemSocket s = open ();
				write (s, message);
				close (s);
			}
			catch {
				GlobalVariables.IpAvailable = false;
			}
		}

		//tell arduino what to return
		public string ask(string message)
		{
			try {
				SystemSocket s = open ();
				write (s, message);
				string awnser = read (s);
				close (s);
				return awnser;
			}
			catch {
				GlobalVariables.IpAvailable = false;
				return "null";
			}
		}
	}
}

