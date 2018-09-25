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

        private IGame game;

        private const int SpaceForName = 12;
        private const int SpaceForRegularFrame = 5;
        private const int SpaceForTenthFrame = 7;

        public ScoreboardViewer(IGame g)
        {
            game = g;
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
            Console.WriteLine($"╔{lineOverName}{MultiplyString($"╤{lineOverRegFrame}", 9)}╤{lineOverTenFrame}╗");

            // header line
            var nameHeader = string.Format($" {{0,{SpaceForName - 2}}} ", "Name");
            Console.Write("║" + nameHeader);
            for (var i = 1; i <= 10; i++)
            {
                Console.Write("│");
                var pattern = $" {{0,{(i == 10 ? (-SpaceForTenthFrame + 2) : (-SpaceForRegularFrame + 2))}}} ";
                Console.Write(pattern, i);
            }
            Console.WriteLine("║");
            
            // bottom of header
            Console.WriteLine($"╠{lineOverName}{MultiplyString($"╪{lineOverRegFrame}", 9)}╪{lineOverTenFrame}╣");
        }

        private void WriteFooter()
        {
            var lineOverName = MultiplyString("═", SpaceForName);
            var lineOverRegFrame = MultiplyString("═", SpaceForRegularFrame);
            var lineOverTenFrame = MultiplyString("═", SpaceForTenthFrame);

            Console.WriteLine($"╚{lineOverName}{MultiplyString($"╧{lineOverRegFrame}", 9)}╧{lineOverTenFrame}╝");
        }

        private void WritePlayer(IPlayer player)
        {
            var lineOverName = MultiplyString("═", SpaceForName);
            var lineOverRegFrame = MultiplyString("═", SpaceForRegularFrame);
            var lineOverTenFrame = MultiplyString("═", SpaceForTenthFrame);

            // line 1
            Console.Write($"║ {player.Name,(SpaceForName - 2)} ");
            foreach (var f in player.Frames)
            {
                if (f != player.Frames.Last())
                {
                    Console.Write($"│ {f.ToString(),(-SpaceForRegularFrame + 2)} ");
                }
                else
                {
                    Console.WriteLine($"│ {f.ToString(),(-SpaceForTenthFrame + 2)} ║");
                }
            }
            // line 2
            Console.Write($"║ {" ",(SpaceForName - 2)} ");
            foreach (var f in player.Frames)
            {
                if (f != player.Frames.Last())
                {
                    Console.Write($"│ {f.Score,(-SpaceForRegularFrame + 2)} ");
                }
                else
                {
                    Console.WriteLine($"│ {f.Score,(-SpaceForTenthFrame + 2)} ║");
                }
            }
        }

        private void WritePlayerDividerLine()
        {
            var lineOverName = MultiplyString("─", SpaceForName);
            var lineOverRegFrame = MultiplyString("─", SpaceForRegularFrame);
            var lineOverTenFrame = MultiplyString("─", SpaceForTenthFrame);

            Console.WriteLine($"╟{lineOverName}{MultiplyString($"┼{lineOverRegFrame}", 9)}┼{lineOverTenFrame}╢");
        }

        private string MultiplyString(string s, int count)
        {
            var output = new StringBuilder(s.Length * count);

            for (int i = 0; i < count; i++)
                output.Append(s);

            return output.ToString();
        }
    }
}
