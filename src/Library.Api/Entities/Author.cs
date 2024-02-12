namespace Library.Api.Entities
{
	public class Author
	{
		private Author()
		{

		}

		public string Id { get; private set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public DateTime DateOfBirth { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public ICollection<Book> Books { get; set; } = new List<Book>();

		public static Author Create(string firstName, string lastName, DateTime dateOfBirth)
			=> new()
			{
				Id = Guid.NewGuid().ToString(),
				FirstName = firstName,
				LastName = lastName,
				DateOfBirth = dateOfBirth,
			};

	}
}

