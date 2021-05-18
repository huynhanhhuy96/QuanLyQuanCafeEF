using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cross.NativeInjectorBootStrapper
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDbContext<QuanLyQuanCafeContext>((DbContextOptionsBuilder options)=> options.UseSqlServer(
                connectionString: "Server=.\\SQLEXPRESS;Database=QuanLyQuanCafe;Trusted_Connection=True"
                ));
        }
    }
}
