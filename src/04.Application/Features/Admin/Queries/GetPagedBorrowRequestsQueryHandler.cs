using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Shared.DTOs.Admin;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Admin.Queries;
public class GetPagedBorrowRequestsQueryHandler : IRequestHandler<GetPagedBorrowRequestsQuery, PagedResult<BorrowRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPagedBorrowRequestsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<BorrowRequestDto>> Handle(GetPagedBorrowRequestsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalRecords) = await _unitOfWork.BorrowRepository.GetPagedBorrowRequestsAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            request.Status,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        return PagedResult<BorrowRequestDto>.Success(
            data: items.ToList(),
            totalRecords: totalRecords,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize
        );
    }
}