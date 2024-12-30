using BuildingBlocks.CQRS;
using FluentValidation;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderResult(bool IsSuccess);

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public class  UpdateCommandValidator:AbstractValidator<UpdateOrderCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
    }
}
