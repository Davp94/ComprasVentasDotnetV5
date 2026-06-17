using System;
using AutoMapper;

namespace ComprasVentas;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Producto, ProductoDto>();
    }
}
