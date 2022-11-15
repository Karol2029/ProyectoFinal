namespace Curso.ComercioElectronico.Application;

public class ProductoListInput {

    public int Limit {get;set;} = 10;
    public int Offset {get;set;} = 0;

    public int? TipoProductoId {get;set;}
    
    public int? MarcaId {get;set;}

    public string? ValorBuscar {get;set;}

}