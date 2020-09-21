using System;
using System.Collections.Generic;
using RPGCuzWhyNot.Primitives;

namespace RPGCuzWhyNot.Systems {
	public static partial class Terminal {
		private readonly static List<string> inputHistory = new List<string>();
		private static string inputBuffer = "";
		private static int caretPosition = 0;
		private static int historyIndex;

		public static string ReadLine() {
			historyIndex = inputHistory.Count - 1;
			while (true) {
				ConsoleKeyInfo key = Console.ReadKey(true);
				switch (key.Key) {
					case ConsoleKey.Enter:
						if (inputBuffer != "" && (inputHistory.Count == 0 || inputHistory[^1] != inputBuffer)) {
							inputHistory.Add(inputBuffer);
						}
						string ret = inputBuffer;
						caretPosition = 0;
						inputBuffer = "";
						return ret;
					case ConsoleKey.Backspace:
						if (caretPosition > 0) {
							string latter = inputBuffer[caretPosition..];
							inputBuffer = inputBuffer[..(caretPosition - 1)] + latter;
							--caretPosition;
							Console.Write("\b");
							Console.CursorVisible = false;
							Vec2 cpos = CursorPosition;
							Console.Write(latter);
							Console.Write(" ");
							CursorPosition = cpos;
							Console.CursorVisible = true;
						}
						break;
					case ConsoleKey.Delete:
						if (caretPosition < inputBuffer.Length) {
							string latter = inputBuffer[(caretPosition + 1)..];
							inputBuffer = inputBuffer[..caretPosition] + latter;
							Console.CursorVisible = false;
							Vec2 cpos = CursorPosition;
							Console.Write(latter);
							Console.Write(" ");
							CursorPosition = cpos;
							Console.CursorVisible = true;
						}
						break;
					case ConsoleKey.LeftArrow:
						if (caretPosition > 0) {
							--caretPosition;
							Vec2 cpos = CursorPosition;
							--cpos.x;
							CursorPosition = cpos;
						}
						break;
					case ConsoleKey.RightArrow:
						if (caretPosition < inputBuffer.Length) {
							++caretPosition;
							Vec2 cpos = CursorPosition;
							++cpos.x;
							CursorPosition = cpos;
						}
						break;
					case ConsoleKey.UpArrow:
						if (historyIndex > 0) {
							--historyIndex;
							Console.CursorVisible = false;
							Vec2 cpos = CursorPosition;
							cpos.x -= caretPosition;
							CursorPosition = cpos;
							Console.Write(new string(' ', inputBuffer.Length));
							CursorPosition = cpos;
							inputBuffer = inputHistory[historyIndex];
							Console.Write(inputBuffer);
							caretPosition = inputBuffer.Length;
							cpos.x += caretPosition;
							Console.CursorVisible = true;
						}
						break;
					case ConsoleKey.DownArrow:
						if (historyIndex + 1 < inputHistory.Count) {
							++historyIndex;
							Console.CursorVisible = false;
							Vec2 cpos = CursorPosition;
							cpos.x -= caretPosition;
							CursorPosition = cpos;
							Console.Write(new string(' ', inputBuffer.Length));
							CursorPosition = cpos;
							inputBuffer = inputHistory[historyIndex];
							Console.Write(inputBuffer);
							caretPosition = inputBuffer.Length;
							cpos.x += caretPosition;
							Console.CursorVisible = true;
						}
						break;
					case ConsoleKey.Home: {
						Vec2 cpos = CursorPosition;
						cpos.x -= caretPosition;
						CursorPosition = cpos;
						caretPosition = 0;
						break;
					}
					case ConsoleKey.End: {
						Vec2 cpos = CursorPosition;
						cpos.x += inputBuffer.Length - caretPosition;
						CursorPosition = cpos;
						caretPosition = inputBuffer.Length;
						break;
					}
					case ConsoleKey.Escape: {
						Console.CursorVisible = false;
						Vec2 cpos = CursorPosition;
						cpos.x -= caretPosition;
						CursorPosition = cpos;
						Console.Write(new string(' ', inputBuffer.Length));
						CursorPosition = cpos;
						Console.CursorVisible = true;
						caretPosition = 0;
						inputBuffer = "";
						break;
					}

					case ConsoleKey.PageUp:
					case ConsoleKey.PageDown:
						break;

					default: {
						string latter = inputBuffer[caretPosition..];
						inputBuffer = inputBuffer[..caretPosition] + key.KeyChar + latter;
						++caretPosition;
						Console.Write(key.KeyChar);
						Console.CursorVisible = false;
						Vec2 cpos = CursorPosition;
						Console.Write(latter);
						CursorPosition = cpos;
						Console.CursorVisible = true;
						break;
					}
				}
			}
		}
	}
}
