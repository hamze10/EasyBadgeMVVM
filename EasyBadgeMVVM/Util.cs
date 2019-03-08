using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBadgeMVVM
{
    public class Util
    {

        private static char[] charsToReplace = new char[] { '_', '.', '-', '#', ':' };

        private static readonly IDictionary<string, ISet<string>> LANGUAGES;

        // Static constructor to initialize the static variable.
        // It is invoked before the first instance constructor is run.
        // Used to fill the dictionnary
        static Util()
        {
            //TODO JSON FILE TO STORE EN-FR-NDLS
            LANGUAGES = new Dictionary<string, ISet<string>>();
            LANGUAGES.Add("lastname", new HashSet<string>(new string[] { "nom", "achternaam" }));
            LANGUAGES.Add("firstname", new HashSet<string>(new string[] { "prenom", "voornaam" }));
            LANGUAGES.Add("company", new HashSet<string>(new string[] { "entreprise", "bedrijf" }));
            LANGUAGES.Add("profil", new HashSet<string>(new string[] { "profile", "profiel" }));
        }

        public static String TrimWord(string word)
        {
            return charsToReplace.Aggregate(word, (c1, c2) => c1.Replace(c2, ' ')).Replace(" ", string.Empty).ToLower();
        }

        public static void AddCharToReplace(char c)
        {
            charsToReplace[charsToReplace.Length] = c;
        }

        public static string TranslateField(string field)
        {
            if (LANGUAGES.ContainsKey(field.ToLower())) return field.ToLower();
            return LANGUAGES.SingleOrDefault(s => s.Value.Where(s1 => s1.ToLower().Equals(field.ToLower())).SingleOrDefault() != null).Key;
        }
    }
}
