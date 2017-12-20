namespace ShareDrive.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using AutoMapper;

    using ShareDrive.Services.Contracts;
    using ShareDrive.ViewModels.Admin.User;
    using ShareDrive.Common;
    using ShareDrive.Models;    
    using ShareDrive.Infrastructure;

    public class UsersService : IUsersService
    {
        private readonly IDbRepository<ApplicationUser> users;
        private readonly IMapper mapper;

        public UsersService(IDbRepository<ApplicationUser> users, IMapper mapper)
        {
            this.users = users;
            this.mapper = mapper;
        }

        public bool CheckIfUserExists(int userId)
        {
            return this.users.GetById(userId) != null;
        }

        public ApplicationUser GetById(int id)
        {
            return this.users.GetById(id);
        }

        public IEnumerable<UserAdminIndexViewModel> GetAllAdmin()
        {
            return this.users.GetAll()
                .Include(u => u.Cars)
                .Include(u => u.Drives)
                .Include(u => u.DrivesPassengers)
                .Where(u => u.Email != GlobalConstants.AdminMail)
                .ProjectTo<ShareDrive.ViewModels.Admin.User.UserAdminIndexViewModel>()
                .ToList();
        }

        public UserAdminDetailsViewModel GetDetailsModel(int id)
        {
            ApplicationUser user = this.users.GetByIdQueryable(id)
                .Include(u => u.Cars)
                .Include(u => u.Drives)
                .Include(u => u.DrivesPassengers)
                .FirstOrDefault();

            UserAdminDetailsViewModel model = this.mapper.Map<UserAdminDetailsViewModel>(user);
            return model;
        }

        public bool Delete(int id)
        {
            var user = this.users.GetById(id);

            if (user != null && user.Email != GlobalConstants.AdminMail)
            {
                return this.users.Delete(user);
            }

            return false;
        }
    }
}
