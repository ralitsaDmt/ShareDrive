using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());  //mapping between Web and Business layer objects
                //cfg.AddProfile(new BLProfile());  // mapping between Business and DB layer objects
            });

            return config;
        }
    }
}
