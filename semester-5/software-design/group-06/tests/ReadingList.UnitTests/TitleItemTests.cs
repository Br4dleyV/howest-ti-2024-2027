using ReadingList.Api.Models;

namespace ReadingList.UnitTests;

public class TitleItemTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyTitle_ThrowsArgumentException(string? title)
    {
        Assert.Throws<ArgumentException>(() => TitleItem.Create(title!));
    }

    [Fact]
    public void Create_WithValidTitle_CreatesUnreadTitle()
    {
        // Arrange
        string title = "   Spider-Man: Brand New Day   ";

        // Act
        TitleItem item = TitleItem.Create(title);

        // Assert
        Assert.Equal("Spider-Man: Brand New Day", item.Title);
        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.False(item.IsRead);
    }

    [Fact]
    public void Finish_WithUnreadTitle_MarksTitleAsRead()
    {
        // Arrange
        TitleItem item = TitleItem.Create("Spider-Man: No Way Home");
        // Act
        item.Finish();
        // Assert
        Assert.True(item.IsRead);
    }

    [Fact]
    public void Finish_WithFinishedTitle_ThrowsInvalidOperationException()
    {
        // Arrange
        TitleItem item = TitleItem.Create("Spider-Man: No Way Home");
        item.Finish();
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => item.Finish());
    }
}
