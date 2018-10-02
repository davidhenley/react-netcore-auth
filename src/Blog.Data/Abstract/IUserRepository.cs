namespace Blog.Data.Abstract
{
    public interface IUserRepository
    {
        bool IsEmailUniq(string email);
        bool IsUsernameUniq(string username);
    }
}