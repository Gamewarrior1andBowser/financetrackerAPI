namespace financetrackerAPI.Models
{
    public class UserLoginRequest
    {
        public string UsernameOrEmail { get; set; }

        public string password { get; set; }
    }
}
