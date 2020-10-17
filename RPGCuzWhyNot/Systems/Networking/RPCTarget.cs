namespace RPGCuzWhyNot.Systems.Networking {
	public enum RPCTarget {
		Server = 1 << 0,
		RemoteClients = 1 << 1,
		LocalClients = 1 << 2,

		AllClients = RemoteClients | LocalClients,
		All = AllClients | Server
	}
}
