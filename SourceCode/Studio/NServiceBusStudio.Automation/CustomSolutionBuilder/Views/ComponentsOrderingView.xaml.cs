using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using AbstractEndpoint;
using NuPattern.Presentation;


namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Views
{
    /// <summary>
    /// Interaction logic for ComponentsOrderingView.xaml
    /// </summary>
    public partial class ComponentsOrderingView : UserControl
    {
        public ComponentsOrderingView()
        {
            InitializeComponent();
        }

        public void SetComponentsView(UserControl view)
        {
            this.ComponentsHolder.Children.Add(view);
        }

        internal void SetComponentLinks(IEnumerable<AbstractEndpoint.IAbstractComponentLink> componentLinks)
        {
            var links = new List<ComponentsOrderingViewModel>();
            
            var setOrder = new Action(() =>
                {
                    var worker = new BackgroundWorker();
                    worker.DoWork += (e, s) =>
                    {
                        foreach (var cl in links.AsEnumerable().Reverse())
                        {
                            cl.Link.Order = links.IndexOf(cl) + 1;
                        }
                    };
                    worker.RunWorkerAsync();
                });
            foreach (var cl in componentLinks.OrderBy(x => x.Order))
            {
                var vm = new ComponentsOrderingViewModel { Link = cl };
                links.Add(vm);
                vm.UpCommand = new RelayCommand(
                    () =>
                    {
                        var prevIndex = links.IndexOf(vm);
                        links.Remove(vm);
                        links.Insert(prevIndex - 1, vm);
                        this.ComponentList.ItemsSource = null;
                        this.ComponentList.ItemsSource = links;
                        this.ComponentList.SelectedItem = vm;
                        setOrder();
                    }, () => links.IndexOf(vm) > 0);
                vm.DownCommand = new RelayCommand(
                    () =>
                    {
                        var prevIndex = links.IndexOf(vm);
                        links.Remove(vm);
                        links.Insert(prevIndex + 1, vm);
                        this.ComponentList.ItemsSource = null;
                        this.ComponentList.ItemsSource = links;
                        this.ComponentList.SelectedItem = vm;
                        setOrder();
                    }, () => links.IndexOf(vm) < links.Count - 1);
            }
            this.ComponentList.ItemsSource = links;
        }
    }

    public class ComponentsOrderingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public System.Windows.Input.ICommand UpCommand { get; set; }
        public System.Windows.Input.ICommand DownCommand { get; set; }

        public IAbstractComponentLink Link { get; set; }

        public string Name 
        {
            get
            {
                return this.Link.ComponentReference.Value.Parent.Parent.InstanceName
                    + "." + this.Link.ComponentReference.Value.InstanceName;
            }
        }
    }
}
