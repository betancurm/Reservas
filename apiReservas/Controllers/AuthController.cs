using apiReservas.DTOs.Identities;
using apiReservas.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace apiReservas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userMgr;
    private readonly RoleManager<ApplicationRole> _roleMgr;
    private readonly IConfiguration _cfg;

    public AuthController(UserManager<ApplicationUser> userMgr, RoleManager<ApplicationRole> roleManager, IConfiguration cfg)
    {
        _userMgr = userMgr;
        _cfg = cfg;
        _roleMgr = roleManager;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        // 1. Crear el usuario
        var user = new ApplicationUser
        {
            UserName = dto.UserName,
            Email = dto.Email
        };

        var result = await _userMgr.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return StatusCode(201, new { user.Id, user.UserName, user.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userMgr.FindByNameAsync(dto.Username);
        if (user == null || !await _userMgr.CheckPasswordAsync(user, dto.Password))
            return Unauthorized();

        var roles = await _userMgr.GetRolesAsync(user);
        var token = GenerateToken(user.UserName!, user.Id, roles);

        return Ok(new { token });
    }

    private string GenerateToken(string username, Guid userId, IList<string> roles)
    {
        var jwt = _cfg.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("uid", userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwt["ExpireMinutes"]!)),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [HttpPost("roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
    {
        // 1. Verificar existencia
        if (await _roleMgr.RoleExistsAsync(dto.RoleName))
            return BadRequest($"El rol '{dto.RoleName}' ya existe.");

        // 2. Crear
        var role = new ApplicationRole { Name = dto.RoleName };
        var result = await _roleMgr.CreateAsync(role);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return StatusCode(201, new { role.Id, role.Name });
    }
}
