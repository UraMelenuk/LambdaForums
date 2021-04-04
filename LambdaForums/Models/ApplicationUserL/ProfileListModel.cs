using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaForums.Models.ApplicationUserL
{
    public class ProfileListModel                        // клас ProfileListModel
    {
        public IEnumerable<ProfileModel> Profiles { get; set; }    // Колекція профайлів користувачів модель
    }
}
