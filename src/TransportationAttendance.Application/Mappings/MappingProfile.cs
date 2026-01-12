using AutoMapper;
using TransportationAttendance.Application.DTOs.District;
using TransportationAttendance.Application.DTOs.Location;
using TransportationAttendance.Application.DTOs.Lookups;
using TransportationAttendance.Application.DTOs.Registration;
using TransportationAttendance.Application.DTOs.Student;
using TransportationAttendance.Domain.Entities;
using TransportationAttendance.Domain.Entities.Central;

namespace TransportationAttendance.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // District mappings
        CreateMap<District, DistrictDto>();
        CreateMap<CreateDistrictDto, District>()
            .ConstructUsing(src => District.Create(src.DistrictNameAr, src.DistrictNameEn));

        // Location mappings
        CreateMap<Location, LocationDto>();
        CreateMap<CreateLocationDto, Location>()
            .ConstructUsing(src => Location.Create(src.LocationCode, src.LocationName, src.LocationType));

        // Student mappings
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // Registration Request mappings
        CreateMap<RegistrationRequest, RegistrationRequestDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        // Central DB Lookups mappings
        CreateMap<SetPeriod, PeriodDto>()
            .ForMember(dest => dest.PeriodName, opt => opt.MapFrom(src => src.PeriodDesc));

        CreateMap<SetAgeGroup, AgeGroupDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AgName))
            .ForMember(dest => dest.MinAge, opt => opt.MapFrom(src => src.MinAgeLimit))
            .ForMember(dest => dest.MaxAge, opt => opt.MapFrom(src => src.MaxAgeLimit));

        CreateMap<HalaqatLocation, HalaqaLocationDto>();

        // Student Halaqa Info mapping
        CreateMap<StudentHalaqaInfo, StudentHalaqaInfoDto>();
    }
}
