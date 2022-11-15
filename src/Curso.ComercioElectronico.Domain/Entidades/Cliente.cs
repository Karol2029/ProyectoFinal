using System.ComponentModel.DataAnnotations;

namespace Curso.ComercioElectronico.Domain;

public class Cliente {

    [Required]
    public Guid Id {get;set; }

    [Required]
    [StringLength(DominioConstantes.NOMBRE_MAXIMO)]
    public string Nombres {get;set;}

    //TODO: Agregar campos adicionales. 
     
}
