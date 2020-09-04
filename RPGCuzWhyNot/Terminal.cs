using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RPGCuzWhyNot {
	public static class Terminal {
		private static readonly Stack<State> stateStack = new Stack<State>();
		private static readonly Dictionary<char, Alias> aliases = new Dictionary<char, Alias>();

		public static ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
		public static ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }

		public static Position CursorPosition {
			get => new Position(Console.CursorLeft, Console.CursorTop);
			set {
				Console.CursorLeft = value.x;
				Console.CursorTop = value.y;
			}
		}

		public static int MillisPerChar { get; set; } = 10;
		public static int BeepFrequency { get; set; } = 200;
		public static int CharsPerBeep { get; set; } = 15;
		public static int BeepDuration { get; set; } = 10;

		// If MillisPerChar is greater than BeepDuration then the time not spend beeping will be used for sleeping.
		// If MillisPerChar is less than BeepDuration then there will be noticable stutters when beeping.

		private static int charBeepCounter = 0;

		public static void PushState(Save save = Save.ForegroundColor) => stateStack.Push(GetState(save));
		public static void PopState() => SetState(stateStack.Pop());

		public static void PushForegroundColor(ConsoleColor fg) {
			PushState(Save.ForegroundColor);
			ForegroundColor = fg;
		}

		public static void WriteLine(string text, int frequency, int charsPerSecond = -1) => Write(text + "\n", frequency, charsPerSecond);
		public static void Write(string text, int frequency, int charsPerSecond = -1) {
			PushState(Save.BeepFrequency | Save.MillisPerChar);
			if (charsPerSecond < 0)
				MillisPerChar = 1000 / charsPerSecond;
			BeepFrequency = frequency;
			WriteLine(text);
			PopState();
		}

		public static void WriteLine() => Write("\n");
		public static void WriteLine(string text) {
			Write(text);
			Write("\n");
		}

		public static void Write(char c) => WriteChar(c);
		public static void Write(string text) {
			for (int i = 0; i < text.Length;) {
				char c = text[i++];
				if (c == '{') {
					if (i < text.Length && text[i] == '{') {
						WriteChar(c);
						++i;
					} else {
						i = Decode(text, i);
					}
				} else {
					WriteChar(c);
				}
			}
		}

		private static void WriteChar(char c) {
			bool doAlias = aliases.TryGetValue(c, out Alias alias);
			if (doAlias) {
				alias.preEffect?.Invoke();
				if (!alias.showChar) {
					return;
				}
			}
			Console.Write(c);
			++charBeepCounter;
			if (charBeepCounter >= CharsPerBeep) {
				charBeepCounter = 0;
				if (BeepDuration > 0) {
					Console.Beep(BeepFrequency, BeepDuration);
				}
				int rem = MillisPerChar - BeepDuration;
				if (rem > 0) {
					Thread.Sleep(rem);
				}
			} else {
				Thread.Sleep(MillisPerChar);
			}
			if (doAlias) {
				alias.postEffect?.Invoke();
			}
		}

		private static int Decode(string text, int offset) {
			int braceEnd = text.IndexOf('}', offset);
			if (braceEnd < 0)
				throw new ArgumentException();
			bool doPop = false;
			int parenStart = braceEnd + 1;
			if (parenStart < text.Length && text[parenStart] == '(') {
				PushState(Save.Everything & ~Save.CursorPosition);
				doPop = true;
			}
			string cmds = text.Substring(offset, braceEnd - offset);
			int cmdStart = 0;
			while (cmdStart < cmds.Length) {
				int cmdEnd = cmds.IndexOf(';', cmdStart);
				if (cmdEnd < 0)
					cmdEnd = cmds.Length;
				int argSep = cmds.IndexOf(':', cmdStart, cmdEnd - cmdStart);
				if (argSep < 0) {
					string cmd = cmds.Substring(cmdStart, cmdEnd - cmdStart);
					HandleCommandWithoutArg(cmd);
					cmdStart = cmdEnd + 1;
				} else {
					string cmd = cmds.Substring(cmdStart, argSep - cmdStart);
					string arg = cmds.Substring(argSep + 1, cmdEnd - (argSep + 1));
					HandleCommandWithArg(cmd, arg);
					cmdStart = cmdEnd + 1;
				}
			}
			if (doPop) {
				int parenEnd = parenStart + 1;
				int parenDepth = 1;
				for (; parenEnd < text.Length; ++parenEnd) {
					switch (text[parenEnd]) {
						case '(': ++parenDepth; break;
						case ')': --parenDepth; break;
					}
					if (parenDepth == 0) {
						break;
					}
				}
				if (parenEnd == text.Length)
					throw new ArgumentException();
				string message = text.Substring(parenStart + 1, parenEnd - (parenStart + 1));
				Write(message);
				PopState();
				return parenEnd + 1;
			}
			return braceEnd + 1;
		}

		private static void HandleCommandWithArg(string cmd, string arg) {
			switch (cmd) {
				case "fg": ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), arg, true); break;
				case "bg": BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), arg, true); break;
				case "ch/b": CharsPerBeep = int.Parse(arg); break;
				case "ms/ch": MillisPerChar = int.Parse(arg); break;
				case "bHz": BeepFrequency = int.Parse(arg); break;
				case "bMs": BeepDuration = int.Parse(arg); break;
				case "cursor":
					int sep = arg.IndexOf(',');
					if (sep < 0) throw new ArgumentException();
					string left, right;
					left = arg.Substring(0, sep);
					right = arg.Substring(sep + 1, arg.Length - (sep + 1));
					CursorPosition = new Position(int.Parse(left), int.Parse(right));
					break;
				case "push":
					Save save = 0;
					foreach (char c in arg) {
						switch (c) {
							case 'f': save |= Save.ForegroundColor; break;
							case 'b': save |= Save.BackgroundColor; break;
							case 'p': save |= Save.CursorPosition; break;
							case 'm': save |= Save.MillisPerChar; break;
							case 'h': save |= Save.BeepFrequency; break;
							case 'c': save |= Save.CharsPerBeep; break;
							case 'd': save |= Save.BeepDuration; break;
							default: throw new ArgumentException();
						}
					}
					PushState(save);
					break;
				default: HandleCommandWithoutArg(cmd); break;
			}
		}

		private static void HandleCommandWithoutArg(string cmd) {
			if (Enum.TryParse(cmd, true, out ConsoleColor color)) {
				ForegroundColor = color;
				return;
			}
			switch (cmd) {
				case "pop": PopState(); break;
				default: throw new ArgumentException();
			}
		}

		public static void AddAlias(char symbol, bool showChar, AliasEffect preEffect, AliasEffect postEffect) {
			aliases.Add(symbol, new Alias {
				symbol = symbol,
				showChar = showChar,
				preEffect = preEffect,
				postEffect = postEffect,
			});
		}

		public static State GetState(Save save) {
			State state = default;
			state.save = save;
			if (Test(save, Save.ForegroundColor)) state.foregroundColor = ForegroundColor;
			if (Test(save, Save.BackgroundColor)) state.backgroundColor = BackgroundColor;
			if (Test(save, Save.CursorPosition)) state.cursorPosition = CursorPosition;
			if (Test(save, Save.MillisPerChar)) state.millisPerChar = MillisPerChar;
			if (Test(save, Save.BeepFrequency)) state.beepFrequency = BeepFrequency;
			if (Test(save, Save.CharsPerBeep)) state.charsPerBeep = CharsPerBeep;
			if (Test(save, Save.BeepDuration)) state.beepDuration = BeepDuration;
			return state;
		}

		public static void SetState(State state) {
			if (Test(state.save, Save.ForegroundColor)) ForegroundColor = state.foregroundColor;
			if (Test(state.save, Save.BackgroundColor)) BackgroundColor = state.backgroundColor;
			if (Test(state.save, Save.CursorPosition)) CursorPosition = state.cursorPosition;
			if (Test(state.save, Save.MillisPerChar)) MillisPerChar = state.millisPerChar;
			if (Test(state.save, Save.BeepFrequency)) BeepFrequency = state.beepFrequency;
			if (Test(state.save, Save.CharsPerBeep)) CharsPerBeep = state.charsPerBeep;
			if (Test(state.save, Save.BeepDuration)) BeepDuration = state.beepDuration;
		}

		public static string Encode(State state) {
			StringBuilder sb = new StringBuilder();
			if (Test(state.save, Save.ForegroundColor)) sb.Append(EncodeForegroundColor(state.foregroundColor));
			if (Test(state.save, Save.BackgroundColor)) sb.Append(EncodeBackgroundColor(state.backgroundColor));
			if (Test(state.save, Save.CursorPosition)) sb.Append(EncodeCursorPosition(state.cursorPosition));
			if (Test(state.save, Save.MillisPerChar)) sb.Append(EncodeMillisPerCharacter(state.millisPerChar));
			if (Test(state.save, Save.BeepFrequency)) sb.Append(EncodeBeepFrequency(state.beepFrequency));
			if (Test(state.save, Save.CharsPerBeep)) sb.Append(EncodeCharsPerBeep(state.charsPerBeep));
			if (Test(state.save, Save.BeepDuration)) sb.Append(EncodeBeepDuration(state.beepDuration));
			return sb.ToString();
		}

		public static string EncodePushForegroundColor(ConsoleColor fg) => EncodePush(Save.ForegroundColor) + EncodeForegroundColor(fg);

		public static string EncodeForegroundColor(ConsoleColor newFg) => $"{{fg:{newFg}}}";
		public static string EncodeBackgroundColor(ConsoleColor newBg) => $"{{bg:{newBg}}}";
		public static string EncodeCursorPosition(Position newCur) => $"{{cursor:{newCur.x},{newCur.y}}}";
		public static string EncodeMillisPerCharacter(int millis) => $"{{ms/ch:{millis}}}";
		public static string EncodeBeepFrequency(int frequency) => $"{{bHz:{frequency}}}";
		public static string EncodeCharsPerBeep(int chars) => $"{{ch/b:{chars}}}";
		public static string EncodeBeepDuration(int millis) => $"{{bMs:{millis}}}";

		private const string EncodedPop = "{pop}";
		public static string EncodePop() => EncodedPop;
		public static string EncodePush(Save save) {
			StringBuilder sb = new StringBuilder();
			sb.Append("{push:");
			if (Test(save, Save.ForegroundColor)) sb.Append('f');
			if (Test(save, Save.BackgroundColor)) sb.Append('b');
			if (Test(save, Save.CursorPosition)) sb.Append('p');
			if (Test(save, Save.MillisPerChar)) sb.Append('m');
			if (Test(save, Save.BeepFrequency)) sb.Append('h');
			if (Test(save, Save.CharsPerBeep)) sb.Append('c');
			if (Test(save, Save.BeepDuration)) sb.Append('d');
			sb.Append('}');
			return sb.ToString();
		}

		public static string Escape(string text) {
			return text.Replace("{", "{{");
		}

		private static bool Test(Save save, Save flag) => (save & flag) != 0;

		[Flags]
		public enum Save {
			ForegroundColor = 1 << 0,
			BackgroundColor = 1 << 1,
			CursorPosition = 1 << 2,
			CharsPerBeep = 1 << 3,
			MillisPerChar = 1 << 4,
			BeepFrequency = 1 << 5,
			BeepDuration = 1 << 6,

			Everything = ForegroundColor | BackgroundColor | CursorPosition | CharsPerBeep | MillisPerChar | BeepFrequency | BeepDuration,
		}

		public struct State {
			public Save save;
			public ConsoleColor foregroundColor;
			public ConsoleColor backgroundColor;
			public Position cursorPosition;
			public int charsPerBeep;
			public int millisPerChar;
			public int beepFrequency;
			public int beepDuration;
		}

		public struct Position {
			public int x, y;

			public Position(int x, int y) {
				this.x = x;
				this.y = y;
			}
		}

		public delegate void AliasEffect();

		private struct Alias {
			public char symbol;
			public bool showChar;
			public AliasEffect preEffect;
			public AliasEffect postEffect;
		}
	}
}
