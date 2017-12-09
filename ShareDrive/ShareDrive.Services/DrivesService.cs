using AutoMapper;
using ShareDrive.Common;
using ShareDrive.Models;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareDrive.Services
{
    public class DrivesService : IDrivesService
    {
        private readonly IDbRepository<Drive> drives;
        private readonly IMapper mapper;

        public DrivesService(IDbRepository<Drive> drives, IMapper mapper)
        {
            this.drives = drives;
            this.mapper = mapper;
        }

        public void Create (CreateViewModel model, City cityFrom, City cityTo, int userId)
        {
            Drive drive = this.mapper.Map<Drive>(model);

            drive.DateTime = this.GetDateFromModel(model.DateTime);

            drive.From = cityFrom;
            drive.To = cityTo;
            drive.DriverId = userId;

            this.drives.Create(drive);
        }

        private DateTime GetDateFromModel(string modelDate)
        {
            string[] tokens = modelDate.Split('-');

            int[] dateTokens = this.GetTokens(tokens[0], " ");
            int[] timeTokens = this.GetTokens(tokens[1], ":");

            return new DateTime(dateTokens[2], dateTokens[1], dateTokens[0], timeTokens[0], timeTokens[1], 0);
        }

        private int[] GetTokens(string input, string separator)
        {
            int[] tokens = input.Trim()
                .Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            return tokens;
        }
    }
}
