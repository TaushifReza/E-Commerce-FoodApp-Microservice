using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto?> GetAllCouponAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(string couponDto);
        Task<ResponseDto?> UpdateCouponAsync(string couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
