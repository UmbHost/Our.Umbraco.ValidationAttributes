using Our.Umbraco.ValidationAttributes.Helpers;
using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.ValidationAttributes
{
    /// <summary>
    /// Rejects values containing angle brackets or control characters, so markup/script
    /// cannot be stored in a free-text field. Defence-in-depth against stored XSS: the value
    /// is validated and rejected, never altered, so legitimate text is stored unchanged.
    /// </summary>
    public sealed class UmbracoNoHtmlAttribute : RegularExpressionAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        // Allow any character except '<', '>' and C0/C1 control characters (incl. DEL).
        // Apostrophes, hyphens, accented letters and non-Latin scripts remain valid.
        private const string NoHtmlPattern = @"^[^<>\u0000-\u001F\u007F-\u009F]*$";

        public string DictionaryKey { get; set; } = "No Markup Error";

        public UmbracoNoHtmlAttribute() : base(NoHtmlPattern) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessage = ValidationAttributesService.DictionaryValue(DictionaryKey);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-regex", ErrorMessage);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-regex-pattern", Pattern);
        }
    }
}
