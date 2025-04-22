using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatherineDesktopApp.Services
{

    public interface IDialogService
    {
        Task ShowExceptionDialogAsync(string errorMessage, string entityName, string exceptionType);
    }

    internal class DialogService : IDialogService
    {
        private readonly IApplicationWindow _applicationWindow;
        private readonly ResourceDictionary _resources = new();
        private readonly IResourceLocationService _resourceService;

        public DialogService(IApplicationWindow applicationWindow, IResourceLocationService resourceService)
        {
            _applicationWindow = applicationWindow;
            _resources.MergedDictionaries.Add(new XamlControlsResources());
            _resourceService = resourceService;
        }

        public async Task ShowExceptionDialogAsync(string errorMessage, string entityName, string exceptionType)
        {

            string dialogTitle;

            (dialogTitle, StackPanel dialogContent) = GenerateExceptionDialogProperties(errorMessage, entityName, exceptionType);

            XamlRoot currentXamlRoot = _applicationWindow.XamlRoot;

            ContentDialog dialog = new()
            {
                Style = (Style)_resources["DefaultContentDialogStyle"],
                XamlRoot = currentXamlRoot,
                Title = dialogTitle,
                PrimaryButtonText = "OK",
                DefaultButton = ContentDialogButton.Primary,
                Content = dialogContent
            };
            await dialog.ShowAsync();
        }

        private (string, StackPanel) GenerateExceptionDialogProperties(string errorMessage, string entityName, string exceptionType)
        {
            string dialogTitle;
            string dialogFooter;

            StackPanel dialogContent = new()
            {
                Orientation = Orientation.Vertical,
                Spacing = 10
            };

            List<TextBlock> textBlocks = new();

            switch (exceptionType)
            {
                case nameof(ArgumentNullException):
                case nameof(ArgumentException):
                    {
                        dialogTitle = _resourceService.GetErrorMessageString($"DialogHeader{nameof(ArgumentException)}");
                        dialogTitle = string.Format(dialogTitle, entityName);
                        dialogFooter = _resourceService.GetErrorMessageString($"DialogFooter{nameof(ArgumentException)}");
                        textBlocks = GenerateErrorDialogContent(errorMessage, dialogFooter);
                        foreach (TextBlock block in textBlocks)
                            dialogContent.Children.Add(block);
                        break;
                    }
                case nameof(InvalidOperationException):
                    {
                        dialogTitle = _resourceService.GetErrorMessageString($"DialogHeader{nameof(InvalidOperationException)}");
                        dialogTitle = string.Format(dialogTitle, entityName);
                        dialogFooter = _resourceService.GetErrorMessageString($"DialogFooter{nameof(InvalidOperationException)}");
                        textBlocks = GenerateErrorDialogContent(errorMessage, dialogFooter);
                        foreach (TextBlock block in textBlocks)
                            dialogContent.Children.Add(block);
                        break;
                    }
                default:
                    {
                        dialogTitle = _resourceService.GetErrorMessageString($"DialogHeaderDefaultException");
                        dialogFooter = _resourceService.GetErrorMessageString($"DialogFooterDefaultException");
                        // In the default case, localized text is moved from footer to body to highlight Support instructions.
                        textBlocks = GenerateErrorDialogContent(dialogFooter, errorMessage);
                        foreach (TextBlock block in textBlocks)
                            dialogContent.Children.Add(block);
                        break;
                    }
            }
            return (dialogTitle, dialogContent);
        }

        private List<TextBlock> GenerateErrorDialogContent(string baseMessage, string footerMessage)
        {
            List<TextBlock> response = new() {
                new TextBlock()
                {
                    Style = (Style)_resources["BodyTextBlockStyle"],
                    Text = baseMessage,
                    TextWrapping = TextWrapping.WrapWholeWords,
                    TextAlignment = TextAlignment.Left
                },
                new TextBlock()
                {
                    Style = (Style)_resources["BodyTextBlockStyle"],
                    Text = footerMessage,
                    Foreground = (Brush)_resources["TextFillColorSecondaryBrush"],
                    TextWrapping = TextWrapping.WrapWholeWords,
                    TextAlignment = TextAlignment.Left
                }
            };

            return response;
        }
    }
}

