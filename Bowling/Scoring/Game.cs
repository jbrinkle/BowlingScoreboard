using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bowling.Scoring
{
    internal interface IGame
    {
        /// <summary>
        /// Adds a player to the game
        /// </summary>
        void AddPlayer(IPlayer p);

        /// <summary>
        /// Returns the collection of players
        /// </summary>
        IReadOnlyList<IPlayer> Players { get; }

        /// <summary>
        /// Set the current frame number
        /// </summary>
        //void SetFrame(int frameNum);

        /// <summary>
        /// Record a roll
        /// </summary>
        /// <param name="p">The player whose turn it is</param>
        /// <param name="mark">The scoring mark</param>
        /// <returns>True if the frame is completed for this player</returns>
        //bool RecordRoll(IPlayer p, char mark);

        /// <summary>
        /// Retrieve game data for display purposes
        /// </summary>
        string[][] GetDisplayData();
    }


    internal class Game : IGame
    {
        private int frame;
        private List<IPlayer> players = new List<IPlayer>();

        public IReadOnlyList<IPlayer> Players => players;

        public void AddPlayer(IPlayer p)
        {
            players.Add(p);
        }

        public bool RecordRoll(IPlayer p, char mark)
        {
            return p.RecordRoll(frame, mark);
        }

        public void SetFrame(int frameNum)
        {
            frame = frameNum;
        }

        public string[][] GetDisplayData()
        {
            var playerData = new string[players.Count][];

            for (int i = 0; i < players.Count; i++)
            {
                playerData[i] = players[i].GetDisplayData();
            }

            return playerData;
        }
    }
}
