using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class ProductoAppService : IProductoAppService
{
    private readonly IProductoRepository productoRepository;
    private readonly IMarcaRepository marcaRepository;
    private readonly ITipoProductoRepository tipoProductoRepository;

    public ProductoAppService(IProductoRepository productoRepository,
            IMarcaRepository marcaRepository,
            ITipoProductoRepository tipoProductoRepository)
    {
        this.productoRepository = productoRepository;
        this.marcaRepository = marcaRepository;
        this.tipoProductoRepository = tipoProductoRepository;
    }

    public async Task<ProductoDto> CreateAsync(ProductoCrearActualizarDto productoDto)
    {

        //Mapeo Dto => Entidad
        var producto = new Producto();
        producto.Caducidad = productoDto.Caducidad;
        producto.MarcaId = productoDto.MarcaId;
        producto.Nombre = productoDto.Nombre;
        producto.Observaciones = productoDto.Observaciones;
        producto.Precio = productoDto.Precio;
        producto.TipoProductoId = productoDto.TipoProductoId;

        //Persistencia objeto
        producto = await productoRepository.AddAsync(producto);
        await productoRepository.UnitOfWork.SaveChangesAsync();

        return await GetByIdAsync(producto.Id);
    }

    public async Task<bool> DeleteAsync(int marcaId)
    {
        var producto = await productoRepository.GetByIdAsync(marcaId);
        productoRepository.Delete(producto);
        await productoRepository.UnitOfWork.SaveChangesAsync();
        return true;
    }

    public ListaPaginada<ProductoDto> GetAll(int limit = 10, int offset = 0)
    {
        //Lista.
        //Opcion 1 
        var consulta = productoRepository.GetAllIncluding(x => x.Marca,
                            x => x.TipoProducto);


        //Opcion 2. Se puede utilizar join, cuando no tiene propiedades de navegacion
        //en sus objetos.       
        var consultaJoin = from p in productoRepository.GetAll()
                           join m in marcaRepository.GetAll()
                           on p.MarcaId equals m.Id
                           join tp in tipoProductoRepository.GetAll()
                           on p.TipoProductoId equals tp.Id
                           select p
                           ;

        //Opcion 3. Utilizar Include. No, para evitar acoplamiento con Entity Framework
        //en la capa de aplicacion, se asume que tenemos repositorios, que permite tener
        //una abstraccion de entity framework.



        //1. Ejecuatar linq. Total registros
        var total = consulta.Count();
        //var total = productoRepository.GetAll().Count();

        //Opcion 2.
        var totalOpcion2 = consultaJoin.Count();

        //2. Obtener el listado paginado..
        var consulaListaProductosDto = consulta.Skip(offset)
                                .Take(limit)
                                .Select(
                                    x => new ProductoDto()
                                    {
                                        Id = x.Id,
                                        Caducidad = x.Caducidad,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        Marca = x.Marca.Nombre,
                                        MarcaId = x.MarcaId,
                                        Nombre = x.Nombre,
                                        Observaciones = x.Observaciones,
                                        Precio = x.Precio,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        TipoProducto = x.TipoProducto.Nombre,
                                        TipoProductoId = x.TipoProductoId
                                    }
                                );

        //Opcion 2. 
        var consulaListaProductosJoinDto = consultaJoin.Skip(offset)
                                .Take(limit)
                                .Select(
                                    x => new ProductoDto()
                                    {
                                        Id = x.Id,
                                        Caducidad = x.Caducidad,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        Marca = x.Marca.Nombre,
                                        MarcaId = x.MarcaId,
                                        Nombre = x.Nombre,
                                        Observaciones = x.Observaciones,
                                        Precio = x.Precio,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        TipoProducto = x.TipoProducto.Nombre,
                                        TipoProductoId = x.TipoProductoId
                                    }
                                );



        var resultado = new ListaPaginada<ProductoDto>();
        resultado.Total = total;
        resultado.Lista = consulaListaProductosDto.ToList();

        return resultado;

    }


    public async Task<ListaPaginada<ProductoDto>> GetListAsync(ProductoListInput input){

        var consulta = productoRepository.GetAllIncluding(x => x.Marca,
                            x => x.TipoProducto);
  
        //Aplicar filtros
        if (input.TipoProductoId.HasValue){
          consulta = consulta.Where(x => x.TipoProductoId == input.TipoProductoId);
        }

        if (input.MarcaId.HasValue){
          consulta = consulta.Where(x => x.MarcaId == input.MarcaId);
        }

        if (!string.IsNullOrEmpty(input.ValorBuscar)){

            //consulta = consulta.Where(x => x.Nombre.Contains(input.ValorBuscar) ||
            //    x.Codigo.StartsWith(input.ValorBuscar));
            consulta = consulta.Where(x => x.Nombre.Contains(input.ValorBuscar));
        }

        //Ejecuatar linq. Total registros
        var total = consulta.Count();
     
        //Aplicar paginacion
        consulta = consulta.Skip(input.Offset)
                    .Take(input.Limit);
       
        //Obtener el listado paginado. (Proyeccion)
        var consulaListaProductosDto = consulta
                                .Select(
                                    x => new ProductoDto()
                                    {
                                        Id = x.Id,
                                        Caducidad = x.Caducidad,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        Marca = x.Marca.Nombre,
                                        MarcaId = x.MarcaId,
                                        Nombre = x.Nombre,
                                        Observaciones = x.Observaciones,
                                        Precio = x.Precio,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        TipoProducto = x.TipoProducto.Nombre,
                                        TipoProductoId = x.TipoProductoId
                                    }
                                );

       
        var resultado = new ListaPaginada<ProductoDto>();
        resultado.Total = total;
        resultado.Lista = consulaListaProductosDto.ToList();

        return resultado;

    }

    public Task<ProductoDto> GetByIdAsync(int id)
    {
        var consulta = productoRepository.GetAllIncluding(x => x.TipoProducto, x => x.Marca);
        consulta = consulta.Where(x => x.Id == id);

        var consultaProductoDto = consulta
                                .Select(
                                    x => new ProductoDto()
                                    {
                                        Id = x.Id,
                                        Caducidad = x.Caducidad,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        Marca = x.Marca.Nombre,
                                        MarcaId = x.MarcaId,
                                        Nombre = x.Nombre,
                                        Observaciones = x.Observaciones,
                                        Precio = x.Precio,
                                        //Utilizar propiedad navegacion,
                                        // para obtener informacion de una clase relacionada
                                        TipoProducto = x.TipoProducto.Nombre,
                                        TipoProductoId = x.TipoProductoId
                                    }
                                );

        return Task.FromResult(consultaProductoDto.SingleOrDefault());
    }

    public async Task UpdateAsync(int id, ProductoCrearActualizarDto productoDto)
    {
        var producto = await productoRepository.GetByIdAsync(id);
        producto.Nombre = productoDto.Nombre;
        producto.Precio = productoDto.Precio;
        producto.Observaciones = productoDto.Observaciones;
        producto.Caducidad = productoDto.Caducidad;
        producto.MarcaId = productoDto.MarcaId;
        producto.TipoProductoId = productoDto.TipoProductoId;
        await productoRepository.UpdateAsync(producto);
        await productoRepository.UnitOfWork.SaveChangesAsync();
        return;
    }
}

