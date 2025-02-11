using System.ComponentModel.DataAnnotations;

namespace Test_Net.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên loại sản phẩm không được để trống.")]
        public string Ten { get; set; }
        [Required]
        public DateTime NgayNhap { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<ProductProductType> ProductProductTypes { get; set; } = new List<ProductProductType>();

    }

}
