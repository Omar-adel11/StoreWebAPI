using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Shared.DTOs.Identity;

namespace Services.Mapping.Identity
{
    public class AuthProfile:Profile
    {
        public AuthProfile()
        {
            CreateMap<Address,AddressDto>().ReverseMap();
        }
    }
}
