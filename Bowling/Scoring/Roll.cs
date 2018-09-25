using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bowling.Scoring
{
    internal class Roll
    {
        private const char MarkLowerStrike = 'x';
        private const char MarkUpperStrike = 'X';
        private const char MarkSpare = '/';
        private const char MarkGutter = '-';
        private const char MarkZero = '0';
        private const char MarkNine = '9';

        public char Mark { get; private set; }

        public bool IsSet => Mark != '\0';

        public bool IsSpare => Mark == MarkSpare;

        public bool IsStrike => Mark == MarkUpperStrike;

        public bool IsGutter => Mark == MarkGutter;

        public bool IsNotStrikeNorSpare => !IsSpare && !IsStrike && IsSet;

        public int Value
        {
            get
            {
                if (int.TryParse(Mark.ToString(), out int pinCount)) return pinCount;
                return 0;
            }
        }

        public override string ToString()
        {
            return IsSet ? Mark.ToString() : null;
        }

        public void SetMark(char mark)
        {
            if (!IsValidMark(mark)) throw new ArgumentException("Invalid bowling mark.");

            if (mark == MarkZero) Mark = MarkGutter;
            else if (mark == MarkLowerStrike) Mark = MarkUpperStrike;
            else Mark = mark;
        }

        public static bool IsValidMark(char mark)
        {
            var validMarks = new int[] { MarkLowerStrike, MarkUpperStrike, MarkSpare, MarkGutter };

            if (!validMarks.Contains(mark) && (mark < MarkZero || mark > MarkNine))
                return false;

            return true;
        }
    }
}
