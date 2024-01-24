namespace Library.Api.Entities
{
	public class Publisher
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public ICollection<Book> Books { get; set; } = new List<Book>();
	}
}
