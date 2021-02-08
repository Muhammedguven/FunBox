using Archivist.Models;
using Archivist.ViewModels;
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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public void ShowPass(object sender, EventArgs args)
        {
            password.IsPassword = password.IsPassword ? false : true;
        }
        private async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            Loading.IsRunning = true;
            if (email.Text != null)
            {
                if (password.Text != null)
                {
                    UserViewModel userView = new UserViewModel();
                    var app = Application.Current as App;
                    User user;
                    try
                    {

                        user = await userView.GetUserByEmail(email.Text);

                        if (email.Text.Equals(user.email) && password.Text.Equals(user.password))
                        {
                            app.Email = email.Text;
                            app.LoggedIn = "true";
                            Loading.IsRunning = false;
                            App.Current.MainPage = new HomePage();

                        }
                        else
                        {
                            Loading.IsRunning = false;
                            Error.Text = "Incorrect email/password.Try Again.";
                            Error.TextColor = Color.FromHex("#045762");
                            await Task.Delay(2000);
                            Error.TextColor = Color.Transparent;
                            return;
                        }

                    }
                    catch (Exception)
                    {
                        Loading.IsRunning = false;
                        Error.Text = "User not found. Please sign up.";
                        Error.TextColor = Color.FromHex("#045762");
                        await Task.Delay(2000);
                        Error.TextColor = Color.Transparent;
                        return;
                    }
                }
                else
                {
                    Loading.IsRunning = false;
                    Error.Text = "Please fill your password!";
                    Error.TextColor = Color.FromHex("#045762");
                    await Task.Delay(2000);
                    Error.TextColor = Color.Transparent;
                }

            }
            else
            {
                Loading.IsRunning = false;
                Error.Text = "Please fill your e-mail and password!";
                Error.TextColor = Color.FromHex("#045762");
                await Task.Delay(2000);
                Error.TextColor = Color.Transparent;
            }
        }

        private void HelpedButton(object sender, EventArgs e)
        {
            popUpImageView.IsVisible = false;
        }

        private void HelpButton_Clicked(object sender, EventArgs e)
        {
            popUpImageView.IsVisible = true;
        }
    }
}