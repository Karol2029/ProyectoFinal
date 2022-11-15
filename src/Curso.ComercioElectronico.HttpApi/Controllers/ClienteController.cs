

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{

    private readonly IClienteAppService clienteAppService;

    public ClienteController(IClienteAppService clienteAppService)
    {
        this.clienteAppService = clienteAppService;
    }

     

    [HttpPost]
    public async Task<ClienteDto> CreateAsync(ClienteCrearActualizarDto clienteCrearActualizarDto)
    {

        return await clienteAppService.CreateAsync(clienteCrearActualizarDto);

    }
 
    [HttpPut]
    public async Task UpdateAsync(Guid id, ClienteCrearActualizarDto clienteCrearActualizarDto)
    {

        await clienteAppService.UpdateAsync(id, clienteCrearActualizarDto);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(Guid clienteId)
    {

        return await clienteAppService.DeleteAsync(clienteId);

    }

    [HttpGet]
    public ICollection<ClienteDto> GetAll(string buscar,int limit=10,int offset=0)
    {

        return clienteAppService.GetAll(buscar,limit,offset);

    }

    

    //TODO: Agregar las otras capacidades del api de clientes..

}