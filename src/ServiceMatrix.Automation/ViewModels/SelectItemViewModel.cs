using NuPattern.Presentation;

namespace NServiceBusStudio.Automation.ViewModels
{
    public class SelectItemViewModel : ViewModel
    {
        bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(() => IsSelected);
                }
            }
        }

        public string Name { get; set; }

        public object Item { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
