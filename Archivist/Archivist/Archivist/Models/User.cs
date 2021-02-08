using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Archivist.Models
{
    public class User
    {

        [BsonId, BsonElement("Email")]
        public string email { get; set; }

        [BsonElement("Password")]
        public string password { get; set; }





    }
}
