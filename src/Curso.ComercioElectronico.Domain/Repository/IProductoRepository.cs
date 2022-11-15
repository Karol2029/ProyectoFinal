namespace Curso.ComercioElectronico.Domain;

public interface IProductoRepository :  IRepository<Producto,int> {

   Task<ICollection<Producto>> GetListAsync(IList<int> listaIds, bool asNoTracking = true);

}
