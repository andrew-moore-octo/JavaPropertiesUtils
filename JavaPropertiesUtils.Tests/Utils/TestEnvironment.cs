using System;
using Assent;
using Assent.Namers;

namespace JavaPropertiesUtils.Tests.Utils
{
    public static class TestEnvironment 
    {
        private static readonly bool IsCI = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));

        public static readonly Configuration AssentConfiguration = new Configuration()
            .UsingNamer(new SubdirectoryNamer("Approved"))
            .SetInteractive(!IsCI);
    }
}

