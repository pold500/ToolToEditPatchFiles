using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoveAllPatchesWithParent
{
    class Program
    {
        struct Entry { public int start; public int length; };

        static void Main(string[] args)
        {
            string path = @"D:\dev\projects\NewInfamous\kha_update\trunk\menus.lv.3.patch";//args[0];
            string pathToWrite = @"D:\dev\projects\NewInfamous\kha_update\trunk\menus.lv.3.edited.patch";

            // This text is added only once to the file.
            if (File.Exists(path))
            {
                // Open the file to read from.
                string readText = File.ReadAllText(path);
               
                string search_pattern_in_patch_entry = "<p name=\"Parent\"";
                var allGoodEntries = collectPatchEntriesWhichContainString(readText, search_pattern_in_patch_entry);
                string textToWrite = null;
                foreach (Entry entry in allGoodEntries)
                {
                    var substringToWrite = readText.Substring(entry.start, entry.length);
                    textToWrite += substringToWrite + "\n";
                }
                File.WriteAllText(pathToWrite, textToWrite);

            }
            
        }



        private static List<Entry> collectPatchEntriesWhichContainString(string input, string search_pattern_in_patch_entry)
        {
            string regex_matchStartOfPatchEntry = @"@@[0-9,\s-\+]*@@";
            Regex rx = new Regex(regex_matchStartOfPatchEntry, RegexOptions.Compiled );

            var matches = rx.Matches(input);
            List<Entry> allSatisfiyngEntries = new List<Entry>();
            foreach(Match match in matches)
            {
                Entry entry = new Entry();
                entry.start = match.Index;
                entry.length = match.NextMatch().Index - match.Index;
                if(entry.length <= 0)
                {
                    break;
                }
                if(!input.Substring(entry.start, entry.length).Contains(search_pattern_in_patch_entry))
                {
                    allSatisfiyngEntries.Add(entry);
                }
            }
            return allSatisfiyngEntries;
        }
    }
}
