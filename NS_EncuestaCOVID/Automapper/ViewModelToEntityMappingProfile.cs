using AutoMapper;
using NS_COVID_Entities;
using NS_EncuestaCOVID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Automapper
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public override string ProfileName {
            get { return "ViewModelToEntityMapping"; }
        }

        public ViewModelToEntityMappingProfile()
        {
            CreateMap<PersonaVM, Persona>();
        }
        }
}