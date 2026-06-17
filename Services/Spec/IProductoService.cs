using System;

namespace ComprasVentas;

public interface IProductoService
{
    Task<PageResultDto<ProductoDto>> GetProductosAsync(ProductoFilterDto productoFilterDto);

    Task<ProductoDto> CreateProductoAsync(CreateProductoDto createProductoDto);

    Task<List<ProductoDto>> FindAllProductosByAlmacen(int almacenId);
}
