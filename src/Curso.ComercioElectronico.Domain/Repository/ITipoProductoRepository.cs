namespace Curso.ComercioElectronico.Domain;

public interface ITipoProductoRepository :  IRepository<TipoProducto,int> {


    Task<bool> ExisteNombre(string nombre);

    Task<bool> ExisteNombre(string nombre, int idExcluir);


}
