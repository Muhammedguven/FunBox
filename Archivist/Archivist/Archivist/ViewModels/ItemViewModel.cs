using Archivist.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Archivist.ViewModels
{
    public class ItemViewModel : BaseViewModel
    {
        static IMongoCollection<ListItem> ItemCollection;
        readonly static string dbName = "FunBox";
        readonly static string collectionName = "Items";
        static MongoClient client;

        private List<ListItem> _moviesList;
        private string _ItemName;
        private DateTime _Date;
        private int _Point;
        private string _Type;
        private string _PublisherEmail;
        private string _Description;
        private byte[] _Image;


        public ItemViewModel()
        {
            SaveItemCommand = new Command(InsertItem);
            DeleteUserCommand = new Command(DeleteUser);
        }




        public string ItemName
        {
            get { return _ItemName; }
            set { SetValue(ref _ItemName, value); }
        }

        public DateTime Date
        {
            get { return _Date; }
            set { SetValue(ref _Date, value); }

        }
        public int Point
        {
            get { return _Point; }
            set { SetValue(ref _Point, value); }
        }


        public string Type
        {
            get { return _Type; }
            set { SetValue(ref _Type, value); }
        }

        public byte[] Image
        {
            get { return _Image; }
            set { SetValue(ref _Image, value); }
        }

        public string PublisherEmail
        {
            get { return _PublisherEmail; }
            set { SetValue(ref _PublisherEmail, value); }
        }

        public string Description
        {
            get { return _Description; }
            set { SetValue(ref _Description, value); }
        }
        public List<ListItem> DonationAdvertList
        {
            get { return _moviesList; }
            set { SetValue(ref _moviesList, value); }

        }

        #region Get Functions

        public async Task<List<ListItem>> GetItems()
        {
            try
            {
                var donationAdverts = await MongoConnection
                    .Find(new BsonDocument())
                    .ToListAsync();
                return donationAdverts;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;

        }

        public async Task<List<ListItem>> GetMovies(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Movies") && f.PublisherEmail.Equals(email))
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetSeries(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Series") && f.PublisherEmail.Equals(email))
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetGames(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Games") && f.PublisherEmail.Equals(email))
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetMoviesHighRateFirst(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Movies") && f.PublisherEmail.Equals(email)).SortBy(f => f.Point)
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetSeriesHighRateFirst(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Series") && f.PublisherEmail.Equals(email)).SortBy(f => f.Point)
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetGamesHighRateFirst(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Games") && f.PublisherEmail.Equals(email)).SortBy(f => f.Point)
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetMoviesDateFirst(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Movies") && f.PublisherEmail.Equals(email)).SortBy(f => f.Date)
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetSeriesDateFirst(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Series") && f.PublisherEmail.Equals(email)).SortBy(f => f.Date)
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> GetGamesDateFirst(string email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.Type.Equals("Games") && f.PublisherEmail.Equals(email)).SortBy(f => f.Date)
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }
        public async Task<List<ListItem>> getUsersItems(String email)
        {
            try
            {
                var allItems = await MongoConnection
                    .Find(f => f.PublisherEmail.Equals(email))
                    .ToListAsync();
                return allItems;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return null;
        }





        public async Task<ListItem> getItemViaPublisherEmail(String email)
        {
            var advert = await MongoConnection
                .Find(f => f.PublisherEmail.Equals(email))
                .FirstOrDefaultAsync();

            return advert;
        }
        #endregion

        #region Search Functions 

        #endregion

        #region Save/Delete Functions
        public async void InsertItem()
        {
            var newItem = new ListItem
            {
                PublisherEmail = _PublisherEmail,
                Type = _Type,
                ItemName = _ItemName,
                Image = _Image,
                Date = _Date,
                Description = _Description,
                Point = _Point
                

            };

            await MongoConnection.InsertOneAsync(newItem);
        }
        public async void InsertItem(ListItem newItem)
        {
            await MongoConnection.InsertOneAsync(newItem);

        }

        public async void DeleteUser(object obj)
        {
            var items = (ListItem)obj;
            var result = await MongoConnection.DeleteOneAsync(tdi => tdi.ItemID == items.ItemID);


        }

        #endregion

        #region Command Functions

        public ICommand SaveMessageCommand { get; set; }
        public ICommand SaveItemCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }

        #endregion

        #region Connection
        public IMongoCollection<ListItem> MongoConnection
        {
            get
            {
                if (client == null || ItemCollection == null)
                {
                    ----ALETT!!!   Change Connection String with yours----
                    var connectionString = "mongodb://mguven7006:mguven123@cluster0-shard-00-00.hscqt.mongodb.net:27017,cluster0-shard-00-01.hscqt.mongodb.net:27017,cluster0-shard-00-02.hscqt.mongodb.net:27017/<dbname>?ssl=true&replicaSet=atlas-rlcztw-shard-0&authSource=admin&retryWrites=true&w=majority";
                    MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
                    client = new MongoClient(settings);
                    var db = client.GetDatabase(dbName);


                    var collectionSettings = new MongoCollectionSettings { ReadPreference = ReadPreference.Nearest };
                    ItemCollection = db.GetCollection<ListItem>(collectionName, collectionSettings);

                }
                return ItemCollection;
            }
        }
        #endregion
    }
}
