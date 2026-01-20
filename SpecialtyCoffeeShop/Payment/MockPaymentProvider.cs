namespace SpecialtyCoffeeShop.Payment;

public class MockPaymentProvider : IPaymentProvider
{
    public async Task ChargeAsync(decimal amountToCharge)
    {
        if (amountToCharge > 150000)
        {
            throw new InvalidOperationException("Not enough funds in the account.");
        }

        // Simulate wait
        await Task.Delay(TimeSpan.FromSeconds(3));
    }
}