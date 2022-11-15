

using Curso.ComercioElectronico.Application;
using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class CarroController : ControllerBase
{

    private readonly ICarroAppService carroAppService;

    public CarroController(ICarroAppService carroAppService)
    {
        this.carroAppService = carroAppService;
    }

    [HttpGet]
    public ListaPaginada<CarroDto> GetAll(int limit=10,int offset=0)
    {

        return carroAppService.GetAll(limit,offset);

    }

    [HttpGet("{id}")]
    public async Task<CarroDto>  GetByIdAsync(Guid id)
    {
        return await carroAppService.GetByIdAsync(id);
    }


    

    [HttpPost]
    public async Task<CarroDto> CreateAsync(CarroCrearDto marca)
    {

        return await carroAppService.CreateAsync(marca);

    }

    [HttpPut]
    public async Task UpdateAsync(Guid id, CarroActualizarDto marca)
    {

        await carroAppService.UpdateAsync(id, marca);

    }

    [HttpDelete]
    public async Task<bool> DeleteAsync(Guid carroId)
    {

        return await carroAppService.DeleteAsync(carroId);

    }

}