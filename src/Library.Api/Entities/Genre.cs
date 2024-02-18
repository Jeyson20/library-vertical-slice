namespace Library.Api.Entities
{
	public class Genre
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string? ParentGenreId { get; set; }
		public ICollection<Genre> InverseParentGenre { get; set; } = new List<Genre>();
		public Genre? ParentGenre { get; set; }
		public ICollection<Book> Books { get; set; } = new List<Book>();
		
		public static Genre Create(string name, string? description)
			=> new()
			{
				Id = Guid.NewGuid().ToString(),
				Name = name,
				Description = description
			};
	}
}
