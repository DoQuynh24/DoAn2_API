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
using System.Xml.Linq;

namespace API.Controllers
{
    //[Authorize] // Bảo vệ tất cả các API trong controller này
    [ApiController]
    [Route("api/[controller]")]
    public class DanhMucSanPhamController : ControllerBase
    {
        private readonly IDanhMucSanPhamBusiness _danhMucSanPhamBusiness;

        // Constructor injection của DanhMucSanPhamBusiness
        public DanhMucSanPhamController(IDanhMucSanPhamBusiness danhMucSanPhamBusiness)
        {
            _danhMucSanPhamBusiness = danhMucSanPhamBusiness;
        }

        // API: POST /api/danhmucsanpham (Tạo mới danh mục sản phẩm)
        [HttpPost("create")]

        public IActionResult Create([FromForm] DanhMucSanPhamModel model)
        {
            try
            {
                if (_danhMucSanPhamBusiness.CreateDanhMuc(model))
                {
                    return Ok(new { Success = true, Message = "Danh mục sản phẩm đã được tạo thành công!" });
                }
                return BadRequest("Không thể tạo danh mục sản phẩm.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }

        // API: PUT /api/danhmucsanpham/update (Cập nhật danh mục sản phẩm)
        [HttpPut("update")]
       
        public IActionResult Update([FromForm] DanhMucSanPhamModel model)
        {
            try
            {
                if (_danhMucSanPhamBusiness.UpdateDanhMuc(model))
                {
                    return Ok(new { Success = true, Message = "Danh mục sản phẩm đã được cập nhật thành công!" });
                }
                return BadRequest("Không thể cập nhật danh mục sản phẩm.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }

        // API: DELETE /api/danhmucsanpham/delete/{maDanhMuc} (Xóa danh mục sản phẩm)
        [HttpDelete("delete/{maDanhMuc}")]
      
        public IActionResult Delete(string maDanhMuc)
        {
            try
            {
                if (_danhMucSanPhamBusiness.DeleteDanhMuc(maDanhMuc))
                {
                    return Ok(new { Success = true, Message = "Danh mục sản phẩm đã được xóa thành công!" });
                }
                return BadRequest("Không thể xóa danh mục sản phẩm.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }

        // API: GET /api/danhmucsanpham/{maDanhMuc} (Lấy thông tin danh mục sản phẩm theo mã danh mục)
        [HttpGet("{maDanhMuc}")]
        public IActionResult Get(string maDanhMuc)
        {
            try
            {
                var danhMuc = _danhMucSanPhamBusiness.GetDanhMucById(maDanhMuc);
                if (danhMuc != null)
                {
                    return Ok(danhMuc);
                }
                return NotFound(); // Nếu không tìm thấy danh mục sản phẩm
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }

        // API: GET /api/danhmucsanpham/all (Lấy tất cả danh mục sản phẩm)
        [HttpGet("all")]
      
        public IActionResult GetAll()
        {
            try
            {
                var danhMucs = _danhMucSanPhamBusiness.GetAllDanhMuc();
                return Ok(danhMucs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }
        // API: GET /api/danhmucsanpham/search (Tìm kiếm danh mục sản phẩm theo các tiêu chí)
        [HttpGet("search")]
        public IActionResult Search(int pageIndex = 1, int pageSize = 10, string maDanhMuc = "", string tenDanhMuc = "")
        {
            try
            {
                long total;
                var danhMucs = _danhMucSanPhamBusiness.SearchDanhMucs(pageIndex, pageSize, out total, maDanhMuc, tenDanhMuc);

                if (danhMucs != null && danhMucs.Count > 0)
                {
                    return Ok(new { Total = total, Data = danhMucs });
                }
                return NotFound(new { message = "Không tìm thấy danh mục sản phẩm phù hợp" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


    }
}
