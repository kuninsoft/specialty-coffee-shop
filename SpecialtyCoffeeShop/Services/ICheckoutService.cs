using SpecialtyCoffeeShop.Models.Dto;
using SpecialtyCoffeeShop.Models.RequestDto;

namespace SpecialtyCoffeeShop.Services;

public interface ICheckoutService
{
    Task<OrderDetailsDto> CalculateOrderDetailsAsync(CalculateOrderDto order);
    Task<OrderInfoDto> PlaceOrderAsync(PlaceOrderDto orderRequest);
}