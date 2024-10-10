public class Order_Detail
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }

    // Navigation properties
    public Order Order { get; set; }
    public Product Product { get; set; }
}
