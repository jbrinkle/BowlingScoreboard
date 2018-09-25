using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bowling.Scoring
{
    internal interface IFrame
    {
        Roll Roll1 { get; }

        Roll Roll2 { get; }

        Roll Roll3 { get; }

        void RecordRoll(char mark);

        bool IsComplete { get; }

        int? Score { get; }

        void UpdateScore(int previousScore, IFrame fnext, IFrame fnextnext);
    }

    internal class RegularFrame : IFrame
    {
        public RegularFrame()
        {
            Roll1 = new Roll();
            Roll2 = new Roll();
            Roll3 = new Roll();
        }

        protected int? scoreAtEndOfFrame;

        public Roll Roll1 { get; }

        public Roll Roll2 { get; }

        public virtual Roll Roll3 { get; }

        protected void UniversalChecksOnMark(char mark)
        {
            if (!Roll.IsValidMark(mark))
                throw new BowlingScoreException($"'{mark}' is not a valid bowling score mark");

            if (IsComplete)
                throw new BowlingScoreException("Cannot add a roll to a complete frame.");

            var roll = new Roll();
            roll.SetMark(mark);

            if (!Roll1.IsSet && roll.IsSpare)
                throw new BowlingScoreException("Spare on first roll is not allowed.");

            if (Roll1.IsSet && !Roll2.IsSet && Roll1.Value + roll.Value > 10)
                throw new BowlingScoreException("Pin values of rolls may not exceed 10");

            if (Roll1.IsSet && !Roll2.IsSet && Roll1.Value + roll.Value == 10 && !roll.IsSpare)
                throw new BowlingScoreException("Please use spare notation");
        }

        public virtual void RecordRoll(char mark)
        {
            UniversalChecksOnMark(mark);

            if (!Roll1.IsSet) Roll1.SetMark(mark);
            else Roll2.SetMark(mark);
        }

        public virtual bool IsComplete => Roll1.IsStrike || Roll2.IsSet;

        public int? Score => scoreAtEndOfFrame;

        public virtual void UpdateScore(int previousScore, IFrame fnext, IFrame fnextnext)
        {
            if (scoreAtEndOfFrame != null || !IsComplete) return;

            if (Roll1.IsNotStrikeNorSpare && Roll2.IsNotStrikeNorSpare)
            {
                scoreAtEndOfFrame = previousScore + Roll1.Value + Roll2.Value;
                return;
            }

            if (Roll1.IsStrike)
            {
                if (fnext == null || !fnext.IsComplete) return;

                var score = 10;

                score += fnext.Roll1.IsStrike ? 10 : fnext.Roll1.Value;

                if (fnext.Roll2.IsSet)
                {
                    if (fnext.Roll2.IsSpare) score = 20;
                    else score += fnext.Roll2.Value;
                }
                else
                {
                    if (fnextnext == null || !fnextnext.IsComplete) return;

                    score += fnextnext.Roll1.IsStrike ? 10 : fnextnext.Roll1.Value;
                }

                scoreAtEndOfFrame = previousScore + score;
            }

            if (Roll2.IsSpare)
            {
                if (fnext == null || !fnext.IsComplete) return;

                var score = 10;

                score += fnext.Roll1.IsStrike ? 10 : fnext.Roll1.Value;

                scoreAtEndOfFrame = previousScore + score;
            }
        }

        public override string ToString()
        {
            return Roll1.ToString() + Roll2.ToString();
        }
    }

    internal class TenthFrame : RegularFrame
    {
        public override void RecordRoll(char mark)
        {
            UniversalChecksOnMark(mark);

            var roll = new Roll();
            roll.SetMark(mark);

            if (Roll1.IsStrike)
            {
                if (Roll2.Value + roll.Value > 10)
                    throw new BowlingScoreException("Pin values of rolls may not exceed 10");

                if (Roll2.Value + roll.Value == 10 && !roll.IsSpare)
                    throw new BowlingScoreException("Please use spare notation");
            }

            if ((Roll1.IsStrike && !Roll2.IsSet && roll.IsSpare) ||
                (Roll2.IsStrike && roll.IsSpare))
                throw new BowlingScoreException("Can't have a spare after a strike.");

            if (Roll2.IsSpare && !Roll3.IsSet && roll.IsSpare)
                throw new BowlingScoreException("Can't have a spare after a spare.");

            if (Roll1.IsSet && !Roll1.IsStrike && !Roll2.IsSet && roll.IsStrike)
                throw new BowlingScoreException("Strike cannot follow a single numeric roll.");

            if (!Roll1.IsSet) Roll1.SetMark(mark);
            else if (!Roll2.IsSet) Roll2.SetMark(mark);
            else Roll3.SetMark(mark);
        }

        public override bool IsComplete =>
            (Roll1.IsNotStrikeNorSpare && Roll2.IsNotStrikeNorSpare) ||
            (Roll3.IsSet);

        public override string ToString()
        {
            return Roll1.ToString() + Roll2.ToString() + Roll3.ToString();
        }

        public override void UpdateScore(int previousScore, IFrame fnext, IFrame fnextnext)
        {
            if (scoreAtEndOfFrame != null || !IsComplete) return;

            var score = Roll1.IsStrike ? 10 : Roll1.Value;

            if (Roll2.IsStrike) score += 10;
            else if (Roll2.IsSpare) score = 10;
            else if (Roll2.IsNotStrikeNorSpare) score += Roll2.Value;

            if (!Roll3.IsSet) {; }
            else if (Roll3.IsStrike) score += 10;
            else if (Roll3.IsSpare) score = score - Roll2.Value + 10;
            else if (Roll3.IsNotStrikeNorSpare) score += Roll3.Value;

            scoreAtEndOfFrame = previousScore + score;
        }
    }
}
