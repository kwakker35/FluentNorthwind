namespace FluentNorthwind.Lib.Model;
public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public int SupplierID { get; set; }
    public int CategoryID { get; set; }
    public string QuantityPerUnit { get; set; }
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
    public short UnitsOnOrder { get; set; }
    public short ReorderLevel { get; set; }
    public bool Discontinued { get; set; }

    // Navigation properties
    public Category Category { get; set; }
    public Supplier Supplier { get; set; }
    public ICollection<Order_Detail> Order_Details { get; set; }
}
