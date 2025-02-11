using System.ComponentModel.DataAnnotations;

namespace Test_Net.Models
{


    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        public string Ten { get; set; }
        public decimal Gia { get; set; }
        [Required(ErrorMessage = "Ngày nhập không được để trống.")]
        public DateTime NgayNhap { get; set; }
        public int? ProductTypeId { get; set; }  // Đã chuyển sang quan hệ One-to-Many       
        public ProductType? ProductType { get; set; }
        //public ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
        //public ICollection<ProductProductType> ProductProductTypes { get; set; } = new List<ProductProductType>();                                         


    }

}
