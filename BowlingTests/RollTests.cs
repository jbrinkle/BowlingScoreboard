using System;
using System.Collections.Generic;
using System.Text;
using Bowling.Scoring;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BowlingTests
{
    [TestFixture]
    public class RollTests
    {
        [Test]
        public void Roll_CreateWithInvalidMark_Fails()
        {
            // arrange

            // act
            Action test = () => Roll.Create(':');

            // assert
            test.Should().Throw<ArgumentException>("Colon not a valid bowling mark");
        }

        [Test]
        public void Roll_ValidMarksTests()
        {
            Roll.IsValidMark('x').Should().BeTrue();
            Roll.IsValidMark('X').Should().BeTrue();
            Roll.IsValidMark('-').Should().BeTrue();
            Roll.IsValidMark('/').Should().BeTrue();
            Roll.IsValidMark('0').Should().BeTrue();
            Roll.IsValidMark('1').Should().BeTrue();
            Roll.IsValidMark('2').Should().BeTrue();
            Roll.IsValidMark('3').Should().BeTrue();
            Roll.IsValidMark('4').Should().BeTrue();
            Roll.IsValidMark('5').Should().BeTrue();
            Roll.IsValidMark('6').Should().BeTrue();
            Roll.IsValidMark('7').Should().BeTrue();
            Roll.IsValidMark('8').Should().BeTrue();
            Roll.IsValidMark('9').Should().BeTrue();

            Roll.IsValidMark('A').Should().BeFalse();
            Roll.IsValidMark('Z').Should().BeFalse();
            Roll.IsValidMark('a').Should().BeFalse();
            Roll.IsValidMark('z').Should().BeFalse();
            Roll.IsValidMark(':').Should().BeFalse();
        }


        [Test]
        public void Roll_StrikeMark_Succeeds()
        {
            // arrange

            // act
            var subject1 = Roll.Create('X');
            var subject2 = Roll.Create('x');

            // assert
            subject1.IsStrike.Should().BeTrue();
            subject1.Value.Should().Be(0);
            subject2.IsStrike.Should().BeTrue();
            subject2.Value.Should().Be(0);
        }

        [Test]
        public void Roll_SpareMark_Succeeds()
        {
            // arrange

            // act
            var subject = Roll.Create('/');

            // assert
            subject.IsSpare.Should().BeTrue();
            subject.Value.Should().Be(0);
        }

        [Test]
        public void Roll_ZeroAndGutterMarks_Equivalent()
        {
            // arrange

            // act
            var subject1 = Roll.Create('0');
            var subject2 = Roll.Create('-');

            // assert
            subject1.IsGutter.Should().BeTrue();
            subject1.Value.Should().Be(0);
            subject2.IsGutter.Should().BeTrue();
            subject2.Value.Should().Be(0);
        }
    }
}
