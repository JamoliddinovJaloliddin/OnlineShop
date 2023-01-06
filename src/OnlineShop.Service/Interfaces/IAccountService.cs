﻿using OnlineShop.Service.Dtos.Accounts;

namespace OnlineShop.Service.Interfaces
{
    public interface IAccountService
    {
        public Task<string> LoginAsync(AccountLoginDto dto);
        public Task<bool> RegisterAsync(AccountRegisterDto dto);
    }
}

