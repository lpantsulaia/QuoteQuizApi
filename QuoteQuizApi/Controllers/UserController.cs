using AutoMapper;
using DataModels.DTOs;
using DataModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuoteQuizApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuoteQuizApi.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;

        }
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Authenticate(string userName, string password)
        {
            var user = await _unitOfWork.UserRepository.Authenticate(userName, password);
            if (user == null)
                return Unauthorized();

            var userDto = _mapper.Map<UserDto>(user);




            var Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),

            });
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = Subject,
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                userDto,
                Token = tokenString
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);


            try
            {
                var userInDb = await _unitOfWork.UserRepository.Create(user, userDto.Password);
                return CreatedAtAction(nameof(GetById), new { id = userInDb.Id }, userInDb.Id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            Request.HttpContext.Response.Headers.Add("Total-Count", users.Count().ToString());

            return Ok(usersDto);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserDto userDto)
        {
            // map dto to entity and set id
            var userToBeUpdatedInDb = _unitOfWork.UserRepository.GetAsync(userDto.Id);
            if (userToBeUpdatedInDb == null)
            {
                return BadRequest("User was not found");
            }
            var userToBeUpdated = _mapper.Map<User>(userDto);

            try
            {
                // save
                await _unitOfWork.UserRepository.Update(userToBeUpdated, userDto.Password);
                return Accepted();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("[action]/{id}")]
        
        public async Task<IActionResult> Disable(int id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("User was not found");
            }
            user.IsDisabled = true;
            await _unitOfWork.SaveChangesAsync();
            return Accepted();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("User was not found");
            }
            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

    }
}
