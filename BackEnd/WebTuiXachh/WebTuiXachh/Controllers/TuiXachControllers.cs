using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using static API.Controllers.UserController;

namespace API.Controllers
{
    //[Authorize] // Bảo vệ tất cả các API trong controller này
    [ApiController]
    [Route("api/[controller]")]
    public class TuiXachController : ControllerBase
    {
        private readonly ITuiXachBusiness _tuiXachBusiness;

        public TuiXachController(ITuiXachBusiness tuiXachBusiness)
        {
            _tuiXachBusiness = tuiXachBusiness;
        }

       


        // API: Lấy tất cả sản phẩm túi xách
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var data = _tuiXachBusiness.GetDataAll();
                return Ok(data);  // Trả về danh sách túi xách, bao gồm cả hình ảnh
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDataAll: {ex.Message}");
                return StatusCode(500, "Internal server error: " + ex.Message); // Trả về lỗi server nếu có
            }
        }


        // API: Lấy thông tin túi xách theo mã sản phẩm
        [HttpGet("get-by-id/{maSp}")]
        public IActionResult GetTuiXachById(string maSp)
        {
            try
            {
                var tuiXach = _tuiXachBusiness.GetTuiXachById(maSp);
                if (tuiXach == null)
                {
                    return NotFound(new { message = "Túi xách không tồn tại" });
                }
                return Ok(new { Success = true, TuiXach = tuiXach });  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi", details = ex.Message });

            }
        }

        [HttpGet("get-by-danhmuc/{maDanhMuc}")]
        public IActionResult GetByDanhMuc(string maDanhMuc)
        {
            try
            {
                var tuiXach = _tuiXachBusiness.GetByDanhMuc(maDanhMuc);
                if (tuiXach == null)
                {
                    return NotFound();
                }
                return Ok(tuiXach);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);

            }
        }
   

        // API: Lấy sản phẩm theo màu sắc
        [HttpGet("get-by-mausac/{tenMau}")]
        public IActionResult GetByMauSac(string tenMau)
        {
            try
            {
                var tuiXach = _tuiXachBusiness.GetByMauSac(tenMau);
                if (tuiXach == null)
                {
                    return NotFound();
                }
                return Ok(tuiXach);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);

            }
        }


        // API: Lấy sản phẩm theo size
        [HttpGet("get-by-size/{maSize}")]
        public IActionResult GetBySize(string maSize)
        {
            try
            {
                var tuiXach = _tuiXachBusiness.GetBySize(maSize);
                if (tuiXach == null)
                {
                    return NotFound();
                }
                return Ok(tuiXach);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);

            }
        }
        
        // API: Thêm sản phẩm túi xách
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] TuiXachModel model, IFormFile hinhAnhFile)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Kiểm tra hình ảnh và lưu hình ảnh nếu có
                if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{hinhAnhFile.FileName}";
                    var filePath = Path.Combine(@"D:\DOAN2_API\BackEnd\WebTuiXachh\images", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhFile.CopyToAsync(stream);
                    }
                    model.HinhAnh = uniqueFileName;  // Gán tên hình ảnh cho model
                }

                var result = _tuiXachBusiness.CreateTuiXach(model, hinhAnhFile);
                if (result)
                {
                    return Ok(new { Success = true, Message = "Túi xách đã được thêm thành công!" });
                }
                return BadRequest("Failed to create TuiXach.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateTuiXach: {ex.Message}");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [Route("get-img/{fileName}")]
        [HttpGet] 
        public IActionResult getImg(string fileName)
        {
            try
            {
                string path = Path.Combine("D:\\DOAN2_API\\BackEnd\\WebTuiXachh\\images", fileName);

                if (!System.IO.File.Exists(path))
                {
                    throw new FileNotFoundException("File không tồn tại.");
                }
                var memoryStream = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    stream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0;
                IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/octet-stream"
                };
                return File(formFile.OpenReadStream(), formFile.ContentType, formFile.FileName);
            }
            catch
            {
                return null;
            }
        }

        // API: Cập nhật thông tin túi xách
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] TuiXachModel model, IFormFile? hinhAnhFile)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                // Kiểm tra hình ảnh và lưu hình ảnh nếu có
                if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{hinhAnhFile.FileName}";
                    var filePath = Path.Combine(@"D:\DOAN2_API\BackEnd\WebTuiXachh\images", uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhFile.CopyToAsync(stream);
                    }
                    model.HinhAnh = uniqueFileName;  // Gán tên hình ảnh cho model
                }

                var result = _tuiXachBusiness.UpdateTuiXach(model, hinhAnhFile);
                if (result)
                {
                    return Ok(new { Success = true, Message = "Túi xách đã được cập nhật thành công!" });
                }
                return BadRequest("Failed to update TuiXach.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateTuiXach: {ex.Message}");
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // API: Xóa sản phẩm túi xách theo mã sản phẩm
        [HttpDelete("delete/{maSp}")]
        public IActionResult Delete(string maSp)
        {
            try
            {
                var result = _tuiXachBusiness.DeleteTuiXach(maSp);
                if (result)
                {
                    return Ok();  // Trả về OK nếu xóa thành công
                }
                return BadRequest("Failed to delete TuiXach.");  // Trả về BadRequest nếu có lỗi
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDataAll: {ex.Message}");
                throw;  // Ném lại lỗi để giữ nguyên thông tin
            }
        }
        public class TuiXachSearchRequest : PagedRequest
        {
            public string MaDanhMuc { get; set; }
            public string MaSp { get; set; }
            public string TenSp { get; set; }
            public string TenMau { get; set; }
            public string MaSize { get; set; }
            public decimal? GiaBanMin { get; set; }
            public decimal? GiaBanMax { get; set; }



        }
        [HttpPost("search")]
        public IActionResult SearchTuiXachs([FromBody] TuiXachSearchRequest searchRequest)
        {
            try
            {
                // Lấy dữ liệu từ request body
                int pageIndex = searchRequest?.PageIndex ?? 1; // Giá trị mặc định là 1
                int pageSize = searchRequest?.PageSize ?? 10; // Giá trị mặc định là 10
                string madanhmuc = searchRequest?.MaDanhMuc ?? string.Empty;
                string masp = searchRequest?.MaSp ?? string.Empty;
                string tensp = searchRequest?.TenSp ?? string.Empty;
                string tenmau = searchRequest?.TenMau ?? string.Empty;
                string masize = searchRequest?.MaSize ?? string.Empty;
                decimal? giabanMin = searchRequest?.GiaBanMin;
                decimal? giabanMax = searchRequest?.GiaBanMax;

                // Gọi tầng business để tìm kiếm
                var tuiXachs = _tuiXachBusiness.SearchTuiXachs(pageIndex, pageSize, out long total, madanhmuc, masp, tensp, tenmau, masize, giabanMin, giabanMax);

                // Tạo đối tượng kết quả phân trang
                var result = new PagedResult<TuiXachModel>
                {
                    Success = true,
                    TotalRecords = total,
                    TotalPages = (int)Math.Ceiling((double)total / pageSize),
                    CurrentPage = pageIndex,
                    Data = tuiXachs ?? new List<TuiXachModel>() // Nếu không có dữ liệu, trả về danh sách rỗng
                };

                // Trả về kết quả
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ và trả về lỗi
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Lỗi khi tìm kiếm túi xách",
                    Details = ex.Message
                });
            }
        }


        // API: Tìm kiếm túi xách
        //[HttpGet("search")]
        //public IActionResult Search(int pageIndex, int pageSize, string searchCriteria, string color, string size, decimal? minPrice, decimal? maxPrice)
        //{
        //    try
        //    {
        //        long total;
        //        var sizes = _tuiXachBusiness.SearchTuiXachs(pageIndex, pageSize, out total, searchCriteria, color, size, minPrice, maxPrice);
        //        return Ok(new { Total = total, Data = sizes });  // Trả về kết quả tìm kiếm với tổng số
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in GetDataAll: {ex.Message}");
        //        throw;  // Ném lại lỗi để giữ nguyên thông tin
        //    }
        //}

    }
}
 