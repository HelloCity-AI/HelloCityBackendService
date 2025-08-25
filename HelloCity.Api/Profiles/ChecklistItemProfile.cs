using AutoMapper;
using HelloCity.Models.Entities;
using HelloCity.Api.DTOs.ChecklistItem;

namespace HelloCity.Api.Profiles
{
  public class ChecklistItemProfile : Profile
  {
    public ChecklistItemProfile()
    {
      CreateMap<CreateChecklistItemDto, ChecklistItem>();
      CreateMap<ChecklistItem, ChecklistItemDto>();
      CreateMap<EditCheckListItemDto, ChecklistItem>();
    }
  }
}
