namespace Curso.ComercioElectronico.Application;



public interface IOrdenAppService
{
    Task<OrdenDto> GetByIdAsync(Guid id);

    ListaPaginada<OrdenDto> GetAll(int limit=10,int offset=0);

    //ListaPaginada<OrdenDto> GetByClientIdAll(int clientId, int limit=10,int offset=0);


    Task<OrdenDto> CreateAsync(OrdenCrearDto orden);

    Task UpdateAsync (Guid id, OrdenActualizarDto orden);

    Task<bool> AnularAsync(Guid ordenId);
}

