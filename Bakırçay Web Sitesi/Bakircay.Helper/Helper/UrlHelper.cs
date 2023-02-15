using System.Text;

namespace Bakircay.Helper.Helper
{
  public class UrlHelper
  {
    public static string TurkishChrToEnglishChr(string text)
    {
      if (string.IsNullOrEmpty(text)) return text;

      var turkishChToEnglishChDic = new Dictionary<char, char>()
      {
        {'ç','c'},
        {'Ç','C'},
        {'ğ','g'},
        {'Ğ','G'},
        {'ı','i'},
        {'İ','I'},
        {'ş','s'},
        {'Ş','S'},
        {'ö','o'},
        {'Ö','O'},
        {'ü','u'},
        {'Ü','U'},
        {' ','-'}
      };

      return text.Aggregate(new StringBuilder(), (sb, chr) =>
      {
        sb.Append(turkishChToEnglishChDic.ContainsKey(chr) ? turkishChToEnglishChDic[chr] : chr);

        return sb;
      }).ToString();
    }


  }
}
