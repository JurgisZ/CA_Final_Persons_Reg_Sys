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

        [HttpGet("Users/{id}")]
        public async Task<ActionResult<UserResult>> GetUserById(long id)
        {
            var existingUser = await _userRepository
                .GetByIdAsync(id);

            if(existingUser == null)
                return NotFound($"User not found. Id {id}");

            return Ok(existingUser);
        }

        [HttpPut("Users/{id}")]
        public async Task<IActionResult> UpdateUser(long id, [FromBody] UserRequest newUserData)
        {
            bool success = await _userRepository.UpdateAsync(id, newUserData);

            if (!success) return StatusCode(500, "Internal error while updating user.");

            return NoContent(); //success
        }

        [HttpDelete("Users/{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            bool success = await _userRepository.DeleteAsync(id);

            if (!success) return NotFound($"User not found {id}");

            return NoContent(); //success
        }
        /*
        [HttpPut("Users/{id}/UserPersonalData/Name")]
        public async Task<IActionResult> UpdatePersonalData (long id, [FromBody] string newName)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();    //user not found!

            //validacijos
            if (string.IsNullOrEmpty(newName))
                return BadRequest("Name can not be empty.");

            existingUser.UserPersonalData.Name = newName;
            bool updateSuccess = await _repository.UpdateAsync(existingUser);
            if (updateSuccess)
                return NoContent(); //success
            return StatusCode(500, "An error occured while updating User");
        }
        */
        [HttpPut("Users/{id}/UserPersonalData/Name")]
        public async Task<IActionResult> UpdatePersonalData (long id, [FromBody] string newName)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();    //user not found!

            //validacijos
            if (string.IsNullOrEmpty(newName))
                return BadRequest("Name can not be empty.");

            existingUser.UserPersonalData.Name = newName;
            bool updateSuccess = await _userRepository.UpdatePersonalDataPropertyAsync(existingUser.Id, nameof(existingUser.Id), newName);
            if (updateSuccess)
                return NoContent(); //success
            return StatusCode(500, "An error occured while updating User");
        }
    }
}
