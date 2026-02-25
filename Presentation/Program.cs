using Application.Interfaces.Repository.Cart_Repo;
using Application.Interfaces.Repository.Ctegory_Repo;
using Application.Interfaces.Repository.Product_Repo;
using Application.Interfaces.Repository.User_Repo;
using Application.Interfaces.Services.Cart_services;
using Application.Interfaces.Services.Category_servises;
using Application.Interfaces.Services.Product_services;
using Application.Interfaces.Services.User_services;
using Application.Mapper;
using Application.Services;
using Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Data;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MapCart.Configure();
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>();

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<CartRepository>().As<ICartRepository>();
            builder.RegisterType<CartItemRepository>().As<ICartItemRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryReposirory>();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            //fadel el order repo

            builder.RegisterType<UserService>().As<IUserServices>();
            builder.RegisterType<CartService>().As<ICartServices>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<ProductService>().As<IProductService>();

        }
    }
}
