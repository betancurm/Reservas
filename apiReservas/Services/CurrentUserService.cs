
namespace apiReservas.Services;
public class CurrentUserService : ICurrentUserService
{
    public Guid ApplicationUserId { get; }
    public CurrentUserService(IHttpContextAccessor http)
    {
        var id = http.HttpContext?.User.FindFirst("uid")?.Value;
        ApplicationUserId = id is not null ? Guid.Parse(id) : Guid.Empty;
    }
}
public interface ICurrentUserService
{
    Guid ApplicationUserId { get; }
}
