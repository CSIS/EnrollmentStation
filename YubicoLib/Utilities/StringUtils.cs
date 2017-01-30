using System.Collections.Generic;
using System.Text;

namespace YubicoLib.Utilities
{
    internal static class StringUtils
    {
        public static IEnumerable<string> ParseStrings(byte[] data)
        {
            int prev = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != 0)
                    continue;

                string strName = Encoding.ASCII.GetString(data, prev, i - prev);

                if (!string.IsNullOrEmpty(strName))
                    yield return strName;

                prev = i + 1;
            }
        }
    }
}
