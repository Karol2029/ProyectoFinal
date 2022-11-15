
using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;

namespace Curso.ComercioElectronico.Infraestructure;

public class ProductoRepository : EfRepository<Producto,int>, IProductoRepository
{
    public ProductoRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }

    public async Task<ICollection<Producto>> GetListAsync(IList<int> listaIds, bool asNoTracking = true)
    {
        //GetAll, se ejecuta el linq???
        var consulta = GetAll(asNoTracking);

        consulta = consulta.Where(
                x => listaIds.Contains(x.Id)
            );

        //select * from productos where id in (1,3,4,5)
        return await consulta.ToListAsync();
       
    }
}