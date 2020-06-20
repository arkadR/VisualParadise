using System;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Common.Extensions
{
  public static class EnumExtensions
  {
    //https://www.codingame.com/playgrounds/2487/c---how-To-display-friendly-names-for-enumerations
    public static string GetDescription(this Enum GenericEnum)
    {
      MemberInfo[] memberInfo = GenericEnum.GetType().GetMember(GenericEnum.ToString());
      if ((memberInfo != null && memberInfo.Length > 0))
      {
        var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        if ((_Attribs != null && _Attribs.Count() > 0))
        {
          return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
        }
      }
      return GenericEnum.ToString();
    }
  }
}
