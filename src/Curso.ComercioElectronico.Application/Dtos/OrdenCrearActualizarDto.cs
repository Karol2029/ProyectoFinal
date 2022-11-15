using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class OrdenCrearDto
{
    
    [Required]
    public Guid ClienteId {get;set;}

    [Required]   
    public virtual ICollection<OrdenItemCrearActualizarDto> Items {get;set;}

    [Required]
    public DateTime Fecha {get;set;}

    public string? Observaciones { get;set;}
  
} 

public class OrdenActualizarDto
{
    
    [Required]
    public OrdenEstado Estado {get;set;}


    public string? Observaciones { get;set;}
    public Guid ClienteId {get; set;}
  
}  



public class OrdenItemCrearActualizarDto {

    [Required]
    public int ProductId {get; set;}
 
   
    [Required]
    public long Cantidad {get;set;}

   
    public string? Observaciones { get;set;}
}

