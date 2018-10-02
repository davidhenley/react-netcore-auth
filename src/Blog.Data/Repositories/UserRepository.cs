using Blog.Model.Entities;
using Blog.Data.Abstract;

namespace Blog.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(BlogContext context) : base(context)
        { }

        public bool IsEmailUniq(string email)
        {
            var user = this.GetSingle(u => u.Email == email);
            return user is null;
        }

        public bool IsUsernameUniq(string username)
        {
            var user = this.GetSingle(u => u.Username == username);
            return user is null;
        }
    }
}