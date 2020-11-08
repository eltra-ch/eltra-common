using Xamarin.Forms;
using EltraUiCommon.Controls;
using EltraXamCommon.Framework;
using EltraXamCommon.Plugins.Events;
using System;
using EltraXamCommon.Dialogs;
using Prism.Services.Dialogs;

namespace EltraXamCommon.Controls
{
    public class XamToolViewModel : ToolViewModel
    {
        #region Constructors

        public XamToolViewModel()
        {
            Init(new InvokeOnMainThread());
        }

        public XamToolViewModel(ToolViewBaseModel parent)
            : base(parent)
        {
        }

        #endregion

        #region Properties

        public ImageSource Image { get; set; }

        #endregion

        #region Events

        public event EventHandler<DialogRequestedEventArgs> DialogRequested;

        #endregion

        #region Events handling

        protected void OnDialogRequested(XamDialogViewModel viewModel, DialogParameters parameters)
        {
            DialogRequested?.Invoke(this, new DialogRequestedEventArgs() { Parameters = parameters, ViewModel = viewModel });
        }

        #endregion
    }
}
