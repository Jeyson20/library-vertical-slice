namespace Library.Api.Shared.Models;

public class Paginated<TData>(ICollection<TData> items, int totalCount, int pageNumber, int pageSize)
{
	public ICollection<TData> Items { get; } = items;
	public int PageNumber { get; } = pageNumber;
	public int PageSize { get; } = pageSize;
	public int TotalPages { get; } = (int)Math.Ceiling(totalCount / (double)pageSize); 
	public int TotalCount { get; } = totalCount;
	public bool HasPreviousPage => PageNumber > 1;
	public bool HasNextPage => PageNumber < TotalPages;
}
