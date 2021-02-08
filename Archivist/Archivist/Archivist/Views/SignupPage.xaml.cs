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
    public partial class SignupPage : ContentPage
    {
        public ObservableCollection<string> GetUsers { get; set; }
        public SignupPage()
        {
            InitializeComponent();
            GetUsers = new ObservableCollection<string>();
            
        }
       
        public void ShowPass(object sender, EventArgs args)
        {
            password.IsPassword = password.IsPassword ? false : true;
        }
        public void ShowPassVerify(object sender, EventArgs args)
        {
            verifyPassword.IsPassword = verifyPassword.IsPassword ? false : true;
        }
        private async void Back_Clicked(object sender, EventArgs e)
        {
            
               await Navigation.PopAsync();

        }
        private async void popUpButton(object sender, EventArgs e)
        {
            if (PopUpTitle.Text.Equals("Successful!"))
            {
                popUpImageView.IsVisible = false;
                await Navigation.PopAsync();
            }
            else
            {
                popUpImageView.IsVisible = false;
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            UserViewModel viewModel = new UserViewModel();
            foreach (var item in await viewModel.GetAllUsers())
            {
                GetUsers.Add(item.email);
            }
            if (password.Text != null && email.Text != null && verifyPassword.Text != null)
            {
                if (password.Text == verifyPassword.Text)
                {
                    var user = new User
                    {
                        email = email.Text,
                        password = password.Text
                    };
                    if (GetUsers.Contains(user.email))
                    {
                        PopUpTitle.Text = "Error!";
                        PopUpLabel.Text = "You have registered already!";
                        popUpImageView.IsVisible = true;
                    }
                    else
                    {
                        try
                        {
                            viewModel.InsertUser(user);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                        
                        password.Text = null;
                        email.Text = null;
                        verifyPassword.Text = null;
                        PopUpTitle.Text = "Successful!";
                        PopUpLabel.Text = "You have registered successfully!";
                        popUpImageView.IsVisible = true;
                    }
                    
                }
                else
                {
                    Error.Text = "Passwords do not match!";
                    Error.TextColor = Color.FromHex("#045762");
                    await Task.Delay(2000);
                    Error.TextColor = Color.Transparent;
                }
            }
            else
            {
                Error.Text = "Please fill all of them!";
                Error.TextColor = Color.FromHex("#045762");
                await Task.Delay(2000);
                Error.TextColor = Color.Transparent;
            }
        }
    }
}