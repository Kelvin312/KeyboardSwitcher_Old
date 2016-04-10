using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
   public static partial class Misc
   {
       
       private static bool DevenvFix(string text) //Фикс студии, когда при копировании без выделения копируется вся строка 
       {
           return (text.Length > 5 && text.StartsWith(@"   ") && text.EndsWith(@"
"));
       }

       private static string ConvertText(string text, Language layout) //Перекодирование текста 
       {
           const string RusKey =
               "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю. ";
           const string EngKey =
               "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./ ";

           string str = "";
           switch (layout)
           {
               case Language.ru_RU:
                   foreach (char c in text)
                   {
                       try
                       {
                           str += EngKey.Substring(RusKey.IndexOf(c), 1);
                       }
                       catch
                       {
                           str += c;
                       }
                   }
                   break;
               case Language.en_US:
                   foreach (char c in text)
                   {
                       try
                       {
                           str += RusKey.Substring(EngKey.IndexOf(c), 1);
                       }
                       catch
                       {
                           str += c;
                       }
                   }
                   break;
               default:
                   str = text;
                   break;
           }
           return str;
       }

       public static bool SwitchText(IntPtr hWnd, Language layout, TextReplaceMethod rm)
       {
           bool success = false;
           string text;
           IDictionary<string, object> backup = null;
           if ((rm & TextReplaceMethod.UseClipboard) != 0)
           {
               backup = GetClipboardData();
               var oldId = WinApi.GetClipboardSequenceNumber();
               text = GetText(hWnd, rm);
               var newId = WinApi.GetClipboardSequenceNumber();
               if (oldId == newId) return success;
           }
           else
           {
               text = GetText(hWnd, rm);
           }
           if (!(string.IsNullOrWhiteSpace(text) || rm == TextReplaceMethod.Devenv && DevenvFix(text)))
           {
               var convText = ConvertText(text, layout);
               SetText(convText, hWnd, rm);
               success = true;
           }
           if ((rm & TextReplaceMethod.UseClipboard) != 0)
           {
               SetClipboardData(backup);
           }
           return success;
       }

       public static void SwitchLayout(IntPtr hWnd, Language layout) //Смена раскладки 
       {
           switch (layout)
           {
               case Language.ru_RU:
                   layout = Language.en_US;
                   break;
               case Language.en_US:
                   layout = Language.ru_RU;
                   break;
               default:
                   layout = Language.en_US;
                   break;
           }
           WinApi.PostMessage(hWnd, WinApi.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero,
               WinApi.LoadKeyboardLayout(string.Format("{0:X8}", (int)layout), WinApi.KLF_ACTIVATE));
       }

       public static void SetLayout(IntPtr hWnd, Language layout)
       {
           WinApi.PostMessage(hWnd, WinApi.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero,
           WinApi.LoadKeyboardLayout(string.Format("{0:X8}", (int)layout), WinApi.KLF_ACTIVATE));
       }

   }
}
