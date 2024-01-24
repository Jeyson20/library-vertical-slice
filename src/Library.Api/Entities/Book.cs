namespace Library.Api.Entities
{
	public class Book
	{
		public string Id { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;
		public DateOnly? PublishedDate { get; set; }
		public string PublisherId { get; set; } = string.Empty;
		public int? TotalPages { get; set; }
		public decimal? Rating { get; set; }
		public Publisher Publisher { get; set; } = null!;
		public ICollection<Author> Authors { get; set; } = new List<Author>();
		public ICollection<Genre> Genres { get; set; } = new List<Genre>();
	}
}
