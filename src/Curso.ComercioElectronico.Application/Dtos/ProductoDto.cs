using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;

public class ProductoDto
{
    [Required]
    public int Id {get;set;}

    [Required]
    [StringLength(DominioConstantes.NOMBRE_MAXIMO)]
    public string Nombre {get;set;}

    public decimal Precio {get;set;}

    public string? Observaciones {get;set;}

    public DateTime? Caducidad {get;set;}
    

    [Required]
    public int MarcaId {get;set;}

    public string  Marca {get; set; }


    [Required]
    public int TipoProductoId {get;set;}

    public string TipoProducto {get;set;}

}

