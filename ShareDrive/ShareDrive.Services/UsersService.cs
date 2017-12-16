using ShareDrive.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using ShareDrive.ViewModels.Admin.User;
using ShareDrive.Common;
using ShareDrive.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Linq;
using AutoMapper;
using ShareDrive.Infrastructure;

namespace ShareDrive.Services
{
    public class UsersService : IUsersService
    {
        private readonly IDbRepository<ApplicationUser> users;
        private readonly IMapper mapper;

        public UsersService(IDbRepository<ApplicationUser> users, IMapper mapper)
        {
            this.users = users;
            this.mapper = mapper;
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

        public IEnumerable<IndexViewModel> GetAllAdmin()
        {
            return this.users.GetAll()
                .Include(u => u.Cars)
                .Include(u => u.Drives)
                .Include(u => u.DrivesPassengers)
                .Where(u => u.Email != GlobalConstants.AdminMail)
                .ProjectTo<ShareDrive.ViewModels.Admin.User.IndexViewModel>()
                .ToList();
        }

        public DetailsViewModel GetDetailsModel(int id)
        {
            ApplicationUser user = this.users.GetByIdQueryable(id)
                .Include(u => u.Cars)
                .Include(u => u.Drives)
                .Include(u => u.DrivesPassengers)
                .FirstOrDefault();

            DetailsViewModel model = this.mapper.Map<DetailsViewModel>(user);
            return model;
        }
    }
}
