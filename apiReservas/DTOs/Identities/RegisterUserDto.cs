﻿namespace apiReservas.DTOs.Identities;
public class RegisterUserDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    //public string PhoneNumber { get; set; } = null!;
}
