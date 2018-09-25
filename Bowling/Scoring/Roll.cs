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

        private Roll(char mark)
        {
            Mark = mark;
        }

        public char Mark { get; }

        public bool IsSpare => Mark == '/';

        public bool IsStrike => Mark == 'X';

        public bool IsGutter => Mark == '-';

        public bool IsNotStrikeNorSpare => !IsSpare && !IsStrike;

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
            return Mark.ToString();
        }

        public static Roll Create(char mark)
        {
            if (!IsValidMark(mark)) throw new ArgumentException("Invalid bowling mark.");

            if (mark == MarkZero) return new Roll(MarkGutter);
            if (mark == MarkLowerStrike) return new Roll(MarkUpperStrike);

            return new Roll(mark);
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
