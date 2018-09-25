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
    public class GameTests
    {
        [Test]
        public void Game_CreatesWithTenFrames()
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
    }
}
