namespace Library.Api.DTOs.Responses
{
	public record AuthorResponse
	{
		public string Id { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public DateTime DateOfBirth { get; set; }
	}
}
