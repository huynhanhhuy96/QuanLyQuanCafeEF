namespace Services
{
    using Core.Models;
    using Services.Interface;
    using System.Linq;

    public class LoginServices : ILoginServices
    {
        public bool Login(string userName, string password)
        {
            using var dbcontext = new QuanLyQuanCafeContext();
            if (dbcontext.Accounts.Where(x => $"{userName}".Equals(x.UserName) && $"{password}".Equals(x.PassWord)).Select(x => x).SingleOrDefault() != null)
            {
                return true;
            }
            return false;
        }
    }
}
