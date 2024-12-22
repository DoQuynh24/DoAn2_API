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
    public class SizeController : ControllerBase
    {
        private readonly ISizeBusiness _sizeBusiness;

        public SizeController(ISizeBusiness sizeBusiness)
        {
            _sizeBusiness = sizeBusiness;
        }

        // API: Lấy tất cả kích cỡ
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            try
            {
                var sizes = _sizeBusiness.GetAllSizes();
                return Ok(sizes);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);  
            }
        }

     
        // API: Tìm kiếm kích cỡ
        [HttpGet("search")]
        public IActionResult Search(int pageIndex, int pageSize, string searchCriteria)
        {
            try
            {
                long total;
                var sizes = _sizeBusiness.SearchSizes(pageIndex, pageSize, out total, searchCriteria);
                return Ok(new { Total = total, Data = sizes });  
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); 
            }
        }
    }

}
