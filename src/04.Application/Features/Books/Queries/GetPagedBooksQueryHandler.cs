using AutoMapper;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Shared.DTOs.Books;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Books.Queries;
public class GetPagedBooksQuery : PagedRequest, IRequest<PagedResult<BookResponseDto>>
{
}

public class GetPagedBooksQueryHandler : IRequestHandler<GetPagedBooksQuery, PagedResult<BookResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPagedBooksQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<BookResponseDto>> Handle(GetPagedBooksQuery request, CancellationToken cancellationToken)
    {
        var (data, total) = await _unitOfWork.Books.GetPagedBooksAsync(request);

        var dtoList = _mapper.Map<List<BookResponseDto>>(data);

        return PagedResult<BookResponseDto>.Success(dtoList, total, request.PageNumber, request.PageSize);
    }
}