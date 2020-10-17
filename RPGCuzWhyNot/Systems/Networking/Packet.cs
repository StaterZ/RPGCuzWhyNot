using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace RPGCuzWhyNot.Systems.Networking {
	public class Packet {
		private readonly byte[] data;
		private int offset;

		public Packet(int size) {
			data = new byte[size];
		}

		public void Send(NetworkStream stream) {
			lock (stream) {
				stream.WriteAsync(data, 0, offset);
			}

			offset = 0;
		}

		public void Append(string value) {
			Append(value.Length);
			foreach (char c in value.ToArray()) {
				Append(c);
			}
		}

		public void Append(char value) {
			Append(BitConverter.GetBytes(value));
		}

		public void Append(int value) {
			Append(BitConverter.GetBytes(value));
		}

		public void Append(float value) {
			Append(BitConverter.GetBytes(value));
		}

		public void Append(double value) {
			Append(BitConverter.GetBytes(value));
		}

		public void Append(bool value) {
			Append(BitConverter.GetBytes(value));
		}

		public void Append(object value) {
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream()) {
				bf.Serialize(ms, value);
				byte[] bytes = ms.ToArray();
				Append(bytes.Length);
				Append(bytes);
			}
		}

		public void Append(byte[] rawData) {
			int size = rawData.Length * sizeof(byte);
			Array.Copy(rawData, 0, data, offset, size);
			offset += size;
		}

		public void BeginReceive(NetworkStream stream, Action<Packet> callback) {
			stream.BeginRead(data, 0, data.Length, result => ReceiveCallback(result, stream, callback), null);
		}

		private void ReceiveCallback(IAsyncResult result, NetworkStream stream, Action<Packet> callback) {
			try {
				int size = stream.EndRead(result);
				if (size > 0) {
					offset = 0;
					callback(this);
				}

				BeginReceive(stream, callback);
			} catch (Exception e) {
				Terminal.WriteLineWithoutDelay($"Error receiving TCP Data: {e}");
			}
		}

		public string ExtractString() {
			StringBuilder builder = new StringBuilder();

			int length = ExtractInt32();
			for (int i = 0; i < length; i++) {
				builder.Append(ExtractChar());
			}

			return builder.ToString();
		}

		public char ExtractChar() {
			char value = BitConverter.ToChar(data, offset);
			offset += sizeof(char);
			return value;
		}

		public int ExtractInt32() {
			int value = BitConverter.ToInt32(data, offset);
			offset += sizeof(int);
			return value;
		}

		public float ExtractFloat() {
			float value = BitConverter.ToSingle(data, offset);
			offset += sizeof(float);
			return value;
		}

		public double ExtractDouble() {
			double value = BitConverter.ToDouble(data, offset);
			offset += sizeof(double);
			return value;
		}

		public bool ExtractBoolean() {
			bool value = BitConverter.ToBoolean(data, offset);
			offset += sizeof(bool);
			return value;
		}

		public object ExtractObject() {
			int length = ExtractInt32();

			using (MemoryStream stream = new MemoryStream()) {
				BinaryFormatter formatter = new BinaryFormatter();

				stream.Write(data, offset, length);
				stream.Seek(0, SeekOrigin.Begin);

				return formatter.Deserialize(stream);
			}
		}
	}
}