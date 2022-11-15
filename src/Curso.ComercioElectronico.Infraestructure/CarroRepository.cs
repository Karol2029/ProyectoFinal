using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Infraestructure;

public class CarroRepository : EfRepository<Carro,Guid>, ICarroRepository
{
    public CarroRepository(ComercioElectronicoDbContext context) : base(context)
    {
    } 
}