using Curso.ComercioElectronico.Domain;
using Microsoft.Extensions.Logging;

namespace Curso.ComercioElectronico.Application;

public class CarroAppService : ICarroAppService
{
    private readonly ICarroRepository carroRepository;
    private readonly IProductoAppService productoAppService;
    private readonly ILogger<CarroAppService> logger;

    public CarroAppService(
        ICarroRepository carroRepository,
        //IProductoRepository productoRepository,
        IProductoAppService productoAppService,
        ILogger<CarroAppService> logger )
    {
        this.carroRepository = carroRepository;
        this.productoAppService = productoAppService;
        this.logger = logger;
    }

    public async Task<CarroDto> CreateAsync(CarroCrearDto carroDto)
    {
        logger.LogInformation("Crear Carro");

        

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
        var carro = new Carro(Guid.NewGuid());
        carro.ClienteId = carroDto.ClienteId;
        carro.Fecha = carroDto.Fecha;

        var observaciones = string.Empty;
        foreach (var item in carroDto.Items)
        {
            //TODO: Depende de negocio, reglas
            //1. Si no existe producto, no se agrega a la orden.
            //2. Si no existe producto, agregar otro producto. (Requiere mayor logica) 
            var productoDto = await productoAppService.GetByIdAsync(item.ProductId);
            if (productoDto != null){
                var carroItem = new CarroItem(Guid.NewGuid());
                carroItem.Cantidad = item.Cantidad;
                carroItem.Precio = item.Precio;
                carroItem.ProductId = item.ProductId;
                carroItem.Observaciones = item.Observaciones;
                //ordenItem.SubTotal = (Cantidad * Precio) - Descuento ??
                carro.AgregarItem(carroItem);
            }else{
                observaciones+=$"El producto {carro.Id}, no existe";
            }
        }
        carro.Total =  carro.Items.Sum(x => x.Cantidad*x.Precio);
        carro.Observaciones = observaciones;

        //3. Persistencias.
        carro = await carroRepository.AddAsync(carro);
        await carroRepository.UnitOfWork.SaveChangesAsync();
        
        return await GetByIdAsync(carro.Id);
    }

    public ListaPaginada<CarroDto> GetAll(int limit = 10, int offset = 0)
    {
         var carroList = carroRepository.GetAll();

        var carroListDto =  (from c in carroList
                            select new CarroDto(){
                                Id = c.Id,
                                Cliente = c.Cliente.Nombres,
                                Total = c.Total
                            }).ToList();
        ListaPaginada<CarroDto> lista = new ListaPaginada<CarroDto>();
        lista.Lista = carroListDto;
        lista.Total = carroList.Count();
        return lista;

    }

    public Task<CarroDto> GetByIdAsync(Guid id)
    {
        var consulta = carroRepository.GetAllIncluding(x => x.Cliente, x => x.Items); //, x => x.Vendedor);
        consulta = consulta.Where(x => x.Id == id);

        var consultaCarroDto = consulta
                                .Select(
                                    x => new CarroDto()
                                    {
                                         //VendedorNombre = $"{x.Vendedor.Nombre} {x.Vendedor.Apellido}", 
                                         Id = x.Id,
                                         Cliente = x.Cliente.Nombres,
                                         ClienteId = x.ClienteId,
                                         Fecha = x.Fecha,
                                         FechaAnulacion = x.FechaAnulacion,
                                         Observaciones = x.Observaciones,
                                         Total = x.Total,
                                         Items = x.Items.Select(item => new OrdenItemDto(){
                                            Cantidad = item.Cantidad,
                                            Id = item.Id,
                                            Observaciones = item.Observaciones,
                                            OrdenId = item.CarroId,
                                            Precio  = item.Precio,
                                            ProductId = item.ProductId,
                                            Product = item.Product.Nombre
                                         }).ToList()
                                    }
                                ); 
        return Task.FromResult(consultaCarroDto.SingleOrDefault()!);
    }

    
    public async Task<bool> DeleteAsync(Guid carroId)
    {  
        var carro = await carroRepository.GetByIdAsync(carroId);
        if (carro == null){
            throw new ArgumentException($"El carro con el id: {carroId}, no existe");
        }

        carroRepository.Delete(carro);
        await carroRepository.UnitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task UpdateAsync(Guid id, CarroActualizarDto carroDto)
    {
         var carro = await carroRepository.GetByIdAsync(id);
        carro.Observaciones = carroDto.Observaciones;
        carro.ClienteId = carroDto.ClienteId;
        await carroRepository.UpdateAsync(carro);
        await carroRepository.UnitOfWork.SaveChangesAsync();
        return;
    }
}
