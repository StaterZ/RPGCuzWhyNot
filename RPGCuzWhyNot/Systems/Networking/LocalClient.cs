using System;
using System.Net.Sockets;

namespace RPGCuzWhyNot.Systems.Networking {
	public class LocalClient : Client {
		public event Action OnConnectedAndReady;

		public LocalClient() : base(new TcpClient()) {
		}

		public override void Connect() {
			base.Connect();

			tcpClient.BeginConnect(NetworkManager.Ip, NetworkManager.Port, ConnectCallback, null);
		}

		private void ConnectCallback(IAsyncResult result) {
			Terminal.WriteLineWithoutDelay("LocalClient: ConnectCallback");
			tcpClient.EndConnect(result);

			if (!tcpClient.Connected) return;

			stream = tcpClient.GetStream();
			receivePacket.BeginReceive(stream, ReceiveCallback);

			OnConnectedAndReady?.Invoke();
		}
	}
}
