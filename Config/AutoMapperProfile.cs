using System;
using AutoMapper;

namespace ComprasVentas;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Producto, ProductoDto>();
        CreateMap<Nota, NotaResponseDto>()
            .ForMember(dest=>dest.UsuarioNombre, opt=>opt.MapFrom(s => s.Usuario.Nombre)).ForMember(dest=>dest.ClienteProveedorRazonSocial, opt=> opt.MapFrom(s=>s.ClienteProveedor.RazonSocial));
        CreateMap<Movimiento, MovimientoResponseDto>()
            .ForMember(dest=>dest.ProductoNombre, opt=>opt.MapFrom(s => s.Producto.Nombre)).ForMember(dest=>dest.AlmacenNombre, opt=> opt.MapFrom(s=>s.Almacen.Nombre));
    }
}
