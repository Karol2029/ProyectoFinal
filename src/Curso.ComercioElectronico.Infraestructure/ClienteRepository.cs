
using Curso.ComercioElectronico.Domain;
using Microsoft.EntityFrameworkCore;

namespace Curso.ComercioElectronico.Infraestructure;

public class ClienteRepository : EfRepository<Cliente,Guid>, IClienteRepository
{
    public ClienteRepository(ComercioElectronicoDbContext context) : base(context)
    {
    }
 

    
}

 