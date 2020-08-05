using System;
using System.IO;
using System.Linq;

namespace JavaPropertiesUtils.Tests
{
    public static class ResourceUtils
    {
        public static string ReadEmbeddedResource(string name)
        {
            var assembly = typeof(ResourceUtils).Assembly;
            var prefix = assembly.GetName().Name + ".Resources.";
            var resourceName = prefix + name;
            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                const int numSuggestionsToShow = 3;
                
                var lev = new Fastenshtein.Levenshtein(name);
                
                var numResources = assembly.GetManifestResourceNames().Length;
                var validResourceNames = assembly.GetManifestResourceNames()
                    .Select(resName => resName.Substring(prefix.Length))
                    .OrderBy(lev.DistanceFrom)
                    .Take(numSuggestionsToShow)
                    .ToArray();

                var suggestions = string.Join(", ", validResourceNames
                    .Select(name => $"\"{name}\""));

                var numSuggestionsNotShown = numResources - numSuggestionsToShow;

                var moreText = numSuggestionsNotShown > 0
                    ? $" plus {numSuggestionsNotShown} more"
                    : "";
                
                throw new Exception(
                    $"No resource found for name {name}. Did you mean one of these? {suggestions}{moreText}."
                );
            }
            
            using (stream)
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}