using Xamarin.Forms;
using EltraUiCommon.Controls;

namespace EltraXamCommon.Controls
{
    public class XamToolViewModel : ToolViewModel
    {
        #region Constructors

        public XamToolViewModel()
        {
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
