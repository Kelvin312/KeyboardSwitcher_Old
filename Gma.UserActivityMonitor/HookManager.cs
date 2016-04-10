using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gma.UserActivityMonitor 
{

    /// <summary>
    /// This class monitors all mouse activities globally (also outside of the application) 
    /// and provides appropriate events.
    /// </summary>
    public static partial class HookManager
    {
        //################################################################

        private static ClipboardNotifier clipboardNotifier = new ClipboardNotifier();
        private static KeyboardHook keyboardHook = new KeyboardHook();
        private static MouseHook mouseHook = new MouseHook();
        private static CBTHook cbtHook = new CBTHook();
        
        /// <summary>
        /// Изменение буфера обмена
        /// </summary>
        public static event EventHandler ClipboardChanged
        {
            add
            {
                clipboardNotifier.StartHook();
                clipboardNotifier.ClipboardChanged += value;
            }

            remove
            {
                clipboardNotifier.ClipboardChanged -= value;
                clipboardNotifier.TryUnhook();
            }
        }

       
        //################################################################
        #region CBT events

        /// <summary>
        /// Окно собирается принять фокус клавиатуры. 
        /// </summary>
        public static event SetFocusEventHandler SetFocus
        {
            add
            {
                cbtHook.StartHook();
                cbtHook.SetFocusCallback += value;
            }

            remove
            {
                cbtHook.SetFocusCallback -= value;
                cbtHook.TryUnhook();
            }
        }

        /// <summary>
        /// Активация меню Пуск (Start).
        /// </summary>
        public static event TaskListEventHandler TaskList
        {
            add
            {
                cbtHook.StartHook();
                cbtHook.TaskListCallback += value;
            }

            remove
            {
                cbtHook.TaskListCallback -= value;
                cbtHook.TryUnhook();
            }
        }

        /// <summary>
        /// Окно хочет стать активным
        /// </summary>
        public static event ActivateEventHandler Activate
        {
            add
            {
                cbtHook.StartHook();
                cbtHook.ActivateCallback += value;
            }

            remove
            {
                cbtHook.ActivateCallback -= value;
                cbtHook.TryUnhook();
            }
        }

        #endregion

        //################################################################
        #region Mouse events


        /// <summary>
        /// Occurs when the mouse pointer is moved. 
        /// </summary>
        public static event EventHandler<MouseEventExtArgs> MouseMove
        {
            add
            {
                mouseHook.StartHook();
                mouseHook.MouseMove += value;
            }

            remove
            {
                mouseHook.MouseMove -= value;
                mouseHook.TryUnhook();
            }
        }

        /// <summary>
        /// Occurs when a click was performed by the mouse. 
        /// </summary>
        public static event EventHandler<MouseEventExtArgs> MouseClick
        {
            add
            {
                mouseHook.StartHook();
                mouseHook.MouseClick += value;
            }

            remove
            {
                mouseHook.MouseClick -= value;
                mouseHook.TryUnhook();
            }
        }

        /// <summary>
        /// Occurs when the mouse a mouse button is pressed. 
        /// </summary>
        public static event EventHandler<MouseEventExtArgs> MouseDown
        {
            add
            {
                mouseHook.StartHook();
                mouseHook.MouseDown += value;
            }

            remove
            {
                mouseHook.MouseDown -= value;
                mouseHook.TryUnhook();
            }
        }

        /// <summary>
        /// Occurs when a mouse button is released. 
        /// </summary>
        public static event EventHandler<MouseEventExtArgs> MouseUp
        {
            add
            {
                mouseHook.StartHook();
                mouseHook.MouseUp += value;
            }

            remove
            {
                mouseHook.MouseUp -= value;
                mouseHook.TryUnhook();
            }
        }

        /// <summary>
        /// Occurs when the mouse wheel moves. 
        /// </summary>
        public static event EventHandler<MouseEventExtArgs> MouseWheel
        {
            add
            {
                mouseHook.StartHook();
                mouseHook.MouseWheel += value;
            }

            remove
            {
                mouseHook.MouseWheel -= value;
                mouseHook.TryUnhook();
            }
        }


        private static event MouseEventHandler sMouseDoubleClick;

        //The double click event will not be provided directly from hook.
        //To fire the double click event wee need to monitor mouse up event and when it occures 
        //Two times during the time interval which is defined in Windows as a doble click time
        //we fire this event.

        /// <summary>
        /// Occurs when a double clicked was performed by the mouse. 
        /// </summary>
        public static event MouseEventHandler MouseDoubleClick
        {
            add
            {
                if (sMouseDoubleClick == null)
                {
                    //We create a timer to monitor interval between two clicks
                    sDoubleClickTimer = new Timer
                    {
                        //This interval will be set to the value we retrive from windows. This is a windows setting from contro planel.
                        Interval = GetDoubleClickTime(),
                        //We do not start timer yet. It will be start when the click occures.
                        Enabled = false
                    };
                    //We define the callback function for the timer
                    sDoubleClickTimer.Tick += DoubleClickTimeElapsed;
                    //We start to monitor mouse up event.
                    MouseUp += OnMouseUp;
                }
                sMouseDoubleClick += value;
            }
            remove
            {
                if (sMouseDoubleClick != null)
                {
                    sMouseDoubleClick -= value;
                    if (sMouseDoubleClick == null)
                    {
                        //Stop monitoring mouse up
                        MouseUp -= OnMouseUp;
                        //Dispose the timer
                        sDoubleClickTimer.Tick -= DoubleClickTimeElapsed;
                        sDoubleClickTimer = null;
                    }
                }
            }
        }

        //This field remembers mouse button pressed because in addition to the short interval it must be also the same button.
        private static MouseButtons sPrevClickedButton;
        //The timer to monitor time interval between two clicks.
        private static Timer sDoubleClickTimer;

        private static void DoubleClickTimeElapsed(object sender, EventArgs e)
        {
            //Timer is alapsed and no second click ocuured
            sDoubleClickTimer.Enabled = false;
            sPrevClickedButton = MouseButtons.None;
        }

        /// <summary>
        /// This method is designed to monitor mouse clicks in order to fire a double click event if interval between 
        /// clicks was short enaugh.
        /// </summary>
        /// <param name="sender">Is always null</param>
        /// <param name="e">Some information about click heppened.</param>
        private static void OnMouseUp(object sender, MouseEventArgs e)
        {
            //This should not heppen
            if (e.Clicks < 1) { return;}
            //If the secon click heppened on the same button
            if (e.Button.Equals(sPrevClickedButton))
            {
                if (sMouseDoubleClick!=null)
                {
                    //Fire double click
                    sMouseDoubleClick.Invoke(null, e);
                }
                //Stop timer
                sDoubleClickTimer.Enabled = false;
                sPrevClickedButton = MouseButtons.None;
            }
            else
            {
                //If it was the firts click start the timer
                sDoubleClickTimer.Enabled = true;
                sPrevClickedButton = e.Button;
            }
        }

        /// <summary>
        /// The GetDoubleClickTime function retrieves the current double-click time for the mouse. A double-click is a series of two clicks of the 
        /// mouse button, the second occurring within a specified time after the first. The double-click time is the maximum number of 
        /// milliseconds that may occur between the first and second click of a double-click. 
        /// </summary>
        /// <returns>
        /// The return value specifies the current double-click time, in milliseconds. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/en-us/library/ms646258(VS.85).aspx
        /// </remarks>
        [DllImport("user32")]
        public static extern int GetDoubleClickTime();


        #endregion

        //################################################################
        #region Keyboard events

 
        /// <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        /// <remarks>
        /// Key events occur in the following order: 
        /// <list type="number">
        /// <item>KeyDown</item>
        /// <item>KeyPress</item>
        /// <item>KeyUp</item>
        /// </list>
        ///The KeyPress event is not raised by noncharacter keys; however, the noncharacter keys do raise the KeyDown and KeyUp events. 
        ///Use the KeyChar property to sample keystrokes at run time and to consume or modify a subset of common keystrokes. 
        ///To handle keyboard events only in your application and not enable other applications to receive keyboard events, 
        /// set the KeyPressEventArgs.Handled property in your form's KeyPress event-handling method to <b>true</b>. 
        /// </remarks>
        public static event KeyPressEventHandler KeyPress
        {
            add
            {
                keyboardHook.StartHook();
                keyboardHook.KeyPress += value;
            }
            remove
            {
                keyboardHook.KeyPress -= value;
                keyboardHook.TryUnhook();
            }
        }

        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        public static event KeyEventHandler KeyUp
        {
            add
            {
                keyboardHook.StartHook();
                keyboardHook.KeyUp += value;
            }
            remove
            {
                keyboardHook.KeyUp -= value;
                keyboardHook.TryUnhook();
            }
        }

        /// <summary>
        /// Occurs when a key is preseed. 
        /// </summary>
        public static event KeyEventHandler KeyDown
        {
            add
            {
                keyboardHook.StartHook();
                keyboardHook.KeyDown += value;
            }
            remove
            {
                keyboardHook.KeyDown -= value;
                keyboardHook.TryUnhook();
            }
        }


        #endregion
    }
}
