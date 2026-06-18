using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ComprasVentas;

public class ProductoService(AppDbContext appDbContext, IMapper mapper, IFileService fileService) : IProductoService
{

    private readonly AppDbContext _context = appDbContext;

    private readonly IMapper _mapper = mapper;

    private readonly IFileService _fileService = fileService;

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

        return PageResultDto<ProductoDto>.Build(
            _mapper.Map<IEnumerable<ProductoDto>>(items),
            TotalCount,
            productoFilterDto.Page,
            productoFilterDto.Size
        );
    }

    public async Task<ProductoDto> CreateProductoAsync(CreateProductoDto createProductoDto)
    {
        var producto = new Producto
        {
            Nombre = createProductoDto.Nombre,
            Descripcion = createProductoDto.Descripcion,
            PrecioVentaActual = createProductoDto.PrecioVentaActual,
            Marca = createProductoDto.Marca,
            Categoria = _context.Categorias.Find(createProductoDto.CategoriaId)
        };

        if(createProductoDto.Imagen != null)
        {
            var imagePath = await _fileService.SaveFileAsync(createProductoDto.Imagen);
            producto.Imagen = imagePath;
        }

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        var createdProducto = await _context.Productos.Include(p=>p.Categoria).FirstOrDefaultAsync(p=>p.Id == producto.Id);
        return _mapper.Map<ProductoDto>(createdProducto);
    }

    public async Task<List<ProductoDto>> FindAllProductosByAlmacen(int almacenId)
    {
        var productos = await _context.AlmacenProductos
            .Where(ap => ap.Almacen.Id == almacenId)
            .Include(ap => ap.Producto)
            .ThenInclude(p=>p.Categoria)
            .Select(ap => ap.Producto)
            .ToListAsync();
        return productos.Select(p=>_mapper.Map<ProductoDto>(p)).ToList();
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
