using System.ComponentModel.DataAnnotations;

namespace Test_Net.Models
{
    public class ProductProductType
    {
        [Key]
        public int Id { get; set; } // Khóa chính (Primary Key)
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
