using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archivist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage : ContentPage
    {
        public string Special = null;
        public string ListType = null;
        public FilterPage()
        {
            InitializeComponent();
        }
        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Movies_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (movie.IsChecked == true)
            {
                serie.IsChecked = false;
                game.IsChecked = false;
                movie.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Special = "Movies";
            }
        }
        private void Series_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (serie.IsChecked == true)
            {
                movie.IsChecked = false;
                game.IsChecked = false;
                serie.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Special = "Series";
            }
        }
        private void Games_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (game.IsChecked == true)
            {
                serie.IsChecked = false;
                movie.IsChecked = false;
                game.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Special = "Games";
            }
        }
        private void NewToOld_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (newest.IsChecked == true)
            {
                oldest.IsChecked = false;
                highest.IsChecked = false;
                lowest.IsChecked = false;
                newest.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                ListType = "Newest";
            }
        }
        private void OldToNew_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (oldest.IsChecked == true)
            {
                newest.IsChecked = false;
                highest.IsChecked = false;
                lowest.IsChecked = false;
                oldest.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                ListType = "Oldest";
            }
        }
        private void HighToLow_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (highest.IsChecked == true)
            {
                newest.IsChecked = false;
                oldest.IsChecked = false;
                lowest.IsChecked = false;
                highest.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                ListType = "Highest";
            }
        }
        private void LowToHigh_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (lowest.IsChecked == true)
            {
                newest.IsChecked = false;
                oldest.IsChecked = false;
                highest.IsChecked = false;
                lowest.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                ListType = "Lowest";
            }
        }
        private async void Filter_Clicked(object sender, EventArgs e)
        {
            if (Special == null || ListType == null)
            {
                Error.Text = "Please select item and sort type!";
                Error.TextColor = Color.FromHex("#db6400");
                await Task.Delay(2000);
                Error.TextColor = Color.Transparent;
            }
            else
            {
                try
                {
                    await Navigation.PushAsync(new FilterListPage(Special, ListType));
                }
                catch (Exception)
                {



                }
            }
            

        }


    }
}