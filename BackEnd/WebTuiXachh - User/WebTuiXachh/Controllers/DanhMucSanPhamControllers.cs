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
        

    }
}
