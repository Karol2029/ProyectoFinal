

using Microsoft.AspNetCore.Mvc;

namespace Curso.ComercioElectronico.HttpApi.Controllers;


[ApiController]
[Route("[controller]")]
public class HolaController: ControllerBase {


    [HttpGet]
    public string Hola(){
        return "Hola Mundo";
    }

}