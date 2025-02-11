using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Test_Net.Data;
using Test_Net.Models;

namespace Test_Net.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly AppDbContext _context;

        public ProductTypeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HttpGet]
        public IActionResult GetData(string keyword)
        {
            var data = _context.ProductTypes
                .GroupJoin(
                    _context.Products,
                    pt => pt.Id,
                    p => p.ProductTypeId,
                    (pt, products) => new
                    {
                        id = pt.Id,
                        ten = pt.Ten,
                        soLuongSanPham = products.Count(),
                        ngayNhap = pt.NgayNhap.ToString("yyyy-MM-dd")
                    })
                .Where(pt => string.IsNullOrEmpty(keyword) || pt.ten.Contains(keyword))
                .ToList();

            return Json(new { data });
        }
        
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductType model)
        {
            var errors = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(model.Ten))
            {
                errors["Ten"] = "Tên loại sản phẩm không được để trống.";
            }
            else if (await _context.ProductTypes.AnyAsync(pt => pt.Ten == model.Ten))
            {
                errors["Ten"] = "Tên loại sản phẩm đã tồn tại.";
            }
            if (model.NgayNhap == default)
            {
                errors["NgayNhap"] = "Ngày nhập không hợp lệ.";
            }
            if (errors.Count > 0)
            {
                return BadRequest(errors);
            }
            _context.ProductTypes.Add(model);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Thêm loại sản phẩm thành công!" });
        }
        [HttpPost]
        [Route("ProductType/Edit")]
        public async Task<IActionResult> Edit([FromBody] ProductType model)
        {
            if (!string.IsNullOrWhiteSpace(model.Ten))
            {
                var existingType = await _context.ProductTypes.FindAsync(model.Id);
                if (existingType == null)
                {
                    return NotFound(new { success = false, message = "Loại sản phẩm không tồn tại!" });
                }
                bool isDuplicate = await _context.ProductTypes.AnyAsync(pt => pt.Ten == model.Ten && pt.Id != model.Id);
                if (isDuplicate)
                {
                    return BadRequest(new { success = false, message = "Tên loại sản phẩm đã tồn tại!" });
                }
                existingType.Ten = model.Ten;
                existingType.NgayNhap = model.NgayNhap;
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Cập nhật loại sản phẩm thành công!" });
            }
            return BadRequest(new { success = false, message = "Tên loại sản phẩm không hợp lệ!" });
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var productType = await _context.ProductTypes
                .Include(pt => pt.Products)
                .FirstOrDefaultAsync(pt => pt.Id == id);
            if (productType == null)
            {
                return NotFound(new { success = false, message = "Loại sản phẩm không tồn tại!" });
            }        
            if (productType.Products.Any())
            {
                return BadRequest(new { success = false, message = "Không thể xóa vì còn sản phẩm thuộc loại này!" });
            }
            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa loại sản phẩm thành công!" });
        }
                               
    }
}
