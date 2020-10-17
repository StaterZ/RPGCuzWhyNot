using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using RPGCuzWhyNot.Utilities;

namespace RPGCuzWhyNot.Systems.Networking {
	public abstract class Client {
		public Dictionary<int, TestPlayer> clients = new Dictionary<int, TestPlayer>();
		public int Id { get; set; }
		protected readonly TcpClient tcpClient;
		protected NetworkStream stream;
		protected Packet receivePacket;
		public readonly List<IClientObserver> observers = new List<IClientObserver>();

		protected Client(TcpClient tcpClient) {
			this.tcpClient = tcpClient;
		}

		public virtual void Connect() {
			tcpClient.ReceiveBufferSize = NetworkManager.receiveBufferSize;
			tcpClient.SendBufferSize = NetworkManager.sendBufferSize;

			receivePacket = new Packet(NetworkManager.receiveBufferSize);
		}

		public virtual void Disconnect() {
			tcpClient.Close();
		}

		protected void ReceiveCallback(Packet packet) {
			MessageType type = (MessageType)packet.ExtractInt32();
			switch (type) {
				case MessageType.Welcome:
					ReceiveWelcomeMessage(packet);
					break;
				case MessageType.Connect:
					ReceiveClientConnect(packet);
					break;
				case MessageType.Disconnect:
					break;
				case MessageType.RPCCall:
					ReceiveRPCCall(packet);
					break;
				default:
					throw new InvalidOperationException();
			}
		}

		private static void ReceiveWelcomeMessage(Packet packet) {
			string text = packet.ExtractString();
			Terminal.WriteLineWithoutDelay($"Text: {text}");
		}

		//public void SendWelcomeMessage(string message) {
		//	Packet packet = new Packet(NetworkManager.sendBufferSize);
		//	packet.Append((int)MessageType.Welcome);
		//	packet.Append(message);
		//	packet.Send(stream);
		//}


		//public void SendMove(Vec2 pos) {
		//	Packet packet = new Packet(NetworkManager.sendBufferSize);
		//	packet.Append((int)MessageType.Move);
		//	packet.Append(pos.x);
		//	packet.Append(pos.y);
		//	packet.Send(stream);
		//}

		public void SendClientConnect(Client client) {
			Packet packet = new Packet(NetworkManager.sendBufferSize);
			packet.Append((int)MessageType.Connect);
			packet.Append(client.Id);
			packet.Send(stream);
		}

		public void ReceiveClientConnect(Packet packet) {
			int clientId = packet.ExtractInt32();
			clients.Add(clientId, new TestPlayer());

			Terminal.WriteLineWithoutDelay($"A client with id {clientId} has connected to the server");
		}

		public static readonly Dictionary<string, (MethodInfo info, RPCAttribute attribute)> rpcNameToData;
		public static readonly Dictionary<int, (MethodInfo info, RPCAttribute attribute)> rpcIdToData;

		static Client() {
			(MethodInfo info, RPCAttribute attribute)[] rpcs = Utils.GetAllMethodsWithAttribute<RPCAttribute>().ToArray();
			rpcNameToData = new Dictionary<string, (MethodInfo info, RPCAttribute attribute)>(rpcs.Select(rpc => new KeyValuePair<string, (MethodInfo info, RPCAttribute attribute)>(rpc.info.Name, rpc)));
			rpcIdToData = new Dictionary<int, (MethodInfo info, RPCAttribute attribute)>(rpcs.Select(rpc => new KeyValuePair<int, (MethodInfo info, RPCAttribute attribute)>(rpc.info.MetadataToken, rpc)));
		}

		public void CallRPC(string rpcName, RPCTarget target, params object[] args) {
			if (rpcNameToData.TryGetValue(rpcName, out (MethodInfo info, RPCAttribute attribute) rpc)) {
				Packet packet = new Packet(NetworkManager.sendBufferSize);

				packet.Append((int)MessageType.RPCCall);
				packet.Append(rpc.info.MetadataToken);
				packet.Append(args.Length);
				foreach (object arg in args) {
					packet.Append(arg);
				}
				packet.Send(stream);
			} else {
				throw new ArgumentException($"No RPC was found with the name \"{rpcName}\"");
			}
		}

		public void ReceiveRPCCall(Packet packet) {
			int rpcId = packet.ExtractInt32();
			(MethodInfo info, RPCAttribute attribute) rpc = rpcIdToData[rpcId];

			int numOfParameters = packet.ExtractInt32();
			object[] args = new object[numOfParameters];
			for (int i = 0; i < numOfParameters; i++) {
				args[i] = packet.ExtractObject();
			}

			IClientObserver observer = observers[];

			rpc.info.Invoke(observer, args);
		}
	}
}
