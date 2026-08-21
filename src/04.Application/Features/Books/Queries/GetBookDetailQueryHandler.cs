using AutoMapper;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Shared.DTOs.Books;
using LibraryManagementSystem.Shared.Wrappers;
using MediatR;

namespace LibraryManagementSystem.Application.Features.Books.Queries;

public class GetBookDetailQuery : IRequest<ApiResponse<BookResponseDto>>
{
    public int Id { get; set; }
}

public class GetBookDetailQueryHandler : IRequestHandler<GetBookDetailQuery, ApiResponse<BookResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBookDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<BookResponseDto>> Handle(GetBookDetailQuery request, CancellationToken cancellationToken)
    {
        var book = await _unitOfWork.Books.GetByIdAsync(request.Id);
        if (book == null) return ApiResponse<BookResponseDto>.Fail("Buku tidak ditemukan.");

        var dto = _mapper.Map<BookResponseDto>(book);
        return ApiResponse<BookResponseDto>.Success(dto);
    }
}