namespace Library.Api.DTOs.Requests;

public record CreateAuthorRequest(string FirstName, string LastName, DateTime DateOfBirth);