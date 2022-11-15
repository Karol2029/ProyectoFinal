using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

/**
* TODO: Implementar todos los metodos del servicio de aplicacion de clientes
*/
public class ClienteAppService : IClienteAppService
{
    private readonly IClienteRepository repository;

    public ClienteAppService(IClienteRepository repository)
    {
        this.repository = repository;
    }

    public async Task<ClienteDto> CreateAsync(ClienteCrearActualizarDto clienteDto)
    {
        //TODO: Aplicar validaciones
      
 
        //Mapeo Dto => Entidad
        var cliente = new Cliente();
        cliente.Id = Guid.NewGuid();
        cliente.Nombres = clienteDto.Nombres;
 
        //Persistencia objeto
        cliente = await repository.AddAsync(cliente);
        await repository.UnitOfWork.SaveChangesAsync();

        //Mapeo Entidad => Dto
        var clienteCreado = new ClienteDto();
        clienteCreado.Id = cliente.Id;
        clienteCreado.Nombres = cliente.Nombres; 

        return clienteCreado;
    }

    public async Task<bool> DeleteAsync(Guid clienteId)
    {
         var cliente = await repository.GetByIdAsync(clienteId);
         repository.Delete(cliente);
        await repository.UnitOfWork.SaveChangesAsync();
        return true;
        
    }

    public ICollection<ClienteDto> GetAll(string buscar, int limit = 10, int offset = 0)
    {
        var clienteList = repository.GetAll();

        var clienteListDto =  (from c in clienteList
                            select new ClienteDto(){
                                Id = c.Id,
                                Nombres = c.Nombres,
                            }).ToList();
        return clienteListDto;
    }

    public async Task UpdateAsync(Guid id, ClienteCrearActualizarDto clienteDto)
    {
         var cliente = await repository.GetByIdAsync(id);
        cliente.Nombres = clienteDto.Nombres;
        await repository.UpdateAsync(cliente);
        await repository.UnitOfWork.SaveChangesAsync();
        return;
    }
}
