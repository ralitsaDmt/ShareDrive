using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.ViewModels.Car
{
    public class CarSelectViewModel
    {
        public CarSelectViewModel() { }

        public CarSelectViewModel(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
