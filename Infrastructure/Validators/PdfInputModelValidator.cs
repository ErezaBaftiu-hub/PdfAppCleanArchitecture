using FluentValidation;
using Infrastructure.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WkHtmlToPdfDotNet;

namespace Infrastructure.Validators
{
    public class PdfInputModelValidator : AbstractValidator<PdfInputModel>
    {
        public PdfInputModelValidator()
        {
            RuleFor(x => x.HtmlString)
                .NotEmpty()
                .WithMessage("HtmlString should not be empty")
                .NotNull()
                .WithMessage("HtmlString should not be null");
            
            RuleFor(x => x.HtmlString)
                .Must(ValidateHtmlString)
                .WithMessage("HtmlString should contain only inside <body> tags");
           
            RuleFor(x => x.Options.PageColorMode)
                .Must(ValidatePageColor)
                .NotEmpty()
                .WithMessage("PageColorMode should not be empty")
                .NotNull()
                .WithMessage("PageColorMode should not be null");

            RuleFor(x => x.Options.PageOrientation)
                .Must(ValidatePageOrientation)
                .NotEmpty()
                .WithMessage("PageOrientation should not be empty")
                .NotNull()
                .WithMessage("PageOrientation should not be null");

            RuleFor(x => x.Options.PagePaperSize)
                .Must(ValidatePagePaperSize)
                .NotEmpty()
                .WithMessage("PagePaperSize should not be empty")
                .NotNull()
                .WithMessage("PagePaperSize should not be null");
        }

        private static bool ValidateHtmlString(string htmlString)
        {
            try
            {
                var htmlContentBase64 = Convert.FromBase64String(htmlString);

                var htmlContent = Encoding.UTF8.GetString(htmlContentBase64);

                if (htmlContent.StartsWith("<html>") || htmlContent.StartsWith("<title>") || htmlContent.StartsWith("<body>"))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error in validating html string with details: {htmlString} ", htmlString, ex);

                return false;
            }
        }

        private static bool ValidatePageOrientation(string pageOrientation)
        {
            try
            {
                var pageOrientationEnums = Enum.GetNames(typeof(Orientation));
                var exists = pageOrientationEnums.Any(str => str.Contains(pageOrientation));

                if (!exists)
                    throw new Exception("Page Orientation doesn't exits");

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Page Orientation doesn't exits", ex);

                return false;
            }
        }

        private static bool ValidatePageColor(string pageColor)
        {
            try
            {
                var pageColorEnums = Enum.GetNames(typeof(ColorMode));
                var exists = pageColorEnums.Any(str => str.Contains(pageColor));

                if (!exists)
                    throw new Exception("Page Orientation doesn't exits");

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Page Orientation doesn't exits", ex);

                return false;
            }
        }

        private static bool ValidatePagePaperSize(string pagePaperSize)
        {
            try
            {
                var pagePaperSizeEnums = Enum.GetNames(typeof(PaperKind));
                var exists = pagePaperSizeEnums.Any(str => str.Contains(pagePaperSize));

                if (!exists)
                    throw new Exception("Page Orientation doesn't exits");

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Page Orientation doesn't exits", ex);

                return false;
            }
        }

    }
}
