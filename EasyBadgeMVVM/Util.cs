using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM
{
    public static class Util
    {
        public static readonly IList<string> differentProfiles = new ReadOnlyCollection<string>(new List<string>(new string[] { "profiel", "profile", "profil" }));
    }
}
