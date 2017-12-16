using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShareDrive.ViewModels.BaseViewModels
{
    public abstract class UserBaseDisplayViewModel
    {
        public int Id { get; set; }

        [DisplayName("Full Name:")]
        public string Name { get; set; }

        [DisplayName("Phone Number: ")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email: ")]
        public string Email { get; set; }

        // TODO: Add image to the user
        //public byte[] Image { get; set; }

        // TODO: Add Rating
    }
}
