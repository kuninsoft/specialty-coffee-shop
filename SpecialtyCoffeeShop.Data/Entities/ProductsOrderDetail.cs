using System.ComponentModel.DataAnnotations.Schema;

namespace SpecialtyCoffeeShop.Data.Entities;

public class ProductsOrderDetail
{
    public int? ProductId { get; set; }
    public Product Product { get; set; }
    
    public Guid? OrderId { get; set; }
    public Order Order { get; set; }

    public int Quantity { get; set; }
}