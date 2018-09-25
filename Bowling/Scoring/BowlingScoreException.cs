using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Scoring
{
    internal class BowlingScoreException : Exception
    {
        public BowlingScoreException(string message) : base(message) { }
    }
}
