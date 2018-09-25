using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bowling.Scoring;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BowlingTests
{
    [TestFixture]
    public class PlayerTests
    {
        [Test]
        public void Player_CreatesWithTenFrames()
        {
            // arrange

            // act
            var subject = new Player("Bob");

            // assert
            subject.Name.Should().Be("Bob");
            subject.Frames.Count.Should().Be(10);
            for (var i = 0; i < 9; i++)
            {
                subject.Frames[i].Should().BeOfType<RegularFrame>();
            }
            subject.Frames.Last().Should().BeOfType<TenthFrame>();
        }

        [Test]
        public void Player_AutoUpdateScoresForStrikeNextFrame_Works()
        {
            // arrange
            var subject = new Player("Bob");

            // act
            subject.RecordRoll(0, 'X');
            subject.RecordRoll(1, '1');
            subject.RecordRoll(1, '1').Should().BeTrue();

            // assert
            subject.Score.Should().Be(12);
        }

        [Test]
        public void Player_AutoUpdateScoresForStrikeNextTwoFrames_Works()
        {
            // arrange
            var subject = new Player("Bob");

            // act
            subject.RecordRoll(0, 'X').Should().BeTrue();
            subject.RecordRoll(1, 'X').Should().BeTrue();
            subject.Score.Should().Be(0); // needs a second frame of data so delays

            subject.RecordRoll(2, '1');
            subject.RecordRoll(2, '-'); // allows frame 1 to be scored, but also second frame

            // assert
            subject.Score.Should().Be(32);
        }

        [Test]
        public void Player_AutoUpdateScoresForSpare_Works()
        {
            // arrange
            var subject = new Player("Bob");

            // act
            subject.RecordRoll(0, '4');
            subject.RecordRoll(0, '/').Should().BeTrue();
            subject.RecordRoll(1, '1');
            subject.RecordRoll(1, '-'); // finish the frame to trigger scoring first frame

            // assert
            subject.Score.Should().Be(11);
        }

        [Test]
        public void Player_GutterBallsAddZeroToScore_Works()
        {
            // arrange
            var subject = new Player("Bob");

            // act
            subject.RecordRoll(0, '4');
            subject.RecordRoll(0, '2').Should().BeTrue();
            subject.RecordRoll(1, '-');
            subject.RecordRoll(1, '-');

            // assert
            subject.Score.Should().Be(6);
        }

        [Test]
        [TestCase('4', '/', '1', '-', 11)]
        [TestCase('X', null, '3', '3', 16)]
        public void Player_AutoUpdateScoreForNinthFrame_Works(char frame9roll1, char? frame9roll2, char frame10roll1, char frame10roll2, int expectedScore)
        {
            // arrange
            var subject = new Player("Bob");

            // act
            subject.RecordRoll(8, frame9roll1);
            if (frame9roll2.HasValue) subject.RecordRoll(8, frame9roll2.Value);
            subject.RecordRoll(9, frame10roll1);
            subject.RecordRoll(9, frame10roll2);

            // assert
            subject.Score.Should().Be(expectedScore);
        }

        [Test]
        public void Player_GetDisplayDataReturnsData()
        {
            // arrange
            var subject = new Player("Bob");
            subject.Frames[0].RecordRoll('8');
            subject.Frames[0].RecordRoll('1');
            subject.Frames[1].RecordRoll('3');
            subject.Frames[1].RecordRoll('/');

            // act
            var result = subject.GetDisplayData();

            // assert
            result[0].Should().Be("Bob");
            result[1].Should().Be("81");
            result[2].Should().Be("3/");
        }
    }
}
