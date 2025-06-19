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
    }
}
