namespace Mango.Services.OrderAPI
{
    public class Helpers
    {
        public static string CouponAPIBase {  get; set; }
        public static string AuthAPIBase {  get; set; }
        public static string ProductAPIBase {  get; set; }
        public static string ShoppingCartAPIBase {  get; set; }
        public static string OrderAPIBase {  get; set; }

        public const string RoleAdmin = "ADMIN";
        
        public const string RoleCustomer = "CUSTOMER";

        public const string TokenCookie = "JWTToken";
    }
}
