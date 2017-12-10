using ShareDrive.ViewModels.Car;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareDrive.ViewModels.Drive
{
    public class EditViewModel : BaseEditViewModel
    {
        public List<SelectViewModel> Cars { get; set; }

        [Required]
        public int CarId { get; set; }
    }
}
