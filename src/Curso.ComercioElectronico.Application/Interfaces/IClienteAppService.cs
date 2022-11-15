using System.ComponentModel.DataAnnotations;
using Curso.ComercioElectronico.Domain;

namespace Curso.ComercioElectronico.Application;


public interface IClienteAppService
{

    ICollection<ClienteDto> GetAll(string buscar,int limit=10,int offset=0);

    Task<ClienteDto> CreateAsync(ClienteCrearActualizarDto clienteDto);

    Task UpdateAsync (Guid clienteId, ClienteCrearActualizarDto clienteDto);

    Task<bool> DeleteAsync(Guid clienteId);
}
