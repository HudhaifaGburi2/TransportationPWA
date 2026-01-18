using AutoMapper;
using TransportationAttendance.Application.DTOs.Department;
using TransportationAttendance.Application.DTOs.District;
using TransportationAttendance.Application.DTOs.Driver;
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
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Periods, opt => opt.MapFrom(src => DeserializePeriods(src.Periods)));

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

        // Driver mappings
        CreateMap<Driver, DriverDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.NameAr : null));

        // Department mappings
        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.DriversCount, opt => opt.MapFrom(src => src.Drivers.Count))
            .ForMember(dest => dest.BusesCount, opt => opt.MapFrom(src => src.Buses.Count));
    }

    private static List<string>? DeserializePeriods(string? periodsJson)
    {
        if (string.IsNullOrEmpty(periodsJson))
            return null;
        
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<List<string>>(periodsJson);
        }
        catch
        {
            return null;
        }
    }
}
