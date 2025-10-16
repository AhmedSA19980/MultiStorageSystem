using BlobStorage.Service;
using BlobStorage.Service.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlobStorage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserService _userService;

        public UserController(UserService userService) {
        
        
            _userService = userService; 
        }


        [Authorize]
        [HttpGet("retrieveUser", Name = "retrieveUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Please enter a valid  ID {id}");
            }

          var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                // Return a 404 Not Found if the user does not exist
                return NotFound("User is not found");
            }
         
            return Ok(user);

        }

        [HttpPost("Add", Name = "AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>  AddUser(CreateUserDTO userDTO)
        {

            try
            {
               
                if (userDTO == null || string.IsNullOrEmpty(userDTO.Username) || string.IsNullOrEmpty(userDTO.Password) ||
                    string.IsNullOrEmpty(userDTO.Email) || userDTO.DateOfBirth == default(DateTime)

                    )
                {
                    return BadRequest("Invalid input User data.");
                }


               int newUser =  await _userService.AddAsync(userDTO);
                return CreatedAtRoute(
                     "AddUser",
                     new { Id = newUser }, 
                     new
                     {
                         Id = newUser,
                         Username = userDTO.Username,
                         Email = userDTO.Email,
                         DateOfBirth = userDTO.DateOfBirth
                     }
                 );

            }
            catch (Exception ex) { 
               if(ex.Message.Contains("Email  is exist"))
                {

                    return Conflict(new { message = ex.Message });
                }

                return StatusCode(500, "An unexpected error occurred.");

            }

        }


        [Authorize]
        [HttpPut("ChangePassword", Name = "ChangePass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO Userdto)
        {

            try
            {
                if (Userdto == null)

                {
                    return BadRequest("userData is Required");
                };


                 string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                if (Convert.ToInt32(userId) != Userdto.ID) return Unauthorized("you're not authorized to update this user, make sure you input your user id number ");



                await _userService.ChangePasswordAsync(Userdto);
                return Ok("Password changed suscessfully ");
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 Not Found , if the user doesn't exist.
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error for any other exceptions.
                return StatusCode(500, "An error occurred while changing the password.");
            }

        }



        [Authorize]
        [HttpPut("UpdateUserById", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(UpdateUserDTO userDTO)
        {

            try
            {
                int UserId = userDTO.ID;
                if (userDTO == null || string.IsNullOrEmpty(userDTO.Username) ||
                       string.IsNullOrEmpty(userDTO.Email))
                {
                    return BadRequest("Invalid User data.");
                }

   

                var currentuserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                if (Convert.ToInt32(currentuserId) != UserId) return Unauthorized("you're not authorized to update this user, make sure you input your user id number ");

                
                await _userService.UpdateAsync(userDTO);
                return Ok(userDTO);
            }
            catch (KeyNotFoundException ex) {

                return NotFound(ex.Message);
                

            }catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

          

           
        }

    }
}
