using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PillsServer.Common;
using PillsServer.Persistancy;

namespace PillsServer.CQRS.Commands
{
    public class AddPillCommandValidator : AbstractValidator<AddPillCommand>
    {
        public AddPillCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Error! Name is required")
                .MaximumLength(100)
                .WithMessage("Error! Name must be less than 100 characters");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Stock.HasValue)
                .WithMessage("Error! Stock must be greater than or equal to 0");
            RuleFor(x => x.PillsToTakePerTimeOfDay)
                .Must(x => x.Length == 3)
                .When(x => x.PillsToTakePerTimeOfDay != null)
                .WithMessage("Error! PillsToTakePerTimeOfDay must have 3 elements")
                .Must(x => x.Length==3 && x[0] >= 0 && x[1] >= 0 && x[2] >= 0)
                .When(x => x.PillsToTakePerTimeOfDay != null)
                .WithMessage("Error! PillsToTakePerTimeOfDay elements must be greater than or equal to 0");
        }
    }
    public class UpdatePillCommandValidator : AbstractValidator<UpdatePillCommand>
    {
        private IRepo<Pill> _repo;
        public UpdatePillCommandValidator(IRepo<Pill> repo)
        {
            _repo = repo;
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Error! Id must be greater than or equal to 0")
                .MustAsync(async (id, cancellation) 
                    => await repo.Query(x => x.Id == id).AnyAsync(cancellation))
                .WithMessage("Error! This pill does not exist");
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .WithMessage("Error! Name must be less than 100 characters");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Stock.HasValue)
                .WithMessage("Error! Stock must be greater than or equal to 0");
            RuleFor(x => x.PillsToTakePerTimeOfDay)
                .Must(x => x.Length == 3)
                .When(x => x.PillsToTakePerTimeOfDay != null)
                .WithMessage("Error! PillsToTakePerTimeOfDay must have 3 elements")
                .Must(x => x[0] >= 0 && x[1] >= 0 && x[2] >= 0)
                .When(x => x.PillsToTakePerTimeOfDay != null)
                .WithMessage("Error! PillsToTakePerTimeOfDay elements must be greater than or equal to 0");
        }
    }
    public class DeletePillCommandValidator : AbstractValidator<DeletePillCommand>
    {
        private IRepo<Pill> _repo;
        public DeletePillCommandValidator(IRepo<Pill> repo)
        {
            _repo = repo;
            RuleFor(x => x.Id)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Error! Id must be greater than or equal to 0")
                .MustAsync(async (id, cancellation)
                    => await repo.Query(x => x.Id == id).AnyAsync(cancellation))
                .WithMessage("Error! This pill does not exist");
        }
    }
}
