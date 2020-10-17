namespace RPGCuzWhyNot.Systems.Networking {
	public static class NetworkManager {
		public const int receiveBufferSize = 1024;
		public const int sendBufferSize = 1024;

		public static string Ip { get; private set; }
		public static int Port { get; private set; }

		public static LocalClient localClient;
		public static Server server;

		public static void ConnectToServer(string ip, int port) {
			Ip = ip;
			Port = port;
		}
	}
}
