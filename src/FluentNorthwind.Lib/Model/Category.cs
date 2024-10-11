namespace FluentNorthwind.Lib.Model;
public class Category
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public byte[] Picture { get; set; }

    // Navigation property
    public ICollection<Product> Products { get; set; }
}
