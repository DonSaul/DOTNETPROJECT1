namespace Softserve.ProjectLab.ClientAPI.Models
{
    public class LoginBody(string user, string pass)
    {
        public string User { get; set; } = user;
        public string Pass { get; set; } = pass;
    }
}
