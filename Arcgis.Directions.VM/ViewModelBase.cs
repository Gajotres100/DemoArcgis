using Arcgis.Directions.Data.Model;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Arcgis.Directions.VM
{
    public class ViewModelBase
    {
        public string ReturnUrl { get; set; }
        public string Lang { get; set; }

        public IEnumerable<Language> LanguageList = new List<Language>();
        public Language Langugae { get; set; }

    }
}
