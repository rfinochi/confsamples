using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MovieIndex.Helpers
{
    public static class RatingExtensions
    {
        public static MvcHtmlString RatingFor<TModel, TProperty>( this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int numerOfStart )
        {
            return InputExtensions.TextBoxFor( htmlHelper, expression, new { @class = String.Format( CultureInfo.InvariantCulture, "rating rating{0}", numerOfStart ) } );
        }
    }
}