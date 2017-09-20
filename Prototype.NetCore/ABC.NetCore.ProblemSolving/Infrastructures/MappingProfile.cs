using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using ABC.NetCore.Models;
using ABC.NetCore.ProblemSolving.Models;

namespace ABC.NetCore.ProblemSolving.Infrastructures
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerEntity, Customer>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(Controllers.CustomersController.GetCustomerByIdAsync), new { Id = src.Id })));
                // TODO:: HETEOS REST Approach
                // Bellow link can be used to attach helper form object to each object for HETEOS approach with REST API
                // Commented out as it adds too much complexity with every REST API Response.
                //.ForMember(dest => dest.Create, opt => opt.MapFrom(src => FormMetadata.FromModel(new CustomerRequest(),
                //        Link.ToForm(nameof(Controllers.CustomersController.CreateCustomerAsync), null, Link.PostMethod, Form.CreateRelation))));

            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerEntity>();
            

            CreateMap<SAPPartEntity, SAPPart>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(Controllers.SAPController.GetSAPPartsAsync), new { SAPMaterialNum = src.SAPMaterialNum })));
            CreateMap<SAPPart, SAPPartEntity>();

            CreateMap<SAPEmployeeEntity, SAPEmployee>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(Controllers.SAPController.GetSAPEmployeesAsync), new { UserName = src.UserName })));
            CreateMap<SAPEmployee, SAPEmployeeEntity>();

            CreateMap<ComplaintCodeEntity, ComplaintCode>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(Controllers.ManagementController.GetComplaintCodesAsync), new { Code = src.Code })));
            CreateMap<ComplaintCode, ComplaintCodeEntity>();

            CreateMap<DepartmentEntity, Department>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(Controllers.SAPController.GetSAPPartsAsync), new { SAPMaterialNum = src.SAPMaterialNum })));
            CreateMap<Department, DepartmentEntity>();

            CreateMap<PlantEntity, Plant>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(Controllers.SAPController.GetSAPPartsAsync), new { SAPMaterialNum = src.SAPMaterialNum })));
            CreateMap<Plant, PlantEntity>();

            CreateMap<ProblemSolvingTypeEntity, ProblemSolvingType>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src => Link.To(nameof(Controllers.SAPController.GetSAPPartsAsync), new { SAPMaterialNum = src.SAPMaterialNum })));
            CreateMap<ProblemSolvingType, ProblemSolvingTypeEntity>();
        }
    }
}
