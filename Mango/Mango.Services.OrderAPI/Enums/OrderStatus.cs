namespace Mango.Services.OrderAPI.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Approved = 2,
        ReadyForPickup = 3,
        Completed = 4,
        Refunded = 5,
        Cancelled = 6,
    }
}
