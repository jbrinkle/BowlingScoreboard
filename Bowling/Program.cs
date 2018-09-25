using System;
using System.Linq;
using System.Text;
using Bowling.Scoring;

namespace Bowling
{
    class Program
    {
        private const ConsoleColor TitleColor = ConsoleColor.Cyan;
        private const ConsoleColor ErrorColor = ConsoleColor.Red;
        private const ConsoleColor QuestionColor = ConsoleColor.White;
        private const ConsoleColor InfoColor = ConsoleColor.Yellow;

        private IGame game;
        private ScoreboardViewer viewer;

        static void Main(string[] args)
        {
            var bowlingGame = new Program();

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

            ScoreboardViewer.WriteText("Bowling Scoresheet (v1.0)", true, TitleColor);
            ScoreboardViewer.WriteText("By Jason Brinkle", true, TitleColor);
            ScoreboardViewer.WriteText("Copyright (c) 2018", true, TitleColor);
            ScoreboardViewer.WriteText(null, true);

            var error = true;
            var playerCount = 0;

            while (error)
            {
                error = false;
                ScoreboardViewer.WriteText("How many players this time? ", false, QuestionColor);
                var playerCountAnswer = Console.ReadLine();

                if (!int.TryParse(playerCountAnswer, out playerCount))
                {
                    ScoreboardViewer.WriteText("Come on, dude. Seriously.", true, ErrorColor);
                    error = true;
                }

                if (playerCount <= 0)
                {
                    ScoreboardViewer.WriteText("So... you didn't want to play?", true, ErrorColor);
                    error = true;
                }

                if (playerCount > 6)
                {
                    ScoreboardViewer.WriteText("To save you from a really looong game, I'm going to ignore that response. Try playing with 6 or less players.", true, ErrorColor);
                    error = true;
                }
            }

            ScoreboardViewer.WriteText(null, true);
            ScoreboardViewer.WriteText("Great! Let's get some names...", true);
            ScoreboardViewer.WriteText(null, true);

            int playerNum = 1;
            while (playerNum <= playerCount)
            {
                ScoreboardViewer.WriteText($"What is player #{playerNum}'s name? ", false, QuestionColor);
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
            ScoreboardViewer.WriteText(null, true);
            ScoreboardViewer.WriteText("PLAYER ", false);
            ScoreboardViewer.WriteText(player.Name, false, InfoColor);
            ScoreboardViewer.WriteText(" -- FRAME ", false);
            ScoreboardViewer.WriteText($"{ frame + 1}", true, InfoColor);
            ScoreboardViewer.WriteText(null, true);

            var done = false;
            var roll = 1;
            while (!done)
            {
                ScoreboardViewer.WriteText($"Roll #{roll}: ", false, QuestionColor);
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;

                try
                {
                    done = player.RecordRoll(frame, input.First());
                    roll++;
                }
                catch (BowlingScoreException ex)
                {
                    ScoreboardViewer.WriteText(ex.Message, true, ErrorColor);
                }
            }
        }
    }
}
