namespace Curso.ComercioElectronico.Application;



public interface ICarroAppService
{
    Task<CarroDto> GetByIdAsync(Guid id);

    ListaPaginada<CarroDto> GetAll(int limit=10,int offset=0);


    Task<CarroDto> CreateAsync(CarroCrearDto orden);

    Task UpdateAsync (Guid id, CarroActualizarDto orden);

    Task<bool> DeleteAsync(Guid carroId);
}