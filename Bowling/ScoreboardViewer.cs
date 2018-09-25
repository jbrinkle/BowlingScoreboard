using Bowling.Scoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bowling
{
    internal class ScoreboardViewer
    {
        // │─┌┐└┘├┤┬┴┼═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬
        private ConsoleColor originalBgColor;
        private IGame game;

        private const int SpaceForName = 12;
        private const int SpaceForRegularFrame = 5;
        private const int SpaceForTenthFrame = 7;

        public const ConsoleColor TableColor = ConsoleColor.Gray;
        public const ConsoleColor HeaderColor = ConsoleColor.White;
        public const ConsoleColor NameColor = ConsoleColor.Yellow;
        public const ConsoleColor RollColor = ConsoleColor.White;
        public const ConsoleColor ScoreColor = ConsoleColor.Cyan;

        public ScoreboardViewer(IGame g)
        {
            game = g;
            originalBgColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        ~ScoreboardViewer()
        {
            Console.BackgroundColor = originalBgColor;
        }

        public void PrintScoreBoard()
        {
            Console.Clear();

            WriteHeader();

            // Players
            foreach (var player in game.Players)
            {
                WritePlayer(player);
                if (player != game.Players.Last()) WritePlayerDividerLine();
            }

            WriteFooter();
        }

        private void WriteHeader()
        {
            var lineOverName = MultiplyString("═", SpaceForName);
            var lineOverRegFrame = MultiplyString("═", SpaceForRegularFrame);
            var lineOverTenFrame = MultiplyString("═", SpaceForTenthFrame);

            // top bar
            WriteText($"╔{lineOverName}{MultiplyString($"╤{lineOverRegFrame}", 9)}╤{lineOverTenFrame}╗", true, TableColor);

            // header line
            WriteText("║", false, TableColor);
            var nameHeader = string.Format($" {{0,{SpaceForName - 2}}} ", "Name");
            WriteText(nameHeader, false, HeaderColor);

            for (var i = 1; i <= 10; i++)
            {
                WriteText("│", false, TableColor);
                var pattern = $" {{0,{(i == 10 ? (-SpaceForTenthFrame + 2) : (-SpaceForRegularFrame + 2))}}} ";
                WriteText(string.Format(pattern, i), false, HeaderColor);
            }
            WriteText("║", true, TableColor);
            
            // bottom of header
            WriteText($"╠{lineOverName}{MultiplyString($"╪{lineOverRegFrame}", 9)}╪{lineOverTenFrame}╣", true, TableColor);
        }

        private void WriteFooter()
        {
            var lineOverName = MultiplyString("═", SpaceForName);
            var lineOverRegFrame = MultiplyString("═", SpaceForRegularFrame);
            var lineOverTenFrame = MultiplyString("═", SpaceForTenthFrame);

            WriteText($"╚{lineOverName}{MultiplyString($"╧{lineOverRegFrame}", 9)}╧{lineOverTenFrame}╝", true, TableColor);
        }

        private void WritePlayer(IPlayer player)
        {
            var lineOverName = MultiplyString("═", SpaceForName);
            var lineOverRegFrame = MultiplyString("═", SpaceForRegularFrame);
            var lineOverTenFrame = MultiplyString("═", SpaceForTenthFrame);

            // line 1
            WriteText("║", false, TableColor);
            WriteText($" {player.Name,(SpaceForName - 2)} ", false, NameColor);
            foreach (var f in player.Frames)
            {
                if (f != player.Frames.Last())
                {
                    WriteText("│", false, TableColor);
                    WriteText($" { f.ToString(),(-SpaceForRegularFrame + 2)} ", false, RollColor);
                }
                else
                {
                    WriteText("│", false, TableColor);
                    WriteText($" { f.ToString(),(-SpaceForTenthFrame + 2)} ", false, RollColor);
                    WriteText("║", true, TableColor);
                }
            }
            // line 2
            WriteText("║", false, TableColor);
            WriteText($" { " ",(SpaceForName - 2)} ", false);
            foreach (var f in player.Frames)
            {
                if (f != player.Frames.Last())
                {
                    WriteText("│", false, TableColor);
                    WriteText($" { f.Score,(-SpaceForRegularFrame + 2)} ", false, ScoreColor);
                }
                else
                {
                    WriteText("│", false, TableColor);
                    WriteText($" {f.Score,(-SpaceForTenthFrame + 2)} ", false, ScoreColor);
                    WriteText("║", true, TableColor);
                }
            }
        }

        private void WritePlayerDividerLine()
        {
            var lineOverName = MultiplyString("─", SpaceForName);
            var lineOverRegFrame = MultiplyString("─", SpaceForRegularFrame);
            var lineOverTenFrame = MultiplyString("─", SpaceForTenthFrame);

            WriteText($"╟{lineOverName}{MultiplyString($"┼{lineOverRegFrame}", 9)}┼{lineOverTenFrame}╢", true, TableColor);
        }

        private string MultiplyString(string s, int count)
        {
            var output = new StringBuilder(s.Length * count);

            for (int i = 0; i < count; i++)
                output.Append(s);

            return output.ToString();
        }

        public static void WriteText(string text, bool newline, ConsoleColor color = ConsoleColor.Black)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color == ConsoleColor.Black ? originalColor : color;

            Console.Write(text);
            if (newline) Console.WriteLine();

            Console.ForegroundColor = originalColor;
        }
    }
}
