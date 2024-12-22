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
    public class MauSacController : ControllerBase
    {
        private readonly IMauSacBusiness _mauSacBusiness;

       
        public MauSacController(IMauSacBusiness mauSacBusiness)
        {
            _mauSacBusiness = mauSacBusiness;
        }

       
     
      
        [HttpGet("all")]

        public IActionResult GetAll()
        {
            try
            {
                var tenMaus = _mauSacBusiness.GetAllMauSac();
                return Ok(tenMaus);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); // Lỗi server
            }
        }
       
        [HttpGet("search")]
        public IActionResult Search(int pageIndex = 1, int pageSize = 10, string tenMau = "")
        {
            try
            {
                long total;
                var mauSacs = _mauSacBusiness.SearchMauSacs(pageIndex, pageSize, out total, tenMau);

                if (mauSacs != null && mauSacs.Count > 0)
                {
                    return Ok(new { Total = total, Data = mauSacs });
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
