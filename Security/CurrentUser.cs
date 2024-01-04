using Town_of_Fairfax.Data;

namespace Town_of_Fairfax.Security
{
    public class CurrentUser
    {

        public CurrentUser() {
            user = new User();
            user.Username = "Guest";
            user.Password = "123";
            user.Role = "User";
        }

        public User user;

    }
}
