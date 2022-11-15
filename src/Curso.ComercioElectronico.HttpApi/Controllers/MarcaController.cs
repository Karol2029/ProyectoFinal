

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class MarcaController : ControllerBase
{

    private readonly IMarcaAppService marcaAppService;

    public MarcaController(IMarcaAppService marcaAppService)
    {
        this.marcaAppService = marcaAppService;
    }

    [HttpGet]
    public ICollection<MarcaDto> GetAll()
    {

        return marcaAppService.GetAll();
    }

    [HttpPost]
    public async Task<MarcaDto> CreateAsync(MarcaCrearActualizarDto marca)
    {

        return await marcaAppService.CreateAsync(marca);

    }

    [HttpPut]
    public async Task UpdateAsync(int id, MarcaCrearActualizarDto marca)
    {

        await marcaAppService.UpdateAsync(id, marca);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(int marcaId)
    {

        return await marcaAppService.DeleteAsync(marcaId);

    }

}