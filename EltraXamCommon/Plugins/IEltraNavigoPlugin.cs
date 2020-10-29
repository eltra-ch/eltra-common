using EltraXamCommon.Controls;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EltraXamCommon.Plugins
{
    public interface IEltraNavigoPlugin
    {
        IDialogService DialogService { get; set; }

        List<ToolViewModel> GetViewModels();

        ContentView GetView(ToolViewModel viewModel);
    }
}
