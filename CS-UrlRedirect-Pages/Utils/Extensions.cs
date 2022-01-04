// ============================================================================
//    Author: Kenneth Perkins
//    Date:   Jan 3, 2022
//    Taken From: http://programmingnotes.org/
//    File:  Utils.cs
//    Description: Handles general utility functions
// ============================================================================
using System;

namespace Utils {
    /// <summary>
    /// Determines if a url is valid or not
    /// </summary>
    /// <param name="url">The url to check</param>
    /// <returns>True if a url is valid, false otherwise</returns>
    public static class Extensions {
        /// <summary>
        /// Creates a PartialViewResult object that renders a partial view, by using the specified view name.
        /// </summary>
        /// <param name="page">The Razor page</param>
        /// <param name="viewName">The name of the view that is rendered to the response</param>
        /// <returns>A partial-view result object</returns>
        [Microsoft.AspNetCore.Mvc.NonAction]
        public static Microsoft.AspNetCore.Mvc.PartialViewResult PartialView(this Microsoft.AspNetCore.Mvc.RazorPages.PageModel page, string viewName) {
            var newViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(page.ViewData);
            return PartialView(page, viewName, newViewData);
        }

        /// <summary>
        /// Creates a PartialViewResult object that renders a partial view, by using the specified view name.
        /// </summary>
        /// <param name="page">The Razor page</param>
        /// <param name="viewName">The name of the view that is rendered to the response</param>
        /// <param name="model">The model that is rendered by the partial view</param>
        /// <returns>A partial-view result object</returns>
        [Microsoft.AspNetCore.Mvc.NonAction]
        public static Microsoft.AspNetCore.Mvc.PartialViewResult PartialView<T>(this Microsoft.AspNetCore.Mvc.RazorPages.PageModel page, string viewName, T model) {
            var newViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<T>(page.ViewData, model);
            return PartialView(page, viewName, newViewData);
        }

        [Microsoft.AspNetCore.Mvc.NonAction]
        public static Microsoft.AspNetCore.Mvc.PartialViewResult PartialView(this Microsoft.AspNetCore.Mvc.RazorPages.PageModel page, string viewName, Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary viewData) {
            return new Microsoft.AspNetCore.Mvc.PartialViewResult() {
                ViewName = viewName,
                ViewData = viewData,
                TempData = page.TempData
            };
        }
    }
}
