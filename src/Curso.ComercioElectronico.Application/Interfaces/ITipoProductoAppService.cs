namespace Curso.ComercioElectronico.Application;

public interface ITipoProductoAppService
{
    Task<TipoProductoDto> GetByIdAsync(int id);

    ListaPaginada<TipoProductoDto> GetAll(int limit=10,int offset=0);

    Task<TipoProductoDto> CreateAsync(TipoProductoCrearActualizarDto marca);

    Task UpdateAsync (int id, TipoProductoCrearActualizarDto marca);

    Task<bool> DeleteAsync(int marcaId);
}

