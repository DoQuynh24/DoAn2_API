using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    //[Authorize] // Bảo vệ tất cả các API trong controller này
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpPost("create-user")]
        [AllowAnonymous]
        public IActionResult CreateUser([FromForm] UserModel model)
        {
            if (model == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {

                model.GioiTinh ??= "Chưa cập nhật";
                model.DiaChi ??= "Chưa cập nhật";
                model.Role = string.IsNullOrEmpty(model.Role) ? "Khách hàng" : model.Role;

                var isSuccess = _userBusiness.CreateUser(model); 
                if (isSuccess)
                {
                    return Ok(new { Success = true, Message = "Tạo người dùng thành công.", User = model });
                }
                return BadRequest("Không thể tạo người dùng.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-user")]
        public IActionResult UpdateUser([FromForm] UserModel model)
        {
            if (model == null || model.PerID <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ hoặc thiếu ID người dùng.");
            }

            try
            {
               
                if (!User.IsInRole("Admin"))
                {
                    model.Role = null; 
                }
               
                var isSuccess = _userBusiness.UpdateUser(model);
                if (isSuccess)
                {
                    return Ok(new { Success = true, Message = "Cập nhật người dùng thành công.", User = model });
                }
                return BadRequest("Không thể cập nhật người dùng.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // API: Xóa người dùng
        [HttpDelete("delete-user/{perId}")]
        public IActionResult DeleteUser(int perId)
        {
            try
            {
                var isSuccess = _userBusiness.DeleteUser(perId);
                if (isSuccess)
                {
                    return Ok(new { Success = true, Message = "Người dùng đã được xóa thành công." });
                }
                return NotFound(new { message = "Người dùng không tồn tại" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi", details = ex.Message });
            }
        }

        // API: Lấy thông tin người dùng theo PerID
        [HttpGet("get-user/{perId}")]
        public IActionResult GetUser(int perId)
        {
            try
            {
                var user = _userBusiness.GetUserById(perId);
                if (user == null)
                {
                    return NotFound(new { message = "Người dùng không tồn tại" });
                }
                return Ok(new { Success = true, User = user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi", details = ex.Message });
            }
        }


        // API: Đăng nhập
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthenticateModel model)
        {
            try
            {
                var user = _userBusiness.Authenticate(model.TaiKhoan, model.MatKhau);
                if (user == null)
                {
                    return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không chính xác" });
                }

                var token = GenerateJwtToken(user); 

                return Ok(new
                {
                    user_id = user.PerID,
                    hoten = user.HoTen,
                    taikhoan = user.TaiKhoan,
                    role = user.Role,
                    token = token // Trả về token JWT
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi", details = ex.Message });
            }
        }

        // Phương thức tạo token JWT
        private string GenerateJwtToken(UserModel user)
        {
            var key = "your-super-secret-key-that-is-longer-than-32-characters";
            var issuer = "your-issuer";
            var audience = "your-audience";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.PerID.ToString()),
                new Claim(ClaimTypes.Name, user.TaiKhoan),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
