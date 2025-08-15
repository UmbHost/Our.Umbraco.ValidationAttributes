using Our.Umbraco.ValidationAttributes.Helpers;
using Our.Umbraco.ValidationAttributes.Interfaces;
using Our.Umbraco.ValidationAttributes.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Our.Umbraco.ValidationAttributes
{
    /// <summary>
    /// Specified that two properties data field value must match.
    /// </summary>
    public sealed class UmbracoCompareAttribute : CompareAttribute, IClientModelValidator, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "EqualToError";

        public new string ErrorMessageString { get; set; }
        public new string OtherPropertyDisplayName { get; set; }

        public UmbracoCompareAttribute(string otherProperty) : base(otherProperty) {}

        public void AddValidation(ClientModelValidationContext context)
        {
            ErrorMessageString = ValidationAttributesService.DictionaryValue(DictionaryKey);
            
            AttributeHelper.MergeAttribute(context.Attributes, "data-val", "true");
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-equalto", ErrorMessageString);
            AttributeHelper.MergeAttribute(context.Attributes, "data-val-equalto-other", $"*.{OtherProperty}");
        }
    }
}
