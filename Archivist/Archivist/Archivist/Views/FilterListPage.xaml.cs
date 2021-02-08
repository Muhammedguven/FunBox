using Archivist.Models;
using Archivist.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archivist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterListPage : ContentPage
    {
        public ObservableCollection<ListItem> SpecialList { get; set; }
        ItemViewModel itemViewModel = new ItemViewModel();

        public string CName { get; }
        public string LType { get; }
        public FilterListPage()
        {
            InitializeComponent();
        }
        public FilterListPage(string cname, string ltype)
        {
            InitializeComponent();
            CName = cname;
            LType = ltype;
            // LType = listType;
            this.filterName.Text = CName;
            SpecialList = new ObservableCollection<ListItem>();
            BindingContext = itemViewModel;

            FilterList.ItemsSource = SpecialList;

        }
        private async void Back_Clicked(object sender, EventArgs e)
        {

            await Navigation.PopAsync();

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await FilterPageAppears();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }

        private async Task FilterPageAppears()
        {
            var Mainapp = Application.Current as App;

            if (CName.Equals("Movies"))
            {

                if (LType.Equals("Highest"))
                {
                    topLabel.Text = "High to Low Rate";
                    var List = await itemViewModel.GetMoviesHighRateFirst(Mainapp.Email);
                    List.Reverse();
                    foreach (var item in List)
                    {
                        //High Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Lowest"))
                {
                    topLabel.Text = "Low to High Rate";
                    var List = await itemViewModel.GetMoviesHighRateFirst(Mainapp.Email);
               
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Newest"))
                {
                    topLabel.Text = "Newest to Oldest";
                    var List = await itemViewModel.GetMoviesDateFirst(Mainapp.Email);
                    List.Reverse();
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Oldest"))
                {
                    topLabel.Text = "Oldest to Newest";
                    var List = await itemViewModel.GetMoviesDateFirst(Mainapp.Email);
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }



            }
            else if (CName.Equals("Series"))
            {
                if (LType.Equals("Highest"))
                {
                    topLabel.Text = "High to Low Rate";
                    var List = await itemViewModel.GetSeriesHighRateFirst(Mainapp.Email);
                    List.Reverse();
                    foreach (var item in List)
                    {
                        //High Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Lowest"))
                {
                    topLabel.Text = "Low to High Rate";
                    var List = await itemViewModel.GetSeriesHighRateFirst(Mainapp.Email);
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Newest"))
                {
                    topLabel.Text = "Newest to Oldest";
                    var List = await itemViewModel.GetSeriesDateFirst(Mainapp.Email);
                    List.Reverse();
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Oldest"))
                {
                    topLabel.Text = "Oldest to Newest";
                    var List = await itemViewModel.GetSeriesDateFirst(Mainapp.Email);
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
            }

            else if (CName.Equals("Games"))
            {
                if (LType.Equals("Highest"))
                {
                    topLabel.Text = "High to Low Rate";
                    var List = await itemViewModel.GetGamesHighRateFirst(Mainapp.Email);
                    List.Reverse();
                    foreach (var item in List)
                    {
                        //High Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Lowest"))
                {
                    topLabel.Text = "Low to High Rate";
                    var List = await itemViewModel.GetGamesHighRateFirst(Mainapp.Email);
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Newest"))
                {
                    topLabel.Text = "Newest to Oldest";
                    var List = await itemViewModel.GetGamesDateFirst(Mainapp.Email);
                    List.Reverse();
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
                else if (LType.Equals("Oldest"))
                {
                    topLabel.Text = "Oldest to Newest";
                    var List = await itemViewModel.GetGamesDateFirst(Mainapp.Email);
                    foreach (var item in List)
                    {
                        //Low Rate First
                        if (!SpecialList.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                            SpecialList.Add(item);
                    }
                }
            }
            else
            {
                
            }





        }
    }
}