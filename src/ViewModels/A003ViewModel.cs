using MetaFrm.Management.Razor.Models;
using MetaFrm.MVVM;

namespace MetaFrm.Management.Razor.ViewModels
{
    /// <summary>
    /// A003ViewModel
    /// </summary>
    public partial class A003ViewModel : BaseViewModel
    {
        /// <summary>
        /// SearchModel
        /// </summary>
        public SearchModel SearchModel { get; set; } = new();

        /// <summary>
        /// SelectResultModel
        /// </summary>
        public List<CommonClassModel> SelectResultModel { get; set; } = new List<CommonClassModel>();

        /// <summary>
        /// C001ViewModel
        /// </summary>
        public A003ViewModel()
        {

        }
    }
}