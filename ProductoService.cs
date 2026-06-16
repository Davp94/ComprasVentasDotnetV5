using System;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class ProductoService(AppDbContext appDbContext) : IProductoService
{

    private readonly AppDbContext _context = appDbContext;

    public async Task<PageResultDto<ProductoDto>> GetProductosAsync(ProductoFilterDto productoFilterDto)
    {
        var query = _context.Productos.Include(p=>p.Categoria).AsQueryable();

        //global filter
        if(!string.IsNullOrEmpty(productoFilterDto.filterValue))
        {
           var filterValue = productoFilterDto.filterValue.ToLower();
           query = query.Where(p=>
                p.Nombre.ToLower().Contains(filterValue) ||
                p.Descripcion.ToLower().Contains(filterValue) || 
                p.Categoria.Nombre.ToLower().Contains(filterValue) ||
                p.Marca.ToLower().Contains(filterValue) 
            ); 
        }
        //specific filter
         if(!string.IsNullOrEmpty(productoFilterDto.Nombre))
        {
             query = query.Where(p=>
                p.Nombre.ToLower().Contains(productoFilterDto.Nombre.ToLower())); 
        }
        if(!string.IsNullOrEmpty(productoFilterDto.Descripcion))
        {
             query = query.Where(p=>
                p.Descripcion.ToLower().Contains(productoFilterDto.Descripcion.ToLower())); 
        }
        if(!string.IsNullOrEmpty(productoFilterDto.NombreCategoria))
        {
             query = query.Where(p=>
                 p.Categoria.Nombre.ToLower().Contains(productoFilterDto.NombreCategoria.ToLower())); 
        }
        //Sorting
        if(!string.IsNullOrEmpty(productoFilterDto.SortField))
        {
            var sortField = productoFilterDto.SortField.ToLower(); 
            var sortOrder = productoFilterDto.SortOrder.ToLower() == "asc" ? "ascending" : "descending"; 
            query = ApplySorting(query, productoFilterDto.SortField, productoFilterDto.SortOrder);
        }

        //add pagination
        var TotalCount = await query.CountAsync();

        var items = await query.Skip((productoFilterDto.Page -1 ) * productoFilterDto.Size)
        .Take(productoFilterDto.Size).ToListAsync();

        return new PageResultDto<ProductoDto>
        {
            Items = items,
            TotalCount = TotalCount,
            Page = productoFilterDto.Page,
            Size = productoFilterDto.Size
        };

    }

    public Task<ProductoDto> CreateProductoAsync(CreateProductoDto createProductoDto)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductoDto>> FindAllProductosByAlmacen(int almacenId)
    {
        throw new NotImplementedException();
    }

    private IQueryable<Producto> ApplySorting(IQueryable<Producto> query, string sortField, string sortOrder)
    {
        if(sortField.Equals("NombreCategoria"))
            return sortField == "ascending" ? query.OrderBy(p=>p.Categoria != null ? p.Categoria.Nombre : null) : query.OrderByDescending(p=>p.Categoria != null ? p.Categoria.Nombre : null);
        

        switch (sortField)
        {
            case "nombre":
                 return sortField == "ascending" ? query.OrderBy(p=>p.Nombre) : query.OrderByDescending(p=>p.Nombre);
            case "descripcion":
                return sortField == "ascending" ? query.OrderBy(p=>p.Descripcion) : query.OrderByDescending(p=>p.Descripcion);
            case "id":   
                return sortField == "ascending" ? query.OrderBy(p=>p.Id) : query.OrderByDescending(p=>p.Id);
        }       
        return query;
    }
}
