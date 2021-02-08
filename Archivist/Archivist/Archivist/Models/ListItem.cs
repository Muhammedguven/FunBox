using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archivist.Models
{
    public class ListItem
    {
        [BsonId, BsonElement("AdvertID")]
        public Guid ItemID { get; set; }

        [BsonElement("DayImage")]
        public byte[] Image { get; set; }


        [BsonElement("PublisherEmail")]
        public string PublisherEmail { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("ItemName")]
        public string ItemName { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Point")]
        public int Point { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }
    

    }
}
