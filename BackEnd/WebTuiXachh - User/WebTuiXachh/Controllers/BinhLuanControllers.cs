using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Model;

namespace API.Controllers
{
    //[Authorize] // Bảo vệ tất cả các API trong controller này
    [ApiController]
    [Route("api/[controller]")]
    public class BinhLuanController : ControllerBase
    {
        private readonly IBinhLuanBusiness _binhLuanBusiness;

        public BinhLuanController(IBinhLuanBusiness binhLuanBusiness)
        {
            _binhLuanBusiness = binhLuanBusiness;
        }

        // API: POST /api/binhluan/create (Tạo mới bình luận)
        [HttpPost("create")]
       
        public IActionResult Create([FromBody] BinhLuanModel model)
        {
            try
            {
                if (model.NgayBinhLuan == null)
                {
                    model.NgayBinhLuan = DateTime.Now;
                }
                if (_binhLuanBusiness.CreateBinhLuan(model))
                {
                    return Ok(new { Success = true, Message = "Bình luận đã được tạo thành công!" });
                }
                return BadRequest("Không thể tạo bình luận.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi khi giá sản phẩm.", Error = ex.Message });
            }
        }

        // API: PUT /api/binhluan/update (Cập nhật bình luận)
        [HttpPut("update")]
        public IActionResult Update([FromBody] BinhLuanModel model)
        {
            try
            {
                if (_binhLuanBusiness.UpdateBinhLuan(model))
                {
                    return Ok(new { Success = true, Message = "Bình luận đã được cập nhật thành công!" });
                }
                return BadRequest("Không thể cập nhật bình luận.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

        // API: DELETE /api/binhluan/delete/{id} (Xóa bình luận)
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_binhLuanBusiness.DeleteBinhLuan(id))
                {
                    return Ok(new { Success = true, Message = "Bình luận đã được xóa thành công!" });
                }
                return BadRequest("Không thể xóa bình luận.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

        // API: GET /api/binhluan/{id} (Lấy thông tin bình luận theo ID)
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var binhLuan = _binhLuanBusiness.GetBinhLuanById(id);
                if (binhLuan != null)
                {
                    return Ok(binhLuan);
                }
                return NotFound(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

        // API: GET /api/binhluan/all (Lấy tất cả bình luận)
        [HttpGet("all")]
       
        public IActionResult GetAll()
        {
            try
            {
                var binhLuans = _binhLuanBusiness.GetAllBinhLuan();
                return Ok(binhLuans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex); 
            }
        }

        // API: GET /api/binhluan/search (Tìm kiếm bình luận theo các tiêu chí)
        [HttpGet("search")]
        public IActionResult Search(int pageIndex = 1, int pageSize = 10, string maSP = "", int perID=0 )
        {
            try
            {
                long total;
                var binhLuans = _binhLuanBusiness.Search(pageIndex, pageSize, out total, maSP, perID);

                if (binhLuans != null && binhLuans.Count > 0)
                {
                    return Ok(new { Total = total, Data = binhLuans });
                }
                return NotFound(new { message = "Không tìm thấy bình luận phù hợp" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
