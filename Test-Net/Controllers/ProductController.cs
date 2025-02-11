using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Test_Net.Data;
using System;
using Test_Net.Models;

namespace Test_Net.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _context.Products
                .Include(p => p.ProductType) 
                .Select(p => new {
                    p.Id,
                    p.Ten,
                    p.Gia,
                    p.NgayNhap,
                    LoaiSanPham = p.ProductType.Ten // Lấy tên loại sản phẩm
                })
                .ToListAsync();

            return Json(new { data });
        }
        [HttpGet]
        public async Task<IActionResult> GetProductTypes()
        {
            var productTypes = await _context.ProductTypes
                .Select(pt => new { pt.Id, pt.Ten })
                .ToListAsync();

            return Json(productTypes);
        }

        public async Task<IActionResult> Add([FromBody] Product model)
        {
            var errors = new Dictionary<string, string>();

            // Kiểm tra tên sản phẩm
            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                errors["Ten"] = "Tên sản phẩm không được để trống.";
            }
            else if (await _context.Products.AnyAsync(p => p.Ten == model.Ten))
            {
                errors["Ten"] = "Tên sản phẩm đã tồn tại.";
            }

            // Kiểm tra ngày nhập
            if (model.NgayNhap == null || model.NgayNhap > DateTime.UtcNow)
            {
                errors["NgayNhap"] = "Ngày nhập không hợp lệ.";
            }

            // Kiểm tra loại sản phẩm
            if (model.ProductTypeId <= 0 || !await _context.ProductTypes.AnyAsync(pt => pt.Id == model.ProductTypeId))
            {
                errors["ProductTypeId"] = "Loại sản phẩm không hợp lệ.";
            }

            // Nếu có lỗi, trả về BadRequest
            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }

            // Thêm sản phẩm vào database
            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Thêm sản phẩm thành công!" });
        }

        [HttpPost]
        [Route("Product/Edit")]
        public async Task<IActionResult> Edit([FromBody] Product model)
        {
            if (!string.IsNullOrWhiteSpace(model.Ten))
            {
                var existingProduct = await _context.Products.FindAsync(model.Id);
                if (existingProduct == null)
                {
                    return NotFound(new { success = false, message = "Sản phẩm không tồn tại!" });
                }
                bool isDuplicate = await _context.Products.AnyAsync(p => p.Ten == model.Ten && p.Id != model.Id);
                existingProduct.Ten = model.Ten;
                existingProduct.Gia = model.Gia;
                existingProduct.NgayNhap = model.NgayNhap;

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Cập nhật sản phẩm thành công!" });
            }
            return BadRequest(new { success = false});
        }



        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { success = false, message = "Sản phẩm không tồn tại!" });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa sản phẩm thành công!" });
        }
        [HttpPost]
        [Route("Product/DeleteAll")]
        public async Task<IActionResult> DeleteAll()
        {
            var products = await _context.Products.ToListAsync();
            if (products.Count == 0)
            {
                return NotFound(new { success = false, message = "Không có sản phẩm nào để xóa!" });
            }

            _context.Products.RemoveRange(products);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Đã xóa hết sản phẩm!" });
        }




    }
}
