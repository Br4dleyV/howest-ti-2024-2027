namespace ReadingList.Api.Models
{
    // Represents a title item in the reading list.
    public class TitleItem
    {
        // Title ID
        public Guid Id { get; init; }
        // Title of the item
        public string Title { get; set; } = string.Empty;
        // Indicates whether the title has been read
        public bool IsRead { get; set; }

        // Creates a new TitleItem instance with the specified title. (static means it can be called without an instance of the class)
        public static TitleItem Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }
            return new TitleItem
            {
                Id = Guid.NewGuid(),
                Title = title.Trim(),
                IsRead = false
            };
        }

        // Finishes the title item by marking it as read.
        public void Finish()
        {
            if (IsRead)
            {
                throw new InvalidOperationException("Title is already marked as read.");
            }
            IsRead = true;
        }
    }
}
