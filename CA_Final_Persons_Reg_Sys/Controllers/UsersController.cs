using CA_Final_Persons_Reg_Sys.Dtos;
using CA_Final_Persons_Reg_Sys.Repositories.Interfaces;
using CA_Final_Persons_Reg_Sys.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CA_Final_Persons_Reg_Sys.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
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

        //private readonly IUserPersonalDataMapper _userPersonalDataMapper;



        //pvz: [Authorize(Roles = "Admin")]
        [HttpGet("Users")]
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
                var jwtToken = _jwtService.GetJwtToken(request.UserName, existingUser.Role);
                return Ok(new { token = jwtToken });
            }
            return BadRequest(new { message = "Username or password incorrect." });
        }

        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [HttpPost("PhotoUploadTest")]
        public async Task<IActionResult> ApiPostTest([FromForm] FileUploadRequest request)
        { 
            if (request == null)
                return BadRequest("No file uploaded.");
            
            var picturePath = await _pictureRepository.Create(request);

            return Ok(new { message = "File uploaded successfully", picturePath });
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

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResult>> GetUserById(long id)
        {
            var existingUser = await _userRepository
                .GetByIdAsync(id);

            if(existingUser == null)
                return NotFound($"User not found. Id {id}");

            return Ok(existingUser);
        }
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


        [AllowAnonymous]
        [HttpPut("{id}/Name")]
        public async Task<IActionResult> UpdateUserName([FromRoute] long id, [FromBody] string newName)
        {
            var existinUser = await _userRepository.GetByIdAsync(id);
            if(existinUser == null)
                return NotFound("User not found");
            
            if(!await _userRepository.UpdateUserName(id, newName))
                return StatusCode(500, "Failed to update name");

            return NoContent();
        }

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

        [Consumes("multipart/form-data")]
        [HttpPut("{id}/Picture")]
        public async Task<IActionResult> UpdateUserPicture([FromRoute] long id, [FromForm] FileUploadRequest request)
        {
            if (request == null)
                return BadRequest("No file uploaded.");

            //delete current file after succesful upload
            var picturePath = await _pictureRepository.Create(request);
            if (picturePath == null)
                return BadRequest("Failed to upload picture");

            return Ok(new { message = "File uploaded successfully", picturePath });
        }

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



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            bool success = await _userRepository.DeleteAsync(id);

            if (!success) return NotFound($"User not found {id}");

            return NoContent(); //success
        }

        
    }
}
