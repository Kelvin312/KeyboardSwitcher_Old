using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardSwitcher
{

    public enum StatusAll
    {
        On = 1,
        Off = 0,
        Default = 3
    }
    public class ProcessClassConfig
    {
        public TextReplaceMethod InputMode;
        public StatusAll TextSwitch;
        public StatusAll LayoutSwitch;
        public Language LayoutSet;
        public StatusAll XMove;
    }
  
    public static class Config
    {
        public static Dictionary<string, Dictionary<string, ProcessClassConfig>> ProcessClassTable { get; set; }

        //Swincher
        public static bool IsSwitchOnSelectedText { get; set; }

        //XMove
        public static bool IsActivateWindow { get; set; }

    }
}
