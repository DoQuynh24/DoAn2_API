﻿using System;
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

        // API: Tìm kiếm túi xách
        [HttpGet("search")]
        public IActionResult Search(int pageIndex, int pageSize, string searchCriteria, string color, string size, decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                long total;
                var sizes = _tuiXachBusiness.SearchTuiXachs(pageIndex, pageSize, out total, searchCriteria, color, size, minPrice, maxPrice);
                return Ok(new { Total = total, Data = sizes });  // Trả về kết quả tìm kiếm với tổng số
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDataAll: {ex.Message}");
                throw;  // Ném lại lỗi để giữ nguyên thông tin
            }
        }
        

    }
}
 