using System;
using System.Text;

public static class BigNumberExtension
{
    private static readonly string[] suffixes = {
        "", "K", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af",
        "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap",
        "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az",
        "ba", "bb", "bc", "bd", "be", "bf","bg", "bh", "bi","bk", "bl", "bm",
        "bn", "bo", "bp","bq", "br", "bs","bt", "bu", "bv","bw", "bx", "by", "bz"
    };

    public static string ToReadableString(this float number, int decimalPlaces = 1, string typeToString = "F0")
    {
        if (number < 10000)
            return number.ToString(typeToString);

        int magnitude = (int)Math.Floor(Math.Log10(number));
        int suffixIndex = magnitude / 3;

        if (suffixIndex >= suffixes.Length)
        {
            return number.ToString("0." + new string('#', decimalPlaces) + "E+0");
        }

        double shortNumber = number / Math.Pow(10, suffixIndex * 3);

        double factor = Math.Pow(10, decimalPlaces);
        shortNumber = Math.Floor(shortNumber * factor) / factor;

        string format = "0";
        if (decimalPlaces > 0)
        {
            format += "." + new string('#', decimalPlaces);
        }

        return shortNumber.ToString(format) + suffixes[suffixIndex];
    }
}