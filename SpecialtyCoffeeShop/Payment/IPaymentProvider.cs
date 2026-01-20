namespace SpecialtyCoffeeShop.Payment;

public interface IPaymentProvider
{
    Task ChargeAsync(decimal amountToCharge);
}