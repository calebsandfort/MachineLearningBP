using Abp.Web.Mvc.Views;

namespace MachineLearningBP.Web.Views
{
    public abstract class MachineLearningBPWebViewPageBase : MachineLearningBPWebViewPageBase<dynamic>
    {

    }

    public abstract class MachineLearningBPWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected MachineLearningBPWebViewPageBase()
        {
            LocalizationSourceName = MachineLearningBPConsts.LocalizationSourceName;
        }
    }
}