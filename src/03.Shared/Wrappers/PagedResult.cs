namespace LibraryManagementSystem.Shared.Wrappers;

public class PagedResult<T>
{
    public bool IsSuccess { get; set; }
    public List<T> Data { get; set; } = new();
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static PagedResult<T> Success(List<T> data, int totalRecords, int pageNumber, int pageSize, string message = "Success")
    {
        return new PagedResult<T>
        {
            IsSuccess = true,
            Data = data,
            TotalRecords = totalRecords,
            PageNumber = pageNumber,
            PageSize = pageSize,
            Message = message
        };
    }

    public static PagedResult<T> Fail(string message, List<string>? errors = null)
    {
        return new PagedResult<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}