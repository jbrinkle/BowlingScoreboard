using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bowling.Scoring
{
    internal interface IPlayer
    {
        /// <summary>
        /// Player name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Frames for the player
        /// </summary>
        IReadOnlyList<IFrame> Frames { get; }

        /// <summary>
        /// Record a roll
        /// </summary>
        /// <param name="frame">The frame</param>
        /// <param name="mark">The bowling score mark</param>
        /// <returns>True if the frame is completed</returns>
        bool RecordRoll(int frame, char mark);

        /// <summary>
        /// Retrieve game data for display purposes
        /// </summary>
        string[] GetDisplayData();
    }

    internal class Player : IPlayer
    {
        private readonly List<IFrame> frames;
        private Dictionary<int, List<int>> framesNeedingScoreUpdates = new Dictionary<int, List<int>>();
        
        public string Name { get; }

        public int Score { get; private set; } = 0;

        public IReadOnlyList<IFrame> Frames => frames;

        public Player(string name)
        {
            Name = name;

            frames = new List<IFrame>
            {
                new RegularFrame(),
                new RegularFrame(),
                new RegularFrame(),
                new RegularFrame(),
                new RegularFrame(),
                new RegularFrame(),
                new RegularFrame(),
                new RegularFrame(),
                new RegularFrame(),
                new TenthFrame()
            };
        }

        public bool RecordRoll(int frameIndex, char mark)
        {
            var frame = frames[frameIndex];
            
            frame.RecordRoll(mark);

            if (frame.IsComplete)
            {
                var scoreUpdateTriggerIndex = frameIndex;
                if (framesNeedingScoreUpdates.ContainsKey(scoreUpdateTriggerIndex))
                {
                    foreach (var indexOfFrameNeedingScore in framesNeedingScoreUpdates[scoreUpdateTriggerIndex])
                    {
                        UpdateScoreByFrameIndex(indexOfFrameNeedingScore);
                        Score = frames[indexOfFrameNeedingScore].Score ?? Score;
                    }
                }
                else if (frameIndex > 0)
                {
                    Score = frames[frameIndex - 1].Score ?? 0;
                }

                if (frameIndex == 9)
                {
                    frame.UpdateScore(Score, null, null);
                }
                else if (frame.Roll1.IsStrike)
                {
                    RememberToUpdateScoreLater(frameIndex + 1, frameIndex);
                    RememberToUpdateScoreLater(frameIndex + 2, frameIndex);
                }
                else if (frame.Roll2.IsSpare)
                {
                    RememberToUpdateScoreLater(frameIndex + 1, frameIndex);
                }
                else
                {
                    frame.UpdateScore(Score, null, null);
                }

                return true;
            }

            return false;
        }

        private void RememberToUpdateScoreLater(int indexOfWhenToRemember, int indexOfFrameNeedingScore)
        {
            if (!framesNeedingScoreUpdates.ContainsKey(indexOfWhenToRemember))
            {
                framesNeedingScoreUpdates.Add(indexOfWhenToRemember, new List<int>());
            }

            framesNeedingScoreUpdates[indexOfWhenToRemember].Add(indexOfFrameNeedingScore);
        }

        private void UpdateScoreByFrameIndex(int indexOfFrameNeedingScore)
        {
            var frame = frames[indexOfFrameNeedingScore];
            var next = frames[indexOfFrameNeedingScore + 1];
            var nextnext = indexOfFrameNeedingScore >= 8 ? null : frames[indexOfFrameNeedingScore + 2];

            frame.UpdateScore(Score, next, nextnext);
        }

        public string[] GetDisplayData()
        {
            var data = new List<string>();

            data.Add(Name);

            foreach (var frame in frames)
            {
                if (!frame.IsComplete) break;

                data.Add(frame.ToString());
            }

            return data.ToArray();
        }
    }
}
