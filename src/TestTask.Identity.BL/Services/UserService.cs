using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TestTask.Identity.BL.DTOs;
using TestTask.Identity.Common.Exceptions;
using TestTask.Identity.DAL.Entities;

namespace TestTask.Identity.BL.Services
{
    public interface IUserService
	{
		Task<UserToReturnDTO> Register(UserForRegisterDTO userForRegisterDto);
		Task<UserDataDTO> Login(UserForLoginDTO userForLoginDto);
		Task Logout();
		List<Claim> GetClaims(User user);

		Task<User> GetCurrentUser(ClaimsPrincipal userProperty);
	}

	public class UserService : IUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IJwtTokenService _jwtTokenService;

		public UserService(
			UserManager<User> userManager,
			SignInManager<User> signinManager,
			IJwtTokenService jwtTokenService)
		{
			_userManager = userManager;
			_signInManager = signinManager;
			_jwtTokenService = jwtTokenService;
		}

		public async Task<UserToReturnDTO> Register(UserForRegisterDTO userForRegisterDto)
		{
			var user = new User
			{
				UserName = userForRegisterDto.UserName,
				CreatedAt = userForRegisterDto.Created
			};

			var result = await _userManager.CreateAsync(user, userForRegisterDto.Password);

			if (result.Succeeded)
			{
				return new UserToReturnDTO
				{
					Id = user.Id,
					UserName = user.UserName
				};
			}

			throw new BusinessLogicException("Unable to createw user at the moment. Try again later");
		}

		public async Task<UserDataDTO> Login(UserForLoginDTO userForLoginDto)
		{
			var user = await _userManager.FindByNameAsync(userForLoginDto.UserName);

			var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

			if (result.Succeeded)
			{
				var appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == userForLoginDto.UserName.ToUpper());

				var claims = GetClaims(user);

				return new UserDataDTO
				{
					UserToReturnDTO = new UserToReturnDTO {
						Id = user.Id,
						UserName = user.UserName
					},
					Token = await _jwtTokenService.GenerateJwtToken(claims)
				};
			}

			throw new UnauthorizedException("Invalid creidentionals");
		}


		public List<Claim> GetClaims(User user)
		{
			var claims = new List<Claim>
			{
				new Claim("userName", user.UserName)
			};

			return claims;
		}

		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		}
		public async Task<User> GetCurrentUser(ClaimsPrincipal userProperty)
		{
			var user = await _userManager.GetUserAsync(userProperty);
			return ToDto(user);
		}

		internal static User ToDto(User user)
		{
			if (user == null) return null;
			return new User
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName,
				//PictureUrl = user.PictureUrl
			};
		}
	}
}
