using System;
using FluentValidation;
using FluentValidation.Validators;

namespace NServiceBusStudio.Automation.ViewModels
{
    internal class ElementHierarchyPickerViewModelValidator : AbstractValidator<ElementHierarchyPickerViewModel>
    {
        const string IdentifierPattern = @"[_\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}][\p{Lu}\p{Ll}\p{Lt}\p{Lm}\p{Lo}\p{Nl}\p{Mn}\p{Mc}\p{Nd}\p{Pc}\p{Cf}]*";

        public ElementHierarchyPickerViewModelValidator()
        {
            RuleFor(vm => vm.SelectedMasterItem)
                .NotNull()
                .Length(1, 30)
                .Matches(IdentifierPattern);

            RuleFor(vm => vm.SelectedSlaveItem)
                .NotNull()
                .Length(1, 30)
                .Matches(IdentifierPattern)
                .Must(BeDifferentToTheMasterItem)
                .WithMessage("Values must be different");
        }

        private bool BeDifferentToTheMasterItem(ElementHierarchyPickerViewModel model, string value, PropertyValidatorContext context)
        {
            return !string.Equals(value, model.SelectedMasterItem, StringComparison.Ordinal);
        }
    }
}
