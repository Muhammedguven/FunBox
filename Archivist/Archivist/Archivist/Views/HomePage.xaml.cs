using Archivist.Models;
using Archivist.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Archivist.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : Shell
    {
        public byte[] imagebyte;
        public int Rate = 0;
        public ObservableCollection<ListItem> Movies { get; set; }
        public ObservableCollection<ListItem> Series { get; set; }
        public ObservableCollection<ListItem> Games { get; set; }
        ItemViewModel itemViewModel = new ItemViewModel();
        public HomePage()
        {
            InitializeComponent();

            Movies = new ObservableCollection<ListItem>();
            Series = new ObservableCollection<ListItem>();
            Games = new ObservableCollection<ListItem>();
            BindingContext = itemViewModel;

            SeriesList.ItemsSource = Series;
            MovieList.ItemsSource = Movies;
            GamesList.ItemsSource = Games;
            OnAppearing();
            MovieList.RefreshCommand = new Command(async () =>
            {
                var Mainapp = Application.Current as App;
                MovieList.RefreshControlColor = Color.FromHex("#db0000");
                MovieList.IsRefreshing = true;
                var movieList = await itemViewModel.GetMovies(Mainapp.Email);

                foreach (var item in movieList)
                {
                    if (!Movies.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                        Movies.Add(item);
                }
                MovieList.ItemsSource = Movies;
                MovieList.IsRefreshing = false;
            });
            SeriesList.RefreshCommand = new Command(async () =>
            {
                var Mainapp = Application.Current as App;
                SeriesList.RefreshControlColor = Color.FromHex("#ff7b54");
                SeriesList.IsRefreshing = true;
                var serieList = await itemViewModel.GetSeries(Mainapp.Email);

                foreach (var item in serieList)
                {
                    if (!Series.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                        Series.Add(item);
                }
                SeriesList.ItemsSource = Series;
                SeriesList.IsRefreshing = false;
            });
            GamesList.RefreshCommand = new Command(async () =>
            {
                var Mainapp = Application.Current as App;
                GamesList.RefreshControlColor = Color.FromHex("#28df99");
                GamesList.IsRefreshing = true;
                var gamesList = await itemViewModel.GetGames(Mainapp.Email);

                foreach (var item in gamesList)
                {
                    if (!Games.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                        Games.Add(item);
                }
                GamesList.ItemsSource = Games;
                GamesList.IsRefreshing = false;
            });

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await HomePageAppears();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
        private async Task HomePageAppears()
        {
            Loading.IsRunning = true;
            var Mainapp = Application.Current as App;

            var movieList = await itemViewModel.GetMovies(Mainapp.Email);
            var serieList = await itemViewModel.GetSeries(Mainapp.Email);
            var gameList = await itemViewModel.GetGames(Mainapp.Email);

            //List reversed to show people last items.
            Movies.Reverse();
            Series.Reverse();
            Games.Reverse();
            foreach (var item in movieList)
            {
                if (!Movies.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                {
                    Movies.Add(item);
                }
                    
            }
            foreach (var item in serieList)
            {
                if (!Series.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                    Series.Add(item);
            }
            foreach (var item in gameList)
            {
                if (!Games.Any((arg) => arg.ItemID == item.ItemID && arg.PublisherEmail == Mainapp.Email))
                    Games.Add(item);
            }

            Loading.IsRunning = false;

        }
        ImageButton btnMovie;
        ImageButton btnSerie;
        ImageButton btnGame;
        private void Movies_Deleted_Clicked(object sender, EventArgs e)
        {
            btnMovie = (ImageButton)sender;

            PopUpTitleMovie.Text = "Are you sure?";
            PopUpLabelMovie.Text = "Would you like to delete this item?";
            CancelMovie.IsVisible = true;
            popUpImageViewMovie.IsVisible = true;

        }
        private void Series_Deleted_Clicked(object sender, EventArgs e)
        {
            btnSerie = (ImageButton)sender;

            PopUpTitleSerie.Text = "Are you sure?";
            PopUpLabelSerie.Text = "Would you like to delete this item?";
            CancelSerie.IsVisible = true;
            popUpImageViewSerie.IsVisible = true;
        }
        private void Games_Deleted_Clicked(object sender, EventArgs e)
        {
            btnGame = (ImageButton)sender;

            PopUpTitleGame.Text = "Are you sure?";
            PopUpLabelGame.Text = "Would you like to delete this item?";
            CancelGame.IsVisible = true;
            popUpImageViewGame.IsVisible = true;
        }
        private void MovieList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
        private async void AddPhotoFromGallery(object sender, EventArgs e)
        {
            try
            {

                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return;
                }

                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                if (selectedImage == null)
                {
                    return;  
                }
                selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
                imagebyte = GetImageStreamAsBytes(selectedImageFile.GetStream());

                //TODO :Add selection of multichocice

            }
            catch (Exception)
            {
                selectedImage.Source = "camera.png";
                ErrorCamera.Text = "Could not get the image , please try again!";
                ErrorCamera.TextColor = Color.FromHex("#ffffff");
                await Task.Delay(2000);
                ErrorCamera.TextColor = Color.Transparent;
            }
        }
        private async void TakePhoto(object sender, EventArgs e)
        {
            try
            {

                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    return;
                }

                var mediaOptions = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                var selectedImageFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { AllowCropping = true, SaveToAlbum = true });


                if (selectedImage == null)
                {
                    ErrorCamera.Text = "Could not get the image , please try again!";
                    ErrorCamera.TextColor = Color.FromHex("#db6400");
                    await Task.Delay(2000);
                    ErrorCamera.TextColor = Color.Transparent;
                }
                selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
                imagebyte = GetImageStreamAsBytes(selectedImageFile.GetStream());

            }
            catch (Exception)
            {
            }
        }
        // Image to Byte Converter
        public byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async void Share_Clicked(object sender, EventArgs e)
        {
            var app = Application.Current as App;
            try
            {
                if (itemName.Text != null && Rate != 0 && shareTypePicker.SelectedItem != null)
                {
                    ListItem newItem = new ListItem
                    {
                        ItemName = itemName.Text,
                        Description = description.Text,
                        Type = shareTypePicker.SelectedItem.ToString(),
                        PublisherEmail = app.Email,
                        Image = imagebyte,
                        Date = date.Date,
                        Point = Rate
                    };
                    itemViewModel.InsertItem(newItem);
                    itemName.Text = "";
                    selectedImage.Source = "camera.png";
                    shareTypePicker.SelectedItem = null;
                    description.Text = "";
                    shareTypePicker.SelectedItem = null;
                    imagebyte = null;
                    PopUpTitleShare.Text = "Successful!";
                    PopUpLabelShare.Text = "Successfully Created";
                    popUpImageViewShare.IsVisible = true;
                    await HomePageAppears();
                }
                else
                {
                    Error.Text = "Please fill all of them!";
                    Error.TextColor = Color.FromHex("#ffffff");
                    await Task.Delay(2000);
                    Error.TextColor = Color.Transparent;
                }


            }
            catch
            {
                return;
            }

            




        }
        private void popUpButtonShare(object sender, EventArgs e)
        {
            if (PopUpTitleShare.Text.Equals("Successful!"))
            {
                popUpImageViewShare.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }
            else
            {
                popUpImageViewShare.IsVisible = false;
            }

        }

        private void popUpButtonMovieCancel(object sender, EventArgs e)
        {

            PopUpTitleMovie.Text = "Not Deleted.";
            popUpImageViewMovie.IsVisible = false;

        }
        private void popUpButtonSerieCancel(object sender, EventArgs e)
        {

            PopUpTitleSerie.Text = "Not Deleted.";
            popUpImageViewSerie.IsVisible = false;

        }
        private void popUpButtonGameCancel(object sender, EventArgs e)
        {

            PopUpTitleGame.Text = "Not Deleted.";
            popUpImageViewGame.IsVisible = false;

        }
        private void popUpButtonMovie(object sender, EventArgs e)
        {
            
            if (PopUpTitleMovie.Text.Equals("Are you sure?"))
            {
                PopUpTitleMovie.Text = "Deleted.";
                PopUpLabelMovie.Text = "Item has deleted successfully!";
                CancelMovie.IsVisible = false;
                popUpImageViewMovie.IsVisible = true;
                var ob = btnMovie.CommandParameter as ListItem;
                itemViewModel.DeleteUser(ob);
            }
            else if (PopUpTitleMovie.Text.Equals("Deleted"))
            {
                popUpImageViewMovie.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }
            else
            {
                popUpImageViewMovie.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }

        }
        private void popUpButtonSerie(object sender, EventArgs e)
        {

            if (PopUpTitleSerie.Text.Equals("Are you sure?"))
            {
                PopUpTitleSerie.Text = "Deleted.";
                PopUpLabelSerie.Text = "Item has deleted successfully!";
                CancelSerie.IsVisible = false;
                popUpImageViewSerie.IsVisible = true;
                var ob = btnSerie.CommandParameter as ListItem;
                itemViewModel.DeleteUser(ob);
            }
            else if (PopUpTitleSerie.Text.Equals("Deleted"))
            {
                popUpImageViewSerie.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }
            else
            {
                popUpImageViewSerie.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }

        }
        private void popUpButtonGame(object sender, EventArgs e)
        {

            if (PopUpTitleGame.Text.Equals("Are you sure?"))
            {
                PopUpTitleGame.Text = "Deleted.";
                PopUpLabelGame.Text = "Item has deleted successfully!";
                CancelGame.IsVisible = false;
                popUpImageViewGame.IsVisible = true;
                var ob = btnGame.CommandParameter as ListItem;
                itemViewModel.DeleteUser(ob);
            }
            else if (PopUpTitleGame.Text.Equals("Deleted"))
            {
                popUpImageViewGame.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }
            else
            {
                popUpImageViewGame.IsVisible = false;
                App.Current.MainPage = new HomePage();
            }

        }


       

        private void ten_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ten.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = true;
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 10;
            }

        }
        private void one_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (one.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 1;
            }
        }
        private void two_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (two.IsChecked == true)
            {
                three.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                two.IsChecked = true;
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 2;
            }
        }
        private void three_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (three.IsChecked == true)
            {
                two.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                three.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 3;
            }
        }

        private void four_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (four.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                four.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 4;
            }
        }

        private void five_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (five.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                four.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                five.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 5;
            }
        }

        private void six_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (six.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                six.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 6;
            }
        }

        private void seven_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (seven.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                eight.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                seven.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 7;
            }
        }

        private void eight_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (eight.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                nine.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                eight.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 8;
            }
        }

        private void nine_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (nine.IsChecked == true)
            {
                two.IsChecked = false;
                three.IsChecked = false;
                four.IsChecked = false;
                five.IsChecked = false;
                six.IsChecked = false;
                seven.IsChecked = false;
                eight.IsChecked = false;
                ten.IsChecked = false;
                one.IsChecked = false;
                nine.IsChecked = true;///////
            }
            var result = e.Value;

            if (result == true)
            {
                Rate = 9;
            }
        }

        private void Movies_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var app = Application.Current as App;
            var searchMovieResult = Movies.Where(c => c.ItemName.ToLower().Contains(moviesSearchBar.Text.ToLower()) && c.Type.ToString().Equals("Movies") && c.PublisherEmail.Equals(app.Email));
            MovieList.ItemsSource = searchMovieResult;
        }
        private void Series_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var app = Application.Current as App;
            var searchSerieResult = Series.Where(c => c.ItemName.ToLower().Contains(seriesSearchBar.Text.ToLower()) && c.Type.ToString().Equals("Series") && c.PublisherEmail.Equals(app.Email));
            SeriesList.ItemsSource = searchSerieResult;
        }
        private void Games_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var app = Application.Current as App;
            var searchGameResult = Games.Where(c => c.ItemName.ToLower().Contains(gamesSearchBar.Text.ToLower()) && c.Type.ToString().Equals("Games") && c.PublisherEmail.Equals(app.Email));
            GamesList.ItemsSource = searchGameResult;
        }
        private async void Logout_Clicked(object sender, EventArgs e)
        {

            bool answer = await DisplayAlert("Are you sure?", "Would you like to log out?", "Yes", "No");

            if (answer.Equals(true))
            {
                var app = Application.Current as App;
                app.Email = "";
                app.LoggedIn = "false";
                App.Current.MainPage = new NavigationPage(new LoginPage());
            }

        }

        private async void Filter_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FilterPage());
        }
    }
}