using System;
using System.Linq;
using System.Text;
using Bowling.Scoring;

namespace Bowling
{
    class Program
    {
        private IGame game;
        private ScoreboardViewer viewer;

        static void Main(string[] args)
        {
            var bowlingGame = new Program();

            //var rightEdge = Console.WindowWidth;
            //var topEdge = Console.WindowTop;
            //var text = $"{rightEdge},{topEdge}";
            //Console.CursorTop = topEdge;
            //Console.CursorLeft = rightEdge - text.Length;
            //Console.BackgroundColor = ConsoleColor.DarkGray;
            //Console.ForegroundColor = ConsoleColor.Yellow;

            bowlingGame.Startup();

            bowlingGame.RunGameLoop();
        }

        private Program()
        {
            game = new Game();
            viewer = new ScoreboardViewer(game);
        }

        private void Startup()
        {
            Console.Clear();

            Console.WriteLine("Bowling Scoresheet (v1.0)");
            Console.WriteLine("By Jason Brinkle");
            Console.WriteLine("Copyright (c) 2018");
            Console.WriteLine();

            var error = true;
            var playerCount = 0;

            while (error)
            {
                error = false;
                Console.Write("How many players this time? ");
                var playerCountAnswer = Console.ReadLine();

                if (!int.TryParse(playerCountAnswer, out playerCount))
                {
                    Console.WriteLine("Come on, dude. Seriously.");
                    error = true;
                }

                if (playerCount <= 0)
                {
                    Console.WriteLine("So... you didn't want to play?");
                    error = true;
                }

                if (playerCount > 6)
                {
                    Console.WriteLine("To save you from a really looong game, I'm going to ignore that response. Try playing with 6 or less players.");
                    error = true;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Great! Let's get some names...");
            Console.WriteLine();

            int playerNum = 1;
            while (playerNum <= playerCount)
            {
                Console.Write($"What is player #{playerNum}'s name? ");
                var name = Console.ReadLine();

                if (name.Length > 10) name = name.Substring(0, 10);

                var player = new Player(name);
                game.AddPlayer(player);
                playerNum++;
            }
        }

        private void RunGameLoop()
        {
            var frame = 0;

            while (frame < 10)
            {
                foreach (var player in game.Players)
                {
                    viewer.PrintScoreBoard();

                    GetRolls(player, frame);
                }

                frame++;
            }

            viewer.PrintScoreBoard();
        }

        private void GetRolls(IPlayer player, int frame)
        {
            Console.WriteLine();
            Console.WriteLine($"PLAYER {player.Name} FRAME {frame + 1}");

            var done = false;
            var roll = 1;
            while (!done)
            {
                Console.WriteLine($"Roll #{roll}: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                try
                {
                    done = player.RecordRoll(frame, input.First());
                    roll++;
                }
                catch (BowlingScoreException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
