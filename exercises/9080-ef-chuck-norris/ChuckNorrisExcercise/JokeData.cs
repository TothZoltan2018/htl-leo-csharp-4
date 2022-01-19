using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChuckNorrisExcercise
{
    public class JokeData
    {
        public int Id { get; set; }

        [JsonPropertyName("id")]
        [MaxLength(40)]
        public string ChuckNorrisId { get; set; }

        [JsonPropertyName("url")]
        [MaxLength(1024)]
        public string Url { get; set; }

        [JsonPropertyName("value")]
        public string Joke { get; set; }

        [NotMapped]
        [JsonPropertyName("categories")]
        public string[] Categories { get; set; }
    }
}
