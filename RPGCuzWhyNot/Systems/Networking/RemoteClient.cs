using System.Net.Sockets;

namespace RPGCuzWhyNot.Systems.Networking {
	public class RemoteClient : Client {
		public RemoteClient(TcpClient tcpClient) : base(tcpClient) {
		}

		public override void Connect() {
			base.Connect();

			stream = tcpClient.GetStream();

			receivePacket.BeginReceive(stream, ReceiveCallback);
		}
	}
}
