using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Archivist.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Xamarin.Forms;

namespace Archivist.ViewModels
{
    class UserViewModel : BaseViewModel
    {
        static IMongoCollection<User> usersCollection;
        readonly static string dbName = "FunBox";
        readonly static string collectionName = "Users";
        static MongoClient client;

        private string _email;
        private string _password;


        public UserViewModel()
        {
            SaveUserCommand = new Command(InsertUser);
        }

        public string Email
        {
            get { return _email; }
            set { SetValue(ref _email, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetValue(ref _password, value); }

        }


        #region Get Functions
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var users = await MongoConnection
                    .Find(new BsonDocument())
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;

        }
        public async Task<User> GetUserByEmail(String email)
        {
            var user = await MongoConnection
                .Find(f => f.email.Equals(email))
                .FirstOrDefaultAsync();

            return user;
        }

        public async void InsertUser()
        {
            var newUser = new User
            {
                email = Email,
                password = Password
            };
            try
            {

                await MongoConnection.InsertOneAsync(newUser);

            }
            catch (Exception)
            {

            }
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async void InsertUser(User newUser)
        {
            try
            {
                await MongoConnection.InsertOneAsync(newUser);
            }
            catch
            {

            }
        }


        #endregion

        #region Command Functions

        public ICommand AddUserCommand { get; set; }

        public ICommand SaveUserCommand { get; set; }

        #endregion

        #region Connection
        public IMongoCollection<User> MongoConnection
        {
            get
            {
                if (client == null || usersCollection == null)
                {
                    ----ALETT!!!   Change Connection String with yours----
                    var connectionString = "mongodb://mguven7006:mguven123@cluster0-shard-00-00.hscqt.mongodb.net:27017,cluster0-shard-00-01.hscqt.mongodb.net:27017,cluster0-shard-00-02.hscqt.mongodb.net:27017/<dbname>?ssl=true&replicaSet=atlas-rlcztw-shard-0&authSource=admin&retryWrites=true&w=majority";
                    MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
                    client = new MongoClient(settings);
                    var db = client.GetDatabase(dbName);


                    var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    usersCollection = db.GetCollection<User>(collectionName, collectionSettings);

                }
                return usersCollection;
            }
        }
        #endregion


    }
}
