using AutoMapper;
using NS_COVID_Entities;
using NS_EncuestaCOVID.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NS_EncuestaCOVID.Automapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<ViewModelToEntityMappingProfile>();
            });


        }
    }
            
}