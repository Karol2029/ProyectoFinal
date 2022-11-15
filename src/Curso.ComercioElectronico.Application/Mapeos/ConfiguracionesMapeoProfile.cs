

using AutoMapper;
using Curso.ComercioElectronico.Application;
using Curso.ComercioElectronico.Domain;

public class ConfiguracionesMapeoProfile : Profile
{
    //TipoProductoCrearActualizarDto => TipoProducto
    //TipoProducto => TipoProductoDto
    public ConfiguracionesMapeoProfile()
    {
        CreateMap<TipoProductoCrearActualizarDto, TipoProducto>();
        CreateMap<TipoProducto, TipoProductoDto>();

        //TODO: Agregar otros mapeos que se requieren...

    }
}

