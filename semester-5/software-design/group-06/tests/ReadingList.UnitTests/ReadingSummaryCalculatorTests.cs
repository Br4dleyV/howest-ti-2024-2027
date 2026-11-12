using ReadingSummary.Api.Models;
using SummaryTitleItem = ReadingSummary.Api.Models.TitleItem;
using ReadingSummary.Api.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReadingList.UnitTests
{
    public class ReadingSummaryCalculatorTests
    {
        [Fact]
        public void Calculate_WithNoTitles_ReturnsZeroCounts()
        {
            // Arrange
            var titles = Array.Empty<SummaryTitleItem>();
            // Act
            var summary = ReadingSummaryCalculator.Calculate(titles);
            // Assert
            Assert.Equal(0, summary.Unread);
            Assert.Equal(0, summary.Finished);
        }

        [Fact]
        public void Calculate_WithOnlyUnreadTitles_ReturnsUnreadCount()
        {
            // Arrange
            var titles = new List<SummaryTitleItem>
            {
                new(Guid.NewGuid(), "Title 1", false),
                new(Guid.NewGuid(), "Title 2", false)
            };
            // Act
            var summary = ReadingSummaryCalculator.Calculate(titles);
            // Assert
            Assert.Equal(2, summary.Unread);
            Assert.Equal(0, summary.Finished);
        }

        [Fact]
        public void Calculate_WithOnlyFinishedTitles_ReturnsFinishedCount()
        {
            // Arrange
            var titles = new List<SummaryTitleItem>
            {
                new(Guid.NewGuid(), "Title 1", true),
                new(Guid.NewGuid(), "Title 2", true)
            };
            // Act
            var summary = ReadingSummaryCalculator.Calculate(titles);
            // Assert
            Assert.Equal(0, summary.Unread);
            Assert.Equal(2, summary.Finished);
        }

        [Fact]
        public void Calculate_WithMixedTitles_ReturnsBothCounts()
        {
            // Arrange
            var titles = new List<SummaryTitleItem>
            {
                new(Guid.NewGuid(), "Title 1", false),
                new(Guid.NewGuid(), "Title 2", true),
                new(Guid.NewGuid(), "Title 3", false),
            };
            // Act
            var summary = ReadingSummaryCalculator.Calculate(titles);
            // Assert
            Assert.Equal(2, summary.Unread);
            Assert.Equal(1, summary.Finished);
        }
    }
}
