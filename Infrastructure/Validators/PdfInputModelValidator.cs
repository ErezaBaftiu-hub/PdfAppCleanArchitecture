using FluentValidation;
using Infrastructure.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
           
            RuleFor(x => x.Options.FileName)
                .NotEmpty()
                .WithMessage("FileName should not be empty")
                .NotNull()
                .WithMessage("FileName should not be null");

            RuleFor(x => x.Options.PageColorMode)
                .IsInEnum();
            
            RuleFor(x => x.Options.PageOrientation)
                .IsInEnum();

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


    }
}
