using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using perAPI.Data;
using perAPI.Dto.User;
using perAPI.Models;
using perAPI.Services;

namespace perAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly TokenService _tokenService;

    public AuthController(DataContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserInfoDto>> Register(UserRegisterDto dto)
    {
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email.ToLower()))
            return BadRequest("Ese correo ya está registrado.");

        if (!Enum.TryParse<Role>(dto.Role, true, out var parsedRole))
            return BadRequest("Rol inválido. Usa 'User', 'Coach' o 'Admin'.");

        using var hmac = new HMACSHA512();

        var user = new User
        {
            Email = dto.Email.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
            PasswordSalt = hmac.Key,
            Role = parsedRole
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserInfoDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserInfoDto>> Login(UserLoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email.ToLower());
        if (user == null) return Unauthorized("Usuario no encontrado.");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        for (int i = 0; i < hash.Length; i++)
            if (hash[i] != user.PasswordHash[i])
                return Unauthorized("Contraseña incorrecta.");

        return new UserInfoDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role.ToString(),
            Token = _tokenService.CreateToken(user)
        };
    }
}