using System;

namespace LegacyTUIComp
{
    public class UI
    {
        public static void showOptions(string tittle, string[] options, string backTip, string[]? additional = null)
        {
            if (additional == null) additional = new string[0];

            Console.WriteLine($"{tittle}\n{new string('-',20)}");

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }
            Console.WriteLine();

            for (int i = 0; i < additional.Length; i++)
            {
                char letter = (char)('a' + i);
                Console.WriteLine($"{letter}. {additional[i]}");
            }

            Console.WriteLine($"0. {backTip}");
            Console.Write(">_: ");
        }

        public static char getChar()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true); // true — чтобы не показывать на экране
            return keyInfo.KeyChar;
        }
    }
}
