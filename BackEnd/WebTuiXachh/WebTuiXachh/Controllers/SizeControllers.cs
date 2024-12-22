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

        // API: Lấy kích cỡ theo mã size
        [HttpGet("get-by-id/{maSize}")]
        public IActionResult GetById(string maSize)
        {
            try
            {
                var size = _sizeBusiness.GetSizeById(maSize);
                if (size == null)
                {
                    return NotFound();  
                }
                return Ok(size);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);  
            }
        }

        // API: Thêm kích cỡ mới
        [HttpPost("create")]
        public IActionResult Create([FromBody] SizeModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid size data");  
                }

                var result = _sizeBusiness.CreateSize(model);
                if (result)
                {
                    return Ok("Size created successfully");  
                }
                return StatusCode(500, "Internal server error: Could not create size");  
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);  
            }
        }

        // API: Cập nhật kích cỡ
        [HttpPut("update")]
        public IActionResult Update([FromBody] SizeModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.MaSize))
                {
                    return BadRequest("Invalid size data");  
                }

                var result = _sizeBusiness.UpdateSize(model);
                if (result)
                {
                    return Ok("Size updated successfully");  
                }
                return StatusCode(500, "Internal server error: Could not update size");  
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message); 
            }
        }

        // API: Xóa kích cỡ
        [HttpDelete("delete/{maSize}")]
        public IActionResult Delete(string maSize)
        {
            try
            {
                if (string.IsNullOrEmpty(maSize))
                {
                    return BadRequest("Invalid size ID");  
                }

                var result = _sizeBusiness.DeleteSize(maSize);
                if (result)
                {
                    return Ok("Size deleted successfully");  
                }
                return StatusCode(500, "Internal server error: Could not delete size");  
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
