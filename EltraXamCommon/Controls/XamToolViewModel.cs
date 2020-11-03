using Xamarin.Forms;
using EltraUiCommon.Controls;
using EltraXamCommon.Framework;

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
    }
}
