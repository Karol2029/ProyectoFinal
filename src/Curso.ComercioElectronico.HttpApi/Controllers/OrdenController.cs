

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class OrdenController : ControllerBase
{

    private readonly IOrdenAppService ordenAppService;

    public OrdenController(IOrdenAppService ordenAppService)
    {
        this.ordenAppService = ordenAppService;
    }

    [HttpGet]
    public ListaPaginada<OrdenDto> GetAll(int limit=10,int offset=0)
    {

        return ordenAppService.GetAll(limit,offset);

    }

    [HttpGet("{id}")]
    public async Task<OrdenDto>  GetByIdAsync(Guid id)
    {
        return await ordenAppService.GetByIdAsync(id);
    }


    

    [HttpPost]
    public async Task<OrdenDto> CreateAsync(OrdenCrearDto marca)
    {

        return await ordenAppService.CreateAsync(marca);

    }

    [HttpPut]
    public async Task UpdateAsync(Guid id, OrdenActualizarDto marca)
    {

        await ordenAppService.UpdateAsync(id, marca);

    }

    [HttpDelete]
    public async Task<bool> AnularAsync(Guid ordenId)
    {

        return await ordenAppService.AnularAsync(ordenId);

    }

}