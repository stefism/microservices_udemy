﻿using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserAsync(string userId);
        Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
    }
}
