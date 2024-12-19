using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Model;
using CA_Final_Persons_Reg_Sys.Repositories;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CA_Final_Persons_Reg_Sys.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IUserMapper _userMapper;
        private readonly IJwtService _jwtService;

        public UsersController(IUserService userService, IPictureService pictureService, IUserMapper userMapper, IJwtService jwtService)
        {
            _userService = userService;
            _pictureService = pictureService;
            _userMapper = userMapper;
            _jwtService = jwtService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResult>>> GetAllUsers()
        {   
            var result = await _userService.GetUsers();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Password and/or user name can not be empty.");
            var result = await _userService.Login(request);
            if(result == null)
                return Unauthorized("Not authorized");

            var token = _jwtService.GetJwtToken(result.Id, result.UserName, result.Role);

            return Ok(new { token });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<long>> CreateUser([FromForm] string requestStr, FileUploadRequest picture)  //[FromForm] FileUploadRequest picture, 
        {
            if (string.IsNullOrEmpty(requestStr))
                return BadRequest();
            var request = JsonConvert.DeserializeObject<UserCreateRequest>(requestStr); //NuGet package: Newtonsoft.Json
            if (request == null)
                return BadRequest("User request is null");

            if (request.userPersonalDataRequest == null)
                return BadRequest("User personal data is null");

            var picturePath = await _pictureService.CreatePicture(picture);
            if (picturePath == null)
                return StatusCode(500, "Internal error");

            request.userPersonalDataRequest.ProfilePicture = picturePath;
            var userId = await _userService.CreateAsync(request);
            if (userId == null)
                return StatusCode(500, "Internal error");

            return Created(string.Empty, userId);
        }
        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResult>> GetUserById(long id)
        {
            if(id <= 0)
                return BadRequest();
            var existingUser = await _userService.GetById(id);

            if (existingUser == null)
                return NotFound($"User not found");

            
            return _userMapper.MapResult(existingUser);
        }

        //[Authorize(Roles = "user,admin")] //neimplementuotas tokenų siuntimas prisijungus prie paskyros, nerodo nuotraukos
        [HttpGet("{id}/Picture")]
        public async Task<IActionResult> GetPicture([FromRoute] long id)
        {
            if (id <= 0)
                return BadRequest();

            var pictureUrl = await _userService.GetUserPictureUrlByUserId(id);
            if (pictureUrl == null)
                return NotFound("User or picture not found");

            var fileData = _pictureService.GetPictureByFileName(pictureUrl);
            if (fileData == null || fileData.Length == 0)
                return StatusCode(500, "Could not retrieve picture");

            var contentType = _pictureService.GetContentTypeByFileName(pictureUrl);
            if (contentType == null)
                return StatusCode(500, "Could not determine content type");

            return File(fileData, contentType);
        }

        [Consumes("multipart/form-data")]
        [HttpPut("{id}/Picture")]
        public async Task<IActionResult> UpdateUserPicture([FromRoute] long id, [FromForm] FileUploadRequest request)
        {
            if (id <= 0)
                return BadRequest();

            if (request == null)
                return BadRequest("No file uploaded.");

            //delete current file after succesful upload
            var picturePath = await _pictureService.CreatePicture(request);
            if (picturePath == null)
                return StatusCode(500, "Failed to upload picture");
            
            var user = await _userService.GetById(id);
            if (user == null)
                return BadRequest("User not found");

            await _userService.UpdateUserPicture(id, picturePath);

            return Ok(new { message = "File uploaded successfully", picturePath });
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/Password")]
        public async Task<IActionResult> UpdateUserPassword([FromRoute] long id, [FromBody] string newPassword)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");

            if(!await _userService.UpdateUserPassword(id, newPassword))
                return StatusCode(500, "Failed to update password");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/Name")]
        public async Task<IActionResult> UpdateUserName([FromRoute] long id, [FromBody] string newName)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");

            if (!await _userService.UpdateUserName(id, newName))
                return StatusCode(500, "Failed to update name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/LastName")]
        public async Task<IActionResult> UpdateUserLastName([FromRoute] long id, [FromBody] string newLastName)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserLastName(id, newLastName))
                return StatusCode(500, "Failed to update last name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/PersonalCode")]
        public async Task<IActionResult> UpdateUserPersonalCode([FromRoute] long id, [FromBody] string newPersonalCode)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserPersonalCode(id, newPersonalCode))
                return StatusCode(500, "Failed to update personal code");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/PhoneNumber")]
        public async Task<IActionResult> UpdateUserPhoneNumber([FromRoute] long id, [FromBody] string newPhoneNumber)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserPhoneNumber(id, newPhoneNumber))
                return StatusCode(500, "Failed to update phone number");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/Email")]
        public async Task<IActionResult> UpdateUserEmail([FromRoute] long id, [FromBody] string newEmail)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserEmail(id, newEmail))
                return StatusCode(500, "Failed to update email");

            return NoContent();
        }


        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/CityName")]
        public async Task<IActionResult> UpdateUserCityName([FromRoute] long id, [FromBody] string newCityName)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserCityName(id, newCityName))
                return StatusCode(500, "Failed to update city name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/StreetName")]
        public async Task<IActionResult> UpdateUserStreetName([FromRoute] long id, [FromBody] string newStreetName)
        {
            if (id <= 0)
                return BadRequest();
            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserStreetName(id, newStreetName))
                return StatusCode(500, "Failed to update street name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/HouseNumber")]
        public async Task<IActionResult> UpdateUserHouseNumber([FromRoute] long id, [FromBody] string newHouseNumber)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserHouseNumber(id, newHouseNumber))
                return StatusCode(500, "Failed to update house number");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/ApartmentNumber")]
        public async Task<IActionResult> UpdateUserApartmentNumber([FromRoute] long id, [FromBody] string newApartmentNumber)
        {
            if (id <= 0)
                return BadRequest();

            var existinUser = await _userService.GetById(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userService.UpdateUserApartmentNumber(id, newApartmentNumber))
                return StatusCode(500, "Failed to update apartment number");

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(long id)
        {
            if (id <= 0)
                return BadRequest();
            _userService.DeleteUser(id);
            
            return NoContent();
        }

        
    }
}
