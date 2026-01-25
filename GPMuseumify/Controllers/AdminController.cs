//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace GPMuseumify.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[Authorize(Policy = "AdminOnly")] // Only Admins can access
//public class AdminController : ControllerBase
//{
//    [HttpGet("dashboard")]
//    public IActionResult GetDashboard()
//    {
//        return Ok(new { message = "Welcome to Admin Dashboard", role = "Admin" });
//    }

//    [HttpGet("users")]
//    public IActionResult GetAllUsers()
//    {
//        // TODO: Implement get all users
//        return Ok(new { message = "Get all users - Admin only" });
//    }
//}