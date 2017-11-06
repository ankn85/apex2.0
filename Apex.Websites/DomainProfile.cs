using Apex.Data.Entities.Logs;
using Apex.Websites.ViewModels.Logs;
using AutoMapper;

namespace Apex.Websites
{
    public sealed class DomainProfile : Profile
    {
        public DomainProfile()
        {
            MapLog();
        }

        private void MapLog()
        {
            CreateMap<UpdateActivityLogTypeViewModel, ActivityLogType>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name.Trim()))
                .ForMember(d => d.SystemKeyword, opt => opt.Ignore())
                .ForMember(d => d.ActivityLogs, opt => opt.Ignore());
                
            CreateMap<ActivityLogType, UpdateActivityLogTypeViewModel>();
        }
    }
}
