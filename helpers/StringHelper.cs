using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTasks.helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Extrai o ID do início de uma string com o formato "ID - ..."
        /// </summary>
        public static int ExtractIdFromFormattedText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return -1;

            var parts = text.Split('-');
            if (parts.Length == 0)
                return -1;

            if (int.TryParse(parts[0].Trim(), out int id))
                return id;

            return -1;
        }

        public static string FormatTimeSpanFriendly(TimeSpan ts)
        {
            if (ts == TimeSpan.Zero) return "-";
            var parts = new List<string>();
            if (Math.Abs(ts.Days) > 0) parts.Add($"{Math.Abs(ts.Days)}d");
            if (Math.Abs(ts.Hours) > 0) parts.Add($"{Math.Abs(ts.Hours)}h");
            if (Math.Abs(ts.Minutes) > 0) parts.Add($"{Math.Abs(ts.Minutes)}m");
            if (parts.Count == 0) return "<1m";
            return string.Join(" ", parts);
        }
    }


}
