using System;
using System.Collections.Generic;
using System.Threading;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems.Data;

namespace RPGCuzWhyNot.Systems {
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

		public static void PushState() => stateStack.Push(GetState());
		public static void PopState() => SetState(stateStack.Pop());

		public static void WriteLine(string text, int frequency, int charsPerSecond = -1) => Write(text + '\n', frequency, charsPerSecond);
		public static void Write(string text, int frequency, int charsPerSecond = -1) {
			PushState();
			if (charsPerSecond < 0) {
				MillisPerChar = 1000 / charsPerSecond;
			}
			BeepFrequency = frequency;
			Write(text);
			PopState();
		}

		public static void WriteLine() => Write("\n");
		public static void WriteLine(string text) {
			Write(text);
			Write('\n');
		}

		/// <summary>
		/// Write a line without delay or beeping.
		/// </summary>
		public static void WriteLineDirect(string text) {
			WriteDirect(text);
			Write('\n');
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

		/// <summary>
		/// Write without delay or beeping.
		/// </summary>
		public static void WriteDirect(string text) {
			PushState();
			MillisPerChar = 0;
			BeepDuration = 0;
			Write(text);
			PopState();
		}

		private static void WriteChar(char c) {
			bool doAlias = aliases.TryGetValue(c, out Alias alias);
			if (doAlias) {
				alias.preEffect?.Invoke();
				if (!alias.showChar) {
					goto postEffect;
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
		postEffect:
			if (doAlias) {
				alias.postEffect?.Invoke();
			}
		}

		private static int Decode(string text, int offset) {
			int braceEnd = text.IndexOf('}', offset);
			if (braceEnd == -1) throw new ArgumentException();

			bool doPop = false;
			int parenStart = braceEnd + 1;
			if (parenStart < text.Length && text[parenStart] == '(') {
				PushState();
				doPop = true;
			}
			string cmds = text[offset..braceEnd];
			int cmdStart = 0;
			while (cmdStart < cmds.Length) {
				int cmdEnd = cmds.IndexOf(';', cmdStart);
				if (cmdEnd == -1) {
					cmdEnd = cmds.Length;
				}
				int argSep = cmds.IndexOf(':', cmdStart, cmdEnd - cmdStart);
				if (argSep == -1) {
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
				if (parenEnd >= text.Length) throw new ArgumentException();
				string message = text[(parenStart + 1)..parenEnd];
				Write(message);
				PopState();
				return parenEnd + 1;
			}
			return braceEnd + 1;
		}

		private static bool GetNameFromID(string id, out string name) {
			if (DataLoader.Prototypes.TryGetValue(id, out Prototype proto)) {
				name = proto.Name;
				return true;
			} else {
			#if DEBUG
				name = $"[no prototype with id '{id}']";
			#else
				name = id;
			#endif
				return false;
			}
		}

		private static void HandleCommandWithArg(string cmd, string arg) {
			switch (cmd) {
				case "fg": ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), arg, true); break;
				case "bg": BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), arg, true); break;
				case "ch/b": CharsPerBeep = int.Parse(arg); break;
				case "ms/ch": MillisPerChar = int.Parse(arg); break;
				case "bHz": BeepFrequency = int.Parse(arg); break;
				case "bMs": BeepDuration = int.Parse(arg); break;
				case "name":
					if (GetNameFromID(arg, out string name)) {
						Write(name);
					} else {
					#if DEBUG
						Write($"{{darkred}}({Escape(name)})");
					#else
						Write(Escape(name));
					#endif
					}
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
				case "push": PushState(); break;
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

		public static State GetState() {
			return new State {
				foregroundColor = ForegroundColor,
				backgroundColor = BackgroundColor,
				millisPerChar = MillisPerChar,
				beepFrequency = BeepFrequency,
				charsPerBeep = CharsPerBeep,
				beepDuration = BeepDuration,
			};
		}

		public static void SetState(State state) {
			ForegroundColor = state.foregroundColor;
			BackgroundColor = state.backgroundColor;
			MillisPerChar = state.millisPerChar;
			BeepFrequency = state.beepFrequency;
			CharsPerBeep = state.charsPerBeep;
			BeepDuration = state.beepDuration;
		}

		public static string Escape(string text) {
			return text.Replace("{", "{{");
		}

		public struct State {
			public ConsoleColor foregroundColor;
			public ConsoleColor backgroundColor;
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

		public static int GetFormattedLength(string formattedText) {
			int Formatbegin = formattedText.IndexOf('{');
			if (Formatbegin == -1) {
				return formattedText.Length;
			}

			int FormatEnd = formattedText.IndexOf('}', Formatbegin + 1);
			if (FormatEnd == -1) {
				throw new TerminalFormatException("Missing closing brace in terminal format string.");
			}

			int additionalLengthFromCommands = 0;
			int cmdBegin = Formatbegin + 1;
			while (cmdBegin < FormatEnd) {
				int cmdEnd = formattedText.IndexOf(';', cmdBegin, FormatEnd - cmdBegin);
				if (cmdEnd == -1) {
					cmdEnd = FormatEnd;
				}
				if (cmdBegin == cmdEnd) {
					break;
				}
				int cmdArg = formattedText.IndexOf(':', cmdBegin, cmdEnd - cmdBegin);
				if (cmdArg == -1) {
					// formatting command doesn't have an argument.
					// none of these currently affect the length of the printed string,
					// so I will ignore them.
				} else {
					string cmdName = formattedText[cmdBegin..cmdArg];
					switch (cmdName) {
						case "name":
							string id = formattedText[(cmdArg + 1)..cmdEnd];
							GetNameFromID(id, out string name);
							additionalLengthFromCommands += GetFormattedLength(name);
							break;
					}
				}
				cmdBegin = cmdEnd + 1;
			}

			if (FormatEnd + 1 < formattedText.Length && formattedText[FormatEnd + 1] == '(') {
				int depth = 1;
				int parenBegin = FormatEnd + 2;
				int remainder = parenBegin;

				for (; depth > 0; ++remainder) {
					if (remainder >= formattedText.Length) {
						throw new TerminalFormatException("Unbalanced parentheses in terminal format string.");
					}

					switch (formattedText[remainder]) {
						case '(': ++depth; break;
						case ')': --depth; break;
					}
				}

				int parenEnd = remainder - 1;

				return Formatbegin + additionalLengthFromCommands
					+ GetFormattedLength(formattedText[parenBegin..parenEnd])
					+ (remainder < formattedText.Length ? GetFormattedLength(formattedText[remainder..]) : 0);

			} else {
				return Formatbegin + additionalLengthFromCommands
					+ (FormatEnd + 1 < formattedText.Length ? GetFormattedLength(formattedText[(FormatEnd + 1)..]) : 0);
			}
		}
	}

	public class TerminalFormatException : Exception {
		public TerminalFormatException(string message) : base(message) { }
	}
}
