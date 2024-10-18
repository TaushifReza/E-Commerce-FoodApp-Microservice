﻿using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Models.Dtos;

namespace Mango.Services.EmailAPI.Services.IService
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task RegisterUserEmailAndLog(string email);
        Task LogOrderPlaced(RewardsMessage rewardsMessage);
    }
}
