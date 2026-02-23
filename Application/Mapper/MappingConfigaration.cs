using Application.DTOs.CategoryDTOs;
using Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mapper
{
    public  static class MappingConfigaration
    {

        public  static  void Config() {

            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);


            TypeAdapterConfig<Category, CreateCategoryDto>
               .NewConfig()
               .Map(dest => dest.CategoryName, src => src.CategoryName)
               .Map(dest => dest.Description, src => src.CategoryDescription);

            TypeAdapterConfig<Category, UpdateCategoryDto>
                .NewConfig()
                .Map(dest => dest.Id, src => src.CategoryID)
                .Map(dest => dest.CategoryName, src => src.CategoryName)
                .Map(dest => dest.Description, src => src.CategoryDescription);

                TypeAdapterConfig<Category, GetCategoryDto>
                .NewConfig()
                .Map(dest => dest.CategoryName, src => src.CategoryName)
                .Map(dest => dest.Description, src => src.CategoryDescription);


        }

    }
}
