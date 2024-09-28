using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CRUD.Solution.Helpers;

public static class HtmlHelperExtensions
{
    public static string RenderSortLink(this IHtmlHelper htmlHelper, string columnName, string columnDisplayName,
        ViewDataDictionary viewData)
    {
        var sortOrderIcon = "";
        var nextSortOrder = "ASC";

        if (viewData["sortBy"]?.ToString() == columnName)
        {
            if (viewData["sortOrder"]?.ToString() == "ASC")
            {
                sortOrderIcon = "<i class='fa-solid fa-sort-up'></i>";
                nextSortOrder = "DESC";
            }
            else if (viewData["sortOrder"]?.ToString() == "DESC")
            {
                sortOrderIcon = "<i class='fa-solid fa-sort-down'></i>";
                nextSortOrder = "ASC";
            }
        }

        var searchBy = viewData["searchBy"]?.ToString() ?? "";
        var searchString = viewData["searchString"]?.ToString() ?? "";

        return
            $"<a href='?searchBy={searchBy}&searchString={searchString}&sortBy={columnName}&sortOrder={nextSortOrder}'>" +
            $"{columnDisplayName} {sortOrderIcon}</a>";
    }
}