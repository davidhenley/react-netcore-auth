using System;
using Blog.API.Services;
using Blog.API.ViewModels;
using Blog.Data.Abstract;
using Blog.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("[action]")]
        public ActionResult<AuthData> Login(LoginDto data)
        {
            var user = _userRepository.GetSingle(u => u.Email == data.Email);

            if (user == null || !_authService.VerifyPassword(data.Password, user.Password))
            {
                return BadRequest(new { error = "Username and password do not exist" });
            }

            return _authService.GetAuthData(user.Id);
        }

        [HttpPost("[action]")]
        public ActionResult<AuthData> Register(RegisterDto data)
        {
            var emailUniq = _userRepository.IsEmailUniq(data.Email);
            var usernameUniq = _userRepository.IsUsernameUniq(data.Username);

            if (!emailUniq)
            {
                return BadRequest(new { error = "A user with that email already exists" });
            }

            if (!usernameUniq)
            {
                return BadRequest(new { error = "A user with that username already exists" });
            }

            var id = Guid.NewGuid().ToString();

            var user = new User
            {
                Id = id,
                Username = data.Username,
                Email = data.Email,
                Password = data.Password
            };

            _userRepository.Add(user);
            _userRepository.Commit();

            return _authService.GetAuthData(id);
        }
    }
}
