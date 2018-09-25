using System;
using Bowling.Scoring;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BowlingTests
{
    [TestFixture]
    public class FrameTests
    {
        [Test]
        public void RegFrm_NumNumMarks_Valid()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            subject.RecordRoll('4');
            subject.RecordRoll('3');

            // assert
            subject.Roll1.Value.Should().Be(4);
            subject.Roll2.Value.Should().Be(3);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("43");
        }

        [Test]
        public void RegFrm_NumSpareMarks_Valid()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            subject.RecordRoll('4');
            subject.RecordRoll('/');

            // assert
            subject.Roll1.Value.Should().Be(4);
            subject.Roll2.Value.Should().Be(0);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("4/");
        }

        [Test]
        public void RegFrm_StrikeMarks_Valid()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            subject.RecordRoll('x');

            // assert
            subject.Roll1.Value.Should().Be(0);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("X", "Roll class capitalizes");
        }

        [Test]
        public void RegFrm_GutterGutterMarks_Valid()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            subject.RecordRoll('0');
            subject.RecordRoll('-');

            // assert
            subject.Roll1.Value.Should().Be(0);
            subject.Roll2.Value.Should().Be(0);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("--");
        }

        [Test]
        public void RegFrm_SpareFirst_Fails()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            Action test = () => subject.RecordRoll('/');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("Spare on first roll");
        }

        [Test]
        public void RegFrm_AddToCompletedFrame_Fails()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            subject.RecordRoll('X');
            Action test = () => subject.RecordRoll('-');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("complete frame");
        }

        [Test]
        public void RegFrm_PinCountAt10_Fails()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            subject.RecordRoll('5');
            Action test = () => subject.RecordRoll('5');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("spare notation");
        }

        [Test]
        public void RegFrm_PinCountAbove10_Fails()
        {
            // arrange
            var subject = new RegularFrame();

            // act
            subject.RecordRoll('6');
            Action test = () => subject.RecordRoll('6');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("may not exceed 10");
        }

        [Test]
        public void RegFrm_ScoringNumNum_Works()
        {
            // arrange
            var subject = new RegularFrame();
            subject.RecordRoll('6');
            subject.RecordRoll('2');
            subject.Score.Should().BeNull();

            // act
            subject.UpdateScore(0, null, null);

            // assert
            subject.Score.Should().Be(8);
        }

        [Test]
        public void RegFrm_ScoringSpare_Works()
        {
            // arrange
            var subject = new RegularFrame();
            subject.RecordRoll('6');
            subject.RecordRoll('/');
            subject.Score.Should().BeNull();

            var next = new RegularFrame();
            next.RecordRoll('X');

            // act
            subject.UpdateScore(0, next, null);

            // assert
            subject.Score.Should().Be(20);
        }

        [Test]
        public void RegFrm_ScoringStrike_Works()
        {
            // arrange
            var subject = new RegularFrame();
            subject.RecordRoll('X');
            subject.Score.Should().BeNull();

            var next = new RegularFrame();
            next.RecordRoll('6');
            next.RecordRoll('2');

            // act
            subject.UpdateScore(0, next, null);

            // assert
            subject.Score.Should().Be(18);
        }

        [Test]
        public void Frm10_ThirdRollWithoutStrikeOrSpare_Fails()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('3');
            subject.RecordRoll('3');

            subject.IsComplete.Should().BeTrue();

            Action test = () => subject.RecordRoll('1');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("complete frame");
        }

        [Test]
        public void Frm10_StrikeNumNumMarks_Valid()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            subject.RecordRoll('3');
            subject.RecordRoll('1');

            // assert
            subject.Roll1.Value.Should().Be(0);
            subject.Roll2.Value.Should().Be(3);
            subject.Roll3.Value.Should().Be(1);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("X31");
        }

        [Test]
        public void Frm10_StrikeNumSpareMarks_Valid()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            subject.RecordRoll('3');
            subject.RecordRoll('/');

            // assert
            subject.Roll1.Value.Should().Be(0);
            subject.Roll2.Value.Should().Be(3);
            subject.Roll3.Value.Should().Be(0);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("X3/");
        }

        [Test]
        public void Frm10_StrikeStrikeNumMarks_Valid()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            subject.RecordRoll('X');
            subject.RecordRoll('1');

            // assert
            subject.Roll1.Value.Should().Be(0);
            subject.Roll2.Value.Should().Be(0);
            subject.Roll3.Value.Should().Be(1);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("XX1");
        }

        [Test]
        public void Frm10_StrikeStrikeStrikeMarks_Valid()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            subject.RecordRoll('X');
            subject.RecordRoll('X');

            // assert
            subject.Roll1.Value.Should().Be(0);
            subject.Roll2.Value.Should().Be(0);
            subject.Roll3.Value.Should().Be(0);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("XXX");
        }

        [Test]
        public void Frm10_NumSpareNumMarks_Valid()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('3');
            subject.RecordRoll('/');
            subject.RecordRoll('1');

            // assert
            subject.Roll1.Value.Should().Be(3);
            subject.Roll2.Value.Should().Be(0);
            subject.Roll3.Value.Should().Be(1);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("3/1");
        }

        [Test]
        public void Frm10_NumSpareStrikeMarks_Valid()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('3');
            subject.RecordRoll('/');
            subject.RecordRoll('X');

            // assert
            subject.Roll1.Value.Should().Be(3);
            subject.Roll2.Value.Should().Be(0);
            subject.Roll3.Value.Should().Be(0);
            subject.IsComplete.Should().BeTrue();
            subject.ToString().Should().Be("3/X");
        }

        [Test]
        public void Frm10_SpareAfterStrike_Fails()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            Action test = () => subject.RecordRoll('/');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("spare after a strike");
        }


        [Test]
        public void Frm10_SpareAfterSecondStrike_Fails()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            subject.RecordRoll('X');
            Action test = () => subject.RecordRoll('/');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("spare after a strike");
        }

        [Test]
        public void Frm10_Roll3SpareAfterRoll2Spare_Fails()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('4');
            subject.RecordRoll('/');
            Action test = () => subject.RecordRoll('/');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("spare after a spare");
        }

        [Test]
        public void Frm10_NumFirstStrikeSecond_Fails()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('4');
            Action test = () => subject.RecordRoll('X');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("Strike cannot follow");
        }

        [Test]
        public void Frm10_StrikeNumNumWithPinCountAbove10_Fails()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            subject.RecordRoll('6');
            Action test = () => subject.RecordRoll('6');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("exceed 10");
        }

        [Test]
        public void Frm10_StrikeNumNumWithPinCountEqual10_Fails()
        {
            // arrange
            var subject = new TenthFrame();

            // act
            subject.RecordRoll('X');
            subject.RecordRoll('5');
            Action test = () => subject.RecordRoll('5');

            // assert
            test.Should().Throw<BowlingScoreException>().And.Message.Should().Contain("spare notation");
        }

        [Test]
        public void Frm10_ScoringNumNum_Works()
        {
            // arrange
            var subject = new TenthFrame();
            subject.RecordRoll('6');
            subject.RecordRoll('2');
            subject.IsComplete.Should().BeTrue();
            subject.Score.Should().BeNull();

            // act
            subject.UpdateScore(0, null, null);

            // assert
            subject.Score.Should().Be(8);
        }

        [Test]
        public void Frm10_ScoringSpare_Works()
        {
            // arrange
            var subject = new TenthFrame();
            subject.RecordRoll('6');
            subject.RecordRoll('/');
            subject.RecordRoll('7');
            subject.IsComplete.Should().BeTrue();
            subject.Score.Should().BeNull();

            // act
            subject.UpdateScore(0, null, null);

            // assert
            subject.Score.Should().Be(17);

            // Another edge case...

            // arrange
            subject = new TenthFrame();
            subject.RecordRoll('X');
            subject.RecordRoll('6');
            subject.RecordRoll('/');
            subject.IsComplete.Should().BeTrue();
            subject.Score.Should().BeNull();

            // act
            subject.UpdateScore(0, null, null);

            // assert
            subject.Score.Should().Be(20);
        }

        [Test]
        public void Frm10_ScoringStrike_Works()
        {
            // arrange
            var subject = new TenthFrame();
            subject.RecordRoll('X');
            subject.RecordRoll('X');
            subject.RecordRoll('4');
            subject.Score.Should().BeNull();

            // act
            subject.UpdateScore(0, null, null);

            // assert
            subject.Score.Should().Be(24);
        }
    }
}
