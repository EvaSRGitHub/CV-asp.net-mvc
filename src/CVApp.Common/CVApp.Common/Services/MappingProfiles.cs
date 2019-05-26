using AutoMapper;
using CVApp.Models;
using CVApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CVApp.Common.Services
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            //CreateMap<CVAppUser, PersonalInfoViewModel>()
            //    .ForMember(x => x.FirstName, y => y.MapFrom(x => x.FirstName))
            //    .ForMember(x => x.LastName, y => y.MapFrom(x => x.LastName))
            //    .ForMember(x => x.PhoneNumber, y => y.MapFrom(x => x.PhoneNumber))
            //    .ForMember(x => x.PictureFile.ToString(), y => y.MapFrom(x => x.Picture))
            //    .ForMember(x => x.ProjectsUrl, y => y.MapFrom(x => x.GitHubProfile))
            //    .;
        }
    }
}
