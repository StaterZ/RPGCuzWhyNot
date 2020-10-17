using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace RPGCuzWhyNot.Systems.Networking {
	public class Server {
		public Dictionary<int, Client> clients = new Dictionary<int, Client>();
		public int Port { get; private set; }
		private TcpListener tcpListener;

		public event Action<Client> OnClientConnect;
		public event Action<Client> OnClientDisconnect;

		public void Start(int port) {
			Port = port;

			Terminal.WriteLineWithoutDelay("Server: Starting...");

			tcpListener = new TcpListener(IPAddress.Any, port);
			tcpListener.Start();
			tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

			Terminal.WriteLineWithoutDelay($"Server: Started on port: {port}");
		}

		private void TCPConnectCallback(IAsyncResult result) {
			TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
			tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

			Terminal.WriteLineWithoutDelay($"Server: Incoming TCP connection from: {tcpClient.Client.RemoteEndPoint}");

			RemoteClient client = new RemoteClient(tcpClient);
			Register(client);

			foreach ((int id, Client otherClient) in clients) {
				if (id == client.Id) continue;

				otherClient.SendClientConnect(client);
				client.SendClientConnect(otherClient);
			}
		}

		public void Register(Client client) {
			for (int i = 0; ; i++) {
				if (clients.ContainsKey(i)) continue;
				clients.Add(i, client);
				client.Id = i;

				client.Connect();
				OnClientConnect?.Invoke(client);
				break;
			}
		}
	}
}
