using Basket.API.Dtos;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

// command

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckOutDto) : ICommand<CheckoutBasketResult>;

// result
public record CheckoutBasketResult(bool IsSuccess);

// validator
public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketValidator()
    {
        RuleFor(x => x.BasketCheckOutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckOutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

// handler

public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command
        , CancellationToken cancellationToken)
    {
        // get existing basket with user name
        var basket = await repository.GetBasket(command.BasketCheckOutDto.UserName, cancellationToken);
        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }
        // set total price on basket checkout dto
        // set all necessary fields from the ShoppingCart object to ensure read-only integrity.
        // You can add more fields as needed based on your requirements.
        var eventMessage = command.BasketCheckOutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;
        // send basket checkout event to rabbitmq
        await publishEndpoint.Publish(eventMessage, cancellationToken);
        // remove basket from redis
        await repository.DeleteBasket(command.BasketCheckOutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}
