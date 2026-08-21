using AutoMapper;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Shared.DTOs.BorrowRequests;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.BorrowRequests.Queries;

public class GetMyBorrowRequestsQuery : IRequest<ApiResponse<List<BorrowRequestResponseDto>>>
{
    public int UserId { get; set; }
}

public class GetMyBorrowRequestsQueryHandler : IRequestHandler<GetMyBorrowRequestsQuery, ApiResponse<List<BorrowRequestResponseDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetMyBorrowRequestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<BorrowRequestResponseDto>>> Handle(GetMyBorrowRequestsQuery request, CancellationToken cancellationToken)
    {
        var data = await _unitOfWork.BorrowRequests.GetUserRequestsAsync(request.UserId);
        var dtoList = _mapper.Map<List<BorrowRequestResponseDto>>(data);

        return ApiResponse<List<BorrowRequestResponseDto>>.Success(dtoList);
    }
}