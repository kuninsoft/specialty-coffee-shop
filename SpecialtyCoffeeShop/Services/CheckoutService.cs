using SpecialtyCoffeeShop.Data.Entities;
using SpecialtyCoffeeShop.Data.UnitOfWork;
using SpecialtyCoffeeShop.Models.Dto;
using SpecialtyCoffeeShop.Models.RequestDto;
using SpecialtyCoffeeShop.Payment;

namespace SpecialtyCoffeeShop.Services;

public class CheckoutService(IUnitOfWork unitOfWork, IPaymentProvider paymentProvider, ILogger<CheckoutService> logger) 
    : ICheckoutService
{
    public async Task<OrderDetailsDto> CalculateOrderDetailsAsync(CalculateOrderDto order)
    {
        Dictionary<int, Product> productsByIds = await GetProductsIds(order.Items);

        return CalculateFromProducts(productsByIds, order);
    }

    public async Task<OrderInfoDto> PlaceOrderAsync(PlaceOrderDto orderRequest)
    {
        Dictionary<int, Product> productsByIds = await GetProductsIds(orderRequest.Items);
        
        OrderDetailsDto orderPrice = CalculateFromProducts(productsByIds, new CalculateOrderDto
        {
            Items = orderRequest.Items
        });
        
        try
        {
            await unitOfWork.BeginTransactionAsync();

            var order = new Order
            {
                FullName = orderRequest.ShippingInfo.FullName,
                Address = orderRequest.ShippingInfo.Address,
                City = orderRequest.ShippingInfo.City,
                Region = orderRequest.ShippingInfo.Region,
                Country = orderRequest.ShippingInfo.Country
            };

            if (!string.IsNullOrWhiteSpace(orderRequest.ShippingInfo.Email))
            {
                order.Email = orderRequest.ShippingInfo.Email;
            }

            if (!string.IsNullOrWhiteSpace(orderRequest.ShippingInfo.PhoneNumber))
            {
                order.PhoneNumber = orderRequest.ShippingInfo.PhoneNumber;
            }

            foreach (OrderItemDto orderItem in orderRequest.Items)
            {
                order.ProductsDetails.Add(new ProductsOrderDetail
                {
                    ProductId = orderItem.Id,
                    Quantity = orderItem.Quantity,
                });
                
                productsByIds[orderItem.Id].Stock -= orderItem.Quantity;
                
                unitOfWork.Orders.Add(order);
            }
            
            await unitOfWork.SaveChangesAsync();

            await paymentProvider.ChargeAsync(orderPrice.TotalPrice);

            await unitOfWork.CommitTransactionAsync();

            return new OrderInfoDto(order.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Order placement failed!");

            await unitOfWork.RollbackTransactionAsync();

            throw;
        }
    }
    
    private static OrderDetailsDto CalculateFromProducts(Dictionary<int, Product> productsIds,
        CalculateOrderDto order)
    {
        var resultDto = new OrderDetailsDto();
        
        foreach (OrderItemDto orderItem in order.Items)
        {
            Product product = productsIds[orderItem.Id];
            
            if (product.Stock < orderItem.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock for product {product.Id}");
            }

            resultDto.SubtotalPrice += product.Price * orderItem.Quantity;
            resultDto.Discount += product.CurrentDiscount * orderItem.Quantity;
        }

        resultDto.TotalPrice = resultDto.SubtotalPrice - resultDto.Discount;

        return resultDto;
    }

    private async Task<Dictionary<int, Product>> GetProductsIds(ICollection<OrderItemDto> orderItems)
    {
        List<int> productIds = orderItems.Select(i => i.Id)
                                         .Distinct()
                                         .ToList();
        
        List<Product> products = await unitOfWork.Products.GetAsync(product => productIds.Contains(product.Id));

        return products.Count != productIds.Count
            ? throw new KeyNotFoundException() 
            : products.ToDictionary(p => p.Id);
    }
}