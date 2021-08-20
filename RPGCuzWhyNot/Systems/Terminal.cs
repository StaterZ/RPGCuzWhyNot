using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RPGCuzWhyNot.Primitives;
using RPGCuzWhyNot.Systems.Data;
using RPGCuzWhyNot.Systems.Data.Prototypes;
using RPGCuzWhyNot.Systems.VirtualTerminal;

namespace RPGCuzWhyNot.Systems {
	public static class Terminal {
		private static readonly Stack<State> stateStack = new Stack<State>();

		private static int cursorRowDisplacement;

		private static Color currentForegroundColor = new Color(Console.ForegroundColor);
		private static Color currentBackgroundColor = new Color(Console.BackgroundColor);

		public static Color ForegroundColor {
			get => currentForegroundColor;
			set {
				if (currentForegroundColor == value) return;
				currentForegroundColor = value;
				VT.TrueColor(ColorLayer.Foreground, value);
			}
		}

		public static Color BackgroundColor {
			get => currentBackgroundColor;
			set {
				if (currentBackgroundColor == value) return;
				currentBackgroundColor = value;
				VT.TrueColor(ColorLayer.Background, value);
			}
		}

		public static bool IsCursorVisible {
			get => Console.CursorVisible;
			set => Console.CursorVisible = value;
		}

		public static int CursorX {
			get => Console.CursorLeft;
			set => Console.CursorLeft = value;
		}

		public static int CursorY {
			get => Console.CursorTop + cursorRowDisplacement;
			set => Console.CursorTop = value - cursorRowDisplacement;
		}

		public static Vec2 CursorPosition {
			get => new Vec2(CursorX, CursorY);
			set {
				CursorX = value.x;
				CursorY = value.y;
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

		private static int charBeepCounter;

		public static StateScope PushState() {
			stateStack.Push(GetState());
			return new StateScope();
		}

		public static void PopState() => SetState(stateStack.Pop());

		public static void WriteLine(string text, int frequency, int charsPerSecond = -1) => Write(text + '\n', frequency, charsPerSecond);
		public static void Write(string text, int frequency, int charsPerSecond = -1) {
			using (PushState()) {
				if (charsPerSecond < 0) {
					MillisPerChar = 1000 / charsPerSecond;
				}
				BeepFrequency = frequency;
				Write(text);
			}
		}

		public static void WriteLine() => Write("\n");
		public static void WriteLine(string text) {
			Write(text);
			Write('\n');
		}

		public static void ClearLine() {
			int startLine = CursorPosition.y;
			CursorPosition = new Vec2(0, startLine);
			WriteLineWithoutDelay(new string(' ', WindowSize.x));
			CursorPosition = new Vec2(0, startLine + 1);
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
		public static void WriteWithoutDelay(string text) {
			using (PushState()) {
				MillisPerChar = 0;
				BeepDuration = 0;
				Write(text);
			}
		}
		public static void WriteWithoutDelay(char c) {
			using (PushState()) {
				MillisPerChar = 0;
				BeepDuration = 0;
				Write(c);
			}
		}


		/// <summary>
		/// Write a line without delay or beeping.
		/// </summary>
		public static void WriteLineWithoutDelay(string text) {
			WriteWithoutDelay(text);
			WriteWithoutDelay('\n');
		}

		/// <summary>
		/// Write without delay or beeping.
		/// </summary>
		public static void WriteLineWithoutDelay() => WriteWithoutDelay('\n');

		public static void WriteRaw(string text) {
			foreach (char c in text) {
				WriteChar(c);
			}
		}

		public static void WriteRawWithoutDelay(string text) {
			PushState();
			MillisPerChar = 0;
			BeepDuration = 0;
			foreach (char c in text) {
				WriteChar(c);
			}
			PopState();
		}

		public static void WriteLineRaw(string text) {
			WriteRaw(text);
			WriteRaw("\n");
		}

		public static void WriteLineRawWithoutDelay(string text) {
			WriteRawWithoutDelay(text);
			WriteRawWithoutDelay("\n");
		}

		private static void WriteChar(char c) {
			int consoleLeftBefore = Console.CursorLeft;
			int consoleTopBefore = Console.CursorTop;
			Console.Write(c);
			int consoleLeftAfter = Console.CursorLeft;
			int consoleTopAfter = Console.CursorTop;

			if (consoleTopBefore == consoleTopAfter) {
				// If the cursor didn't actually move down, we may need to emulate it.
				switch (c) {
					case '\r': // carrige return
					case '\b': // backspace
						break; // never change line

					case '\n': // newline/linefeed
						++cursorRowDisplacement;
						break; // always change line

					default:
						if (consoleLeftAfter < consoleLeftBefore) {
							// If the cursor moved back, we assume a new line has begun.
							++cursorRowDisplacement;
						}
						break;
				}
			}

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
		}

		private static int Decode(string text, int offset) {
			int braceEnd = text.IndexOf('}', offset);
			if (braceEnd == -1) throw new ArgumentException("Missing closing brace.");

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
				if (parenEnd >= text.Length) throw new ArgumentException("Missing closing parenthesis.");
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
				case "fg": ForegroundColor = Color.GetPredefinedColor(arg); break;
				case "bg": BackgroundColor = Color.GetPredefinedColor(arg); break;
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
			if (Color.TryGetPredefinedColor(cmd, out Color color)) {
				ForegroundColor = color;
				return;
			}
			switch (cmd) {
				case "push": PushState(); break;
				case "pop": PopState(); break;
				default: throw new ArgumentException("Unknown command.");
			}
		}

		public static State GetState() {
			return new State {
				foregroundColor = ForegroundColor,
				backgroundColor = BackgroundColor,
				millisPerChar = MillisPerChar,
				beepFrequency = BeepFrequency,
				charsPerBeep = CharsPerBeep,
				beepDuration = BeepDuration,
				cursorVisible = IsCursorVisible,
			};
		}

		public static void SetState(State state) {
			ForegroundColor = state.foregroundColor;
			BackgroundColor = state.backgroundColor;
			MillisPerChar = state.millisPerChar;
			BeepFrequency = state.beepFrequency;
			CharsPerBeep = state.charsPerBeep;
			BeepDuration = state.beepDuration;
			IsCursorVisible = state.cursorVisible;
		}

		public static string Escape(string text) {
			return text.Replace("{", "{{");
		}

		public static string ReadLine() => Console.ReadLine();

		public static ConsoleKeyInfo ReadKey(bool intercept = false) => Console.ReadKey(intercept);

		public static int GetFormattedLength(string formattedText) {
			int formatBegin = formattedText.IndexOf('{');
			if (formatBegin == -1) {
				return formattedText.Length;
			}

			int formatEnd = formattedText.IndexOf('}', formatBegin + 1);
			if (formatEnd == -1) {
				throw new TerminalFormatException("Missing closing brace in terminal format string.");
			}

			int additionalLengthFromCommands = 0;
			int cmdBegin = formatBegin + 1;
			while (cmdBegin < formatEnd) {
				int cmdEnd = formattedText.IndexOf(';', cmdBegin, formatEnd - cmdBegin);
				if (cmdEnd == -1) {
					cmdEnd = formatEnd;
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

			if (formatEnd + 1 < formattedText.Length && formattedText[formatEnd + 1] == '(') {
				int depth = 1;
				int parenBegin = formatEnd + 2;
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

				return formatBegin + additionalLengthFromCommands
					+ GetFormattedLength(formattedText[parenBegin..parenEnd])
					+ (remainder < formattedText.Length ? GetFormattedLength(formattedText[remainder..]) : 0);

			} else {
				return formatBegin + additionalLengthFromCommands
					+ (formatEnd + 1 < formattedText.Length ? GetFormattedLength(formattedText[(formatEnd + 1)..]) : 0);
			}
		}

		/// <summary>
		/// Remove all formatting from the string by expanding any formats that produce text and removing any that don't.
		/// </summary>
		/// <param name="formattedText">The string containing formats to remove.</param>
		/// <returns>Returns the expanded string.</returns>
		public static string ExpandFormatString(string formattedText) {
			StringBuilder sb = new StringBuilder();
			ExpandFormatString(formattedText, sb);
			return sb.ToString();
		}

		/// <summary>
		/// Remove all formatting from the string by expanding any formats that produce text and removing any that don't.
		/// </summary>
		/// <param name="formattedText">The string containing formats to remove.</param>
		/// <param name="builder">The string builder to place the resulting expanded string into.</param>
		public static void ExpandFormatString(string formattedText, StringBuilder builder) {
			int formatBegin = formattedText.IndexOf('{');
			if (formatBegin == -1) {
				builder.Append(formattedText);
				return;
			}

			int formatEnd = formattedText.IndexOf('}', formatBegin + 1);
			if (formatEnd == -1) {
				throw new TerminalFormatException("Missing closing brace in terminal format string.");
			}

			int cmdBegin = formatBegin + 1;
			while (cmdBegin < formatEnd) {
				int cmdEnd = formattedText.IndexOf(';', cmdBegin, formatEnd - cmdBegin);
				if (cmdEnd == -1) {
					cmdEnd = formatEnd;
				}
				if (cmdBegin == cmdEnd) {
					break;
				}
				int cmdArg = formattedText.IndexOf(':', cmdBegin, cmdEnd - cmdBegin);
				if (cmdArg == -1) {
					// Formatting command doesn't have an argument.
					// None of these currently add any text to the printed string,
					// so I will ignore them.
				} else {
					string cmdName = formattedText[cmdBegin..cmdArg];
					switch (cmdName) {
						case "name":
							string id = formattedText[(cmdArg + 1)..cmdEnd];
							GetNameFromID(id, out string name);
							ExpandFormatString(name, builder);
							break;
					}
				}
				cmdBegin = cmdEnd + 1;
			}

			if (formatEnd + 1 < formattedText.Length && formattedText[formatEnd + 1] == '(') {
				int depth = 1;
				int parenBegin = formatEnd + 2;
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

				ExpandFormatString(formattedText[parenBegin..parenEnd], builder);
				if (remainder < formattedText.Length) {
					ExpandFormatString(formattedText[remainder..], builder);
				}
			} else {
				if (formatEnd + 1 < formattedText.Length) {
					ExpandFormatString(formattedText[(formatEnd + 1)..], builder);
				}
			}
		}

		public static void ResetColor() {
			VT.ResetColor();
			currentForegroundColor = new Color(Console.ForegroundColor);
			currentBackgroundColor = new Color(Console.BackgroundColor);
		}

		public struct State {
			public Color foregroundColor;
			public Color backgroundColor;
			public int charsPerBeep;
			public int millisPerChar;
			public int beepFrequency;
			public int beepDuration;
			public bool cursorVisible;
		}

		public static void Beep(int frequency, int duration) {
			Console.Beep(frequency, duration);
		}

		public static void Clear() {
			Console.Clear();
			cursorRowDisplacement = 0;
		}

		public struct StateScope : IDisposable {
			public void Dispose() {
				PopState();
			}
		}
	}

	public class TerminalFormatException : Exception {
		public TerminalFormatException(string message) : base(message) { }
	}
}
