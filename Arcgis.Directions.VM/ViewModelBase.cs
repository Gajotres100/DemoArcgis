using Arcgis.Directions.Data.Model;
using System.Collections.Generic;


namespace Arcgis.Directions.VM
{
    public class ViewModelBase
    {
        public ViewModelBase() { }
        public string ReturnUrl { get; set; }
        public string Lang { get; set; }

        public IEnumerable<Language> LanguageList = new List<Language>();
        public Language Langugae { get; set; }      
    }
}
