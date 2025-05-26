using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BikeRental.DDD.Domain.DTOs.User
{
    public class UserSignUpFacebookInfoDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }        

        [JsonProperty("picture")]
        public PictureData Picture { get; set; }
    }

    public partial class PictureData
    {
        [JsonProperty("data")]
        public PictureDataInfo Data { get; set; }
    }

    public partial class PictureDataInfo
    {
        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("is_silhouette")]
        public string IsSilhouette { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public string Width { get; set; }        
    }
}
