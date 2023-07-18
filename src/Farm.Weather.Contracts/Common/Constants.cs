using System.Collections.Generic;

namespace Farm.Weather.Contracts.Common
{
    public static class Constants
    {
        public static IEnumerable<string> UkrainianCities =>
            new[] { "Kiev", "Kharkiv", "Dnipro", "Lviv", "Bila Tserkva", "Vinnitsa", "Odesa", "Yalta" };
    }
}