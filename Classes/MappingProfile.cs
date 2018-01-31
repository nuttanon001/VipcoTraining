using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using VipcoTraining.Models;
using VipcoTraining.ViewModels;

using AutoMapper;

namespace VipcoTraining.Classes
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //TblBasicCourse
            CreateMap<TblBasicCourse, BasicCourseViewModel>();
            CreateMap<BasicCourseViewModel, TblBasicCourse>();
            //TblEmployee
            CreateMap<TblEmployee, EmployeeViewModel>()
                //.ForMember(x => x.DepartmentString, obt => obt.MapFrom(src => src.DeptCodeNavigation.DeptName ?? "-"))
                //.ForMember(x => x.DeptCodeNavigation, obt => obt.Ignore())
                .ForMember(x => x.DivisionString, obt => obt.MapFrom(src => src.DivisionCodeNavigation.DivisionName ?? "-"))
                .ForMember(x => x.DivisionCodeNavigation, obt => obt.Ignore())
                //.ForMember(x => x.EmployeeTypeString, obt => obt.MapFrom(src => src.EmptypeCodeNavigation.EmpTypeDesc ?? "-"))
                //.ForMember(x => x.EmptypeCodeNavigation, obt => obt.Ignore())
                //.ForMember(x => x.GroupString, obt => obt.MapFrom(src => src.GroupCodeNavigation.GroupDesc ?? "-"))
                //.ForMember(x => x.GroupCodeNavigation, obt => obt.Ignore())
                .ForMember(x => x.SectionString, obt => obt.MapFrom(src => src.SectionCodeNavigation.SectionName ?? "-"))
                .ForMember(x => x.SectionCodeNavigation, obt => obt.Ignore())
                //.ForMember(x => x.LocationString, obt => obt.MapFrom(src => src.Locate.LocateDesc ?? "-"))
                //.ForMember(x => x.Locate, obt => obt.Ignore())
                .ForMember(x => x.EmpPict, obt => obt.Ignore());
                //.ForMember(x => x.EmployeePicture, obt => obt.MapFrom(src => src.EmpPict != null ? "data:image/png;base64," + System.Convert.ToBase64String(src.EmpPict) : ""));
            CreateMap<EmployeeViewModel, TblEmployee>();
            //TblTrainingType
            CreateMap<TblTrainingType, TrainingTypeViewModel>()
                .ForMember(x => x.TrainingTypeParentString, obt => obt.MapFrom(src => src.TrainingTypeParent.TrainingTypeName ?? "-"));
            CreateMap<TrainingTypeViewModel, TblTrainingType>();
            //TblTrainingCourse
            CreateMap<TblTrainingCousre, TrainingCourseViewModel>()
                .ForMember(x => x.EducationRequirementString, obt => obt.MapFrom(src => src.EducationRequirement == null ? "ไม่ระบุ" : src.EducationRequirement.EducationName))
                .ForMember(x => x.TrainingLevelString, obt => obt.MapFrom(src => src.TrainingLevel == null ? "ไม่ระบุ" : src.TrainingLevel.TrainingLevel))
                .ForMember(x => x.TrainingTypeString, obt => obt.MapFrom(src => src.TrainingType == null ? "ไม่ระบุ" : src.TrainingType.TrainingTypeName))
                .ForMember(x => x.EducationRequirement, obt => obt.Ignore())
                .ForMember(x => x.TrainingLevel, obt => obt.Ignore())
                .ForMember(x => x.TrainingType, obt => obt.Ignore());
            CreateMap<TrainingCourseViewModel, TblTrainingCousre>();
            //TblTrainingMaster
            CreateMap<TblTrainingMaster, TrainingMasterViewModel>()
                .ForMember(x => x.TrainingDateString, obt => obt.MapFrom(src => src.TrainingDate.Value.ToString("dd/MM/yy")))
                .ForMember(x => x.TrainingDateTime, obt => obt.MapFrom(src => src.TrainingDate.Value.ToString("HH:mm")))
                .ForMember(x => x.TrainingDateEndString, obt => obt.MapFrom(src => src.TrainingDateEnd != null ? src.TrainingDateEnd.Value.ToString("dd/MM/yy") : "-"))
                .ForMember(x => x.TrainingDateEndTime, obt => obt.MapFrom(src => src.TrainingDateEnd != null ? src.TrainingDateEnd.Value.ToString("HH:mm") : "-"))
                //Place
                .ForMember(x => x.PlaceId,
                obt => obt.MapFrom(src => src.TblTrainingMasterHasPlace == null ? 0 : src.TblTrainingMasterHasPlace.FirstOrDefault().PlaceId))
                .ForMember(x => x.PlaceName,
                obt => obt.MapFrom(src => src.TblTrainingMasterHasPlace == null ? "ไม่ระบุ" : src.TblTrainingMasterHasPlace.FirstOrDefault().Place.PlaceName))
                .ForMember(x => x.TblTrainingMasterHasPlace, obt => obt.Ignore())
                //Detail
                .ForMember(x => x.TblTrainingDetail, obt => obt.Ignore());
            CreateMap<TrainingMasterViewModel, TblTrainingMaster>();
            //TblTrainingDetail
            CreateMap<TblTrainingDetail, TrainingDetailViewModel>()
                .ForMember(x => x.EmployeeTrainingNavigation, obt => obt.Ignore())
                .ForMember(x => x.EmployeeNameString, obt => obt.MapFrom(src => src.EmployeeTrainingNavigation == null ? "ไม่ระบุ" : src.EmployeeTrainingNavigation.NameThai))
                .ForMember(x => x.StatusForTrainingString, obt => obt.MapFrom(src => src.StatusForTraining == null ? "รอคะแนน" : (src.StatusForTraining == 1 ? "ผ่าน" : (src.StatusForTraining == 2 ? "ไม่ผ่าน" : "ไม่ระบุ"))));
            CreateMap<TrainingDetailViewModel, TblTrainingDetail>();
            //TblBasicCourse
            CreateMap<TblBasicCourse, BasicCourseViewModel>()
                .ForMember(x => x.TrainingCousre, obt => obt.Ignore())
                .ForMember(x => x.CourseString, obt => obt.MapFrom(src => src.TrainingCousre.TrainingCousreName));

            //TblStandardCourse
            CreateMap<TblStandardCourse, StandardCourseViewModel>()
                .ForMember(x => x.TrainingCousre, obt => obt.Ignore())
                .ForMember(x => x.CourseString, obt => obt.MapFrom(src => src.TrainingCousre.TrainingCousreName));

            //TblSupplementCourse
            CreateMap<TblSupplementCourse, SupplementCourseViewModel>()
                .ForMember(x => x.TrainingCousre, obt => obt.Ignore())
                .ForMember(x => x.CourseString, obt => obt.MapFrom(src => src.TrainingCousre.TrainingCousreName));

            //TblTrainingProgramHasPosition
            CreateMap<TblTrainingProgramHasPosition, ProgramHasPositionViewModel>()
                .ForMember(x => x.PositionCodeNavigation, obt => obt.Ignore())
                .ForMember(x => x.PositionString, obt => obt.MapFrom(src => src.PositionCodeNavigation.PositionName));

            //TblTrainingProgramHasGroup
            CreateMap<TblTrainingProgramHasGroup, ProgramHasGroupViewModel>()
                .ForMember(x => x.GroupCodeNavigation, o => o.Ignore())
                .ForMember(x => x.GroupString, o => o.MapFrom(s => s.GroupCodeNavigation.GroupDesc));

            //TblTrainingMasterHasPlace
            CreateMap<TblTrainingMasterHasPlace, TrainingMasterHasPlaceViewModel>()
                .ForMember(x => x.Place, o => o.Ignore())
                .ForMember(x => x.PlaceString, o => o.MapFrom(s => s.Place.PlaceName));
        }
    }
}
