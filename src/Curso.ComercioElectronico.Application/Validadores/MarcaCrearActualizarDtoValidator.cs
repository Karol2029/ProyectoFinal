using FluentValidation;

namespace Curso.ComercioElectronico.Application;

 
public class MarcaCrearActualizarDtoValidator : AbstractValidator<MarcaCrearActualizarDto>
{
    public MarcaCrearActualizarDtoValidator()
    {
        //RuleFor(x => x).Must(x => false).WithMessage("Verificacion de validaciones cen el proyecto");

        RuleFor(x => x.Nombre).Length(1,2);
    }
}