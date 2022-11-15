using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class CarroDto
{
    [Required]
    public Guid Id {get;set; }
 
    [Required]
    public Guid ClienteId {get;set;}
   
    public virtual string Cliente {get;set;}

    public virtual ICollection<OrdenItemDto> Items {get;set;}

    [Required]
    public DateTime Fecha {get;set;}

    public DateTime? FechaAnulacion {get;set;}
    

    [Required]
    public decimal Total {get;set;}

    public string? Observaciones { get;set;}

   
 
}


public class CarroItemsDto {

    [Required]
    public Guid Id {get;set; }

    [Required]
    public int CarroId {get; set;}

    public virtual string Carro { get; set; }

    [Required]
    public int CarrosId {get; set;}

   
    [Required]
    public long Cantidad {get;set;}

    public decimal Precio {get;set;}

    public string? Observaciones { get;set;}
}
