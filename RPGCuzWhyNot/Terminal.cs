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

		public static Vec2 CursorPosition {
			get => new Vec2(Console.CursorLeft, Console.CursorTop);
			set {
				Console.CursorLeft = value.x;
				Console.CursorTop = value.y;
			}
		}

		public static int MillisPerChar { get; set; } = 10;
		public static int BeepFrequency { get; set; } = 200;
		public static int CharsPerBeep { get; set; } = 15;
		public static int BeepDuration { get; set; } = 10;

		public static Vec2 WindowSize {
			get => new Vec2(Console.WindowWidth, Console.WindowHeight);
			set {
				Console.WindowWidth = value.x;
				Console.WindowHeight = value.y;
			}
		}

		// If MillisPerChar is greater than BeepDuration then the time not spend beeping will be used for sleeping.
		// If MillisPerChar is less than BeepDuration then there will be noticable stutters when beeping.

		private static int charBeepCounter = 0;

		public static void PushState(Save save) => stateStack.Push(GetState(save));
		public static void PopState() => SetState(stateStack.Pop());

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
				if (!Console.KeyAvailable) {
					if (BeepDuration > 0) {
						Console.Beep(BeepFrequency, BeepDuration);
					}
					int rem = MillisPerChar - BeepDuration;
					if (rem > 0) {
						Thread.Sleep(rem);
					}
				}
			} else if (!Console.KeyAvailable) {
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
			string cmds = text[offset..braceEnd];
			int cmdStart = 0;
			while (cmdStart < cmds.Length) {
				int cmdEnd = cmds.IndexOf(';', cmdStart);
				if (cmdEnd < 0)
					cmdEnd = cmds.Length;
				int argSep = cmds.IndexOf(':', cmdStart, cmdEnd - cmdStart);
				if (argSep < 0) {
					string cmd = cmds[cmdStart..cmdEnd];
					HandleCommandWithoutArg(cmd);
					cmdStart = cmdEnd + 1;
				} else {
					string cmd = cmds[cmdStart..argSep];
					string arg = cmds[(argSep + 1)..cmdEnd];
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
				string message = text[(parenStart + 1)..parenEnd];
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
					left = arg[..sep];
					right = arg[(sep + 1)..];
					CursorPosition = new Position(int.Parse(left), int.Parse(right));
					break;
				case "push":
					Save save = Save.Nothing;
					foreach (char c in arg) {
						save |= c switch
						{
							'f' => Save.ForegroundColor,
							'b' => Save.BackgroundColor,
							'p' => Save.CursorPosition,
							'm' => Save.MillisPerChar,
							'h' => Save.BeepFrequency,
							'c' => Save.CharsPerBeep,
							'd' => Save.BeepDuration,
							_ => throw new ArgumentException(),
						};
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

		private static bool Test(Save save, Save flag) => (save & flag) != 0;

		[Flags]
		public enum Save {
			Nothing = 0,
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
			public Vec2 cursorPosition;
			public int charsPerBeep;
			public int millisPerChar;
			public int beepFrequency;
			public int beepDuration;
		}

		public delegate void AliasEffect();

		private struct Alias {
			public char symbol;
			public bool showChar;
			public AliasEffect preEffect;
			public AliasEffect postEffect;
		}

		public static void Beep(int frequency, int duration) {
			Console.Beep(frequency, duration);
		}

		public static void Clear() {
			Console.Clear();
		}

		public static string ReadLine() {
			return Console.ReadLine();
		}
	}
}
