using AutoMapper;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Shared.DTOs.Books;
using LibraryManagementSystem.Shared.DTOs.BorrowRequests;

namespace LibraryManagementSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookResponseDto>()
            .ForMember(dest => dest.StockStatus,
                       opt => opt.MapFrom(src => src.Stock > 0 ? "Tersedia" : "Habis"));

        CreateMap<BorrowRequest, BorrowRequestResponseDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book!.Title)) 
            .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src => src.User!.Name)) 
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.ApprovedByAdminName, opt => opt.MapFrom(src => src.ApprovedBy != null ? src.ApprovedBy.Name : null));
    }
}