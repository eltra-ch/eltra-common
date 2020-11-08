using EltraXamCommon.Dialogs;
using Prism.Services.Dialogs;
using System;

namespace EltraXamCommon.Plugins.Events
{
    public class DialogRequestedEventArgs : EventArgs
    {
        public XamDialogViewModel ViewModel { get; set; }
        
        public DialogParameters Parameters { get; set; }

        public IDialogResult DialogResult { get; set; }
    }
}
