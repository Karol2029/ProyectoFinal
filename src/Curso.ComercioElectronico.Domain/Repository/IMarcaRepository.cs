namespace Curso.ComercioElectronico.Domain;

public interface IMarcaRepository :  IRepository<Marca,int> {


    Task<bool> ExisteNombre(string nombre);

    Task<bool> ExisteNombre(string nombre, int idExcluir);


}
