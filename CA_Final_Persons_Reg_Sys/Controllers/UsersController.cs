using CA_Final_Persons_Reg_Sys.Dtos;
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
        private readonly IUserService

        private readonly IUserRepository _userRepository;
        private readonly IUserMapper _userMapper;
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IJwtService _jwtService;
        private readonly IPictureRepository _pictureRepository;

        public UsersController(IUserRepository userRepository, IUserMapper userMapper, IUserAuthenticationService userAuthenticationService, IJwtService jwtService, IPictureRepository pictureRepository)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
            _userAuthenticationService = userAuthenticationService;
            _jwtService = jwtService;
            _pictureRepository = pictureRepository;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResult>>> GetAllUsers()
        {   
            var result = await _userRepository.GetAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var existingUser = await _userRepository.GetByUserName(request.UserName);
            if (existingUser == null)
                return NotFound(new { message = $"User {request.UserName} not found." });

            if(await _userAuthenticationService.Login(request.UserName, request.Password))
            {
                var jwtToken = _jwtService.GetJwtToken(existingUser.Id, request.UserName, existingUser.Role);
                return Ok(new { token = jwtToken });
            }
            return BadRequest(new { message = "Username or password incorrect." });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<long>> CreateUser([FromForm] string requestStr, [FromForm] FileUploadRequest picture)  //[FromForm] FileUploadRequest picture, 
        {
            var request = JsonConvert.DeserializeObject<UserCreateRequest>(requestStr); //NuGet package: Newtonsoft.Json
            if (request == null)
                return BadRequest("User request is null");

            if (request.userPersonalDataRequest == null)
                return BadRequest("User personal data is null");

            //check if user with such name exists
            if (await _userRepository.GetByUserName(request.UserName) != null)
                return BadRequest("User with such Username already exists.");   //Returns 409 Conflict for an already existing username, which is semantically accurate.

            var picturePath = await _pictureRepository.Create(picture);
            if (picturePath == null)
                return StatusCode(500, "Failed to upload picture.");

            _userAuthenticationService.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
            var newUser = _userMapper.MapToEntity(request, passwordHash, passwordSalt);

            //assign file path as string to user
            newUser.UserPersonalData.ProfilePicture = picturePath;

            var userId = await _userRepository.CreateAsync(newUser);
            return Created(string.Empty, userId);
        }
        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResult>> GetUserById(long id)
        {
            var userIdStr = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!long.TryParse(userIdStr, out var idSub))
                return BadRequest("Invalid token");
            if (idSub != id)
                return Unauthorized();

            var existingUser = await _userRepository
                .GetByIdAsync(id);

            if(existingUser == null)
                return NotFound($"User not found. Id {id}");

            return Ok(existingUser);
        }

        [Authorize(Roles = "user,admin")]
        [HttpGet("{id}/Picture")]
        public async Task<IActionResult> GetPicture([FromRoute] long id)
        {
            var uploadPath = "uploads/";    //JSON!!!!
            string? fileName = await _userRepository.GetUserPictureUrl(id);
            if (fileName == null)
                return StatusCode(500, "Unable to retrieve file");

            //try
            var filePath = Path.Combine(uploadPath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            string contentType;
            switch (fileExtension)  //JSON!!!!!
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                default:
                    return BadRequest("Unsupported file type");
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/Password")]
        public async Task<IActionResult> UpdateUserPassword([FromRoute] long id, [FromBody] string newPassword)
        {
            {
                var existinUser = await _userRepository.GetByIdAsync(id);
                if (existinUser == null)
                    return NotFound("User not found");

                _userAuthenticationService.CreatePasswordHash(newPassword, out var passwordHash, out var passwordSalt);
                if (!await _userRepository.UpdateUserPassword(id, passwordHash, passwordSalt))
                    return StatusCode(500, "Failed to update password");

                return NoContent();
            }
        }

        [Authorize(Roles = "user,admin")] //403 forbidden
        [HttpPut("{id}/Name")]
        public async Task<IActionResult> UpdateUserName([FromRoute] long id, [FromBody] string newName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existinUser = await _userRepository.GetByIdAsync(id);
            if(existinUser == null)
                return NotFound("User not found");
            
            if(!await _userRepository.UpdateUserName(id, newName))
                return StatusCode(500, "Failed to update name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/LastName")]
        public async Task<IActionResult> UpdateUserLastName([FromRoute] long id, [FromBody] string newLastName)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserLastName(id, newLastName))
                return StatusCode(500, "Failed to update last name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/PersonalCode")]
        public async Task<IActionResult> UpdateUserPersonalCode([FromRoute] long id, [FromBody] string newPersonalCode)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserPersonalCode(id, newPersonalCode))
                return StatusCode(500, "Failed to update personal code");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/PhoneNumber")]
        public async Task<IActionResult> UpdateUserPhoneNumber([FromRoute] long id, [FromBody] string newPhoneNumber)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserPhoneNumber(id, newPhoneNumber))
                return StatusCode(500, "Failed to update phone number");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/Email")]
        public async Task<IActionResult> UpdateUserEmail([FromRoute] long id, [FromBody] string newEmail)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserEmail(id, newEmail))
                return StatusCode(500, "Failed to update email");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [Consumes("multipart/form-data")]
        [HttpPut("{id}/Picture")]
        public async Task<IActionResult> UpdateUserPicture([FromRoute] long id, [FromForm] FileUploadRequest request)
        {
            if (request == null)
                return BadRequest("No file uploaded.");

            var oldPictureUrl = await _userRepository.GetUserPictureUrl(id);
            if (oldPictureUrl == null)
                return StatusCode(500, "Failed to update picture");
         
            var picturePath = await _pictureRepository.Update(request, oldPictureUrl);

            if (picturePath == null)
                return BadRequest("Failed to upload picture");

            if (!await _userRepository.UpdateUserPicture(id, picturePath))
                return StatusCode(500, "Failed to upload picture");


            return Ok(new { message = "File uploaded successfully", picturePath });
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/CityName")]
        public async Task<IActionResult> UpdateUserCityName([FromRoute] long id, [FromBody] string newCityName)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserCityName(id, newCityName))
                return StatusCode(500, "Failed to update city name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/StreetName")]
        public async Task<IActionResult> UpdateUserStreetName([FromRoute] long id, [FromBody] string newStreetName)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserStreetName(id, newStreetName))
                return StatusCode(500, "Failed to update street name");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/HouseNumber")]
        public async Task<IActionResult> UpdateUserHouseNumber([FromRoute] long id, [FromBody] string newHouseNumber)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserHouseNumber(id, newHouseNumber))
                return StatusCode(500, "Failed to update house number");

            return NoContent();
        }

        [Authorize(Roles = "user,admin")]
        [HttpPut("{id}/ApartmentNumber")]
        public async Task<IActionResult> UpdateUserApartmentNumber([FromRoute] long id, [FromBody] string newApartmentNumber)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if (existinUser == null)
                return NotFound("User not found");
            if (!await _userRepository.UpdateUserApartmentNumber(id, newApartmentNumber))
                return StatusCode(500, "Failed to update apartment number");

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            bool success = await _userRepository.DeleteAsync(id);

            if (!success) return NotFound($"User not found {id}");

            return NoContent();
        }

        
    }
}
