using Curso.ComercioElectronico.Domain;
using Microsoft.Extensions.Logging;

namespace Curso.ComercioElectronico.Application;

public class OrdenAppService : IOrdenAppService
{
    private readonly IOrdenRepository ordenRepository;
    private readonly IProductoAppService productoAppService;
    private readonly ILogger<OrdenAppService> logger;

    public OrdenAppService(
        IOrdenRepository ordenRepository,
        //IProductoRepository productoRepository,
        IProductoAppService productoAppService,
        ILogger<OrdenAppService> logger )
    {
        this.ordenRepository = ordenRepository;
        this.productoAppService = productoAppService;
        this.logger = logger;
    }

    public async Task<OrdenDto> CreateAsync(OrdenCrearDto ordenDto)
    {
        logger.LogInformation("Crear Orden");

        

        //Crear una orden... 
        //1. Validaciones...
        //1.1. Stock.  
        //1.2. Restricciones Cliente. (Posee deudas, 
        //Disponibilidad del producto en la localizacion del cliente )
        //Reglas Negocio.
        //Si eres clientes nuevo, te aplico un descuento 10%
        //Si eres clientes frecuente, te aplico un descuento 25%. (Ejercicio)
        //-Siempre y cuando en los ultimos 3 meses tenga compras 
        //Ciertos productos, se puede establecer descuento segun la cantidad de productos comprados
        //2. Mapeos
        var orden = new Orden(Guid.NewGuid());
        orden.ClienteId = ordenDto.ClienteId;
        orden.Estado = OrdenEstado.Registrada;
        orden.Fecha = ordenDto.Fecha;

        var observaciones = string.Empty;
        foreach (var item in ordenDto.Items)
        {
            //TODO: Depende de negocio, reglas
            //1. Si no existe producto, no se agrega a la orden.
            //2. Si no existe producto, agregar otro producto. (Requiere mayor logica) 
            var productoDto = await productoAppService.GetByIdAsync(item.ProductId);
            if (productoDto != null){
                var ordenItem = new OrdenItem(Guid.NewGuid());
                ordenItem.Cantidad = item.Cantidad;
                ordenItem.Precio = productoDto.Precio;
                ordenItem.ProductId = productoDto.Id;
                ordenItem.Observaciones = item.Observaciones;
                //ordenItem.SubTotal = (Cantidad * Precio) - Descuento ??
                orden.AgregarItem(ordenItem);
            }else{
                observaciones+=$"El producto {item.ProductId}, no existe";
            }
        }
        orden.Total =  orden.Items.Sum(x => x.Cantidad*x.Precio);
        orden.Observaciones = observaciones;

        //3. Persistencias.
        orden = await ordenRepository.AddAsync(orden);
        await ordenRepository.UnitOfWork.SaveChangesAsync();
        
        return await GetByIdAsync(orden.Id);
    }

    public async Task<bool> AnularAsync(Guid ordenId)
    {
      var orden = await ordenRepository.GetByIdAsync(ordenId);
        if (orden == null){
            throw new ArgumentException($"La orden con el id: {ordenId}, no existe");
        }

        ordenRepository.Delete(orden);
        await ordenRepository.UnitOfWork.SaveChangesAsync();

        return true;
    }

    public ListaPaginada<OrdenDto> GetAll(int limit = 10, int offset = 0)
    {
        var ordenList = ordenRepository.GetAll();

        var ordenListDto =  (from c in ordenList
                            select new OrdenDto(){
                                Id = c.Id,
                                Cliente = c.Cliente.Nombres,
                                Total = c.Total
                            }).ToList();
        ListaPaginada<OrdenDto> lista = new ListaPaginada<OrdenDto>();
        lista.Lista = ordenListDto;
        lista.Total = ordenList.Count();
        return lista;

    }

    public Task<OrdenDto> GetByIdAsync(Guid id)
    {
        var consulta = ordenRepository.GetAllIncluding(x => x.Cliente, x => x.Items); //, x => x.Vendedor);
        consulta = consulta.Where(x => x.Id == id);

        var consultaOrdenDto = consulta
                                .Select(
                                    x => new OrdenDto()
                                    {
                                         //VendedorNombre = $"{x.Vendedor.Nombre} {x.Vendedor.Apellido}", 
                                         Id = x.Id,
                                         Cliente = x.Cliente.Nombres,
                                         ClienteId = x.ClienteId,
                                         Estado = x.Estado,
                                         Fecha = x.Fecha,
                                         FechaAnulacion = x.FechaAnulacion,
                                         Observaciones = x.Observaciones,
                                         Total = x.Total,
                                         Items = x.Items.Select(item => new OrdenItemDto(){
                                            Cantidad = item.Cantidad,
                                            Id = item.Id,
                                            Observaciones = item.Observaciones,
                                            OrdenId = item.OrdenId,
                                            Precio  = item.Precio,
                                            ProductId = item.ProductId,
                                            Product = item.Product.Nombre
                                         }).ToList()
                                    }
                                ); 
        return Task.FromResult(consultaOrdenDto.SingleOrDefault()!);
    }

    public async Task UpdateAsync(Guid id, OrdenActualizarDto ordenDto)
    {
         var orden = await ordenRepository.GetByIdAsync(id);
        orden.Observaciones = ordenDto.Observaciones;
        orden.ClienteId = ordenDto.ClienteId;
        await ordenRepository.UpdateAsync(orden);
        await ordenRepository.UnitOfWork.SaveChangesAsync();
        return;
    }
}

