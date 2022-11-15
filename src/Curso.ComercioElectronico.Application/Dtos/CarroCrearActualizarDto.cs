using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class CarroCrearDto
{
    
    [Required]
    public Guid ClienteId {get;set;}

    [Required]   
    public virtual ICollection<CarroItemCrearActualizarDto> Items {get;set;}

    [Required]
    public DateTime Fecha {get;set;}

    public string? Observaciones { get;set;}
  
} 

public class CarroActualizarDto
{


    public string? Observaciones { get;set;}

    public Guid ClienteId {get; set;}
        
}  



public class CarroItemCrearActualizarDto {

    public int ProductId {get; set;}
   
    [Required]
    public long Cantidad {get;set;}

    public int Precio {get; set;}
    public string? Observaciones { get;set;}
}