using System;
using System.Reflection;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems.Networking;

namespace RPGCuzWhyNot.Systems {
	public class TestPlayer : IClientObserver {
		private Vec2 pos = new Vec2(10, 10);

		public Vec2 Pos {
			get => pos;
			set => NetworkManager.localClient.CallRPC(nameof(SetPos), RPCTarget.All, value);
		}

		[RPC]
		private void SetPos(Vec2 value) {
			pos = value;
			DrawPlayerIfNeeded();
		}

		private void OnReceivePacket(Packet packet) {
			ReceiveMove(packet);
		}

		private void ReceiveMove(Packet packet) {
			int x = packet.ExtractInt32();
			int y = packet.ExtractInt32();
			Pos = new Vec2(x, y);
		}

		private Vec2 lastPos = -Vec2.One;
		private void DrawPlayerIfNeeded() {
			if (lastPos == Pos) return;

			Vec2 truePos = Terminal.CursorPosition;
			if (lastPos != -Vec2.One) {
				Terminal.CursorPosition = lastPos;
				Terminal.WriteWithoutDelay(" ");
			}

			Terminal.CursorPosition = Pos;
			Terminal.WriteWithoutDelay("@");
			Terminal.CursorPosition = truePos;

			lastPos = Pos;
		}

		public void Update() {
			if (NetworkManager.localClient == null) return;
			if (ClientId != NetworkManager.localClient.Id) return;

			ConsoleKeyInfo keyPress = Terminal.ReadKey(true);
			switch (keyPress.Key) {
				case ConsoleKey.RightArrow:
					Pos += Vec2.Right;
					break;
				case ConsoleKey.LeftArrow:
					Pos += Vec2.Left;
					break;
				case ConsoleKey.UpArrow:
					Pos += Vec2.Up;
					break;
				case ConsoleKey.DownArrow:
					Pos += Vec2.Down;
					break;
			}
		}
	}
}
