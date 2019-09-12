using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Hello.Entities
{
    public class PlController
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public int MaxDigital { get; set; }
        public int MaxCapacityInKilobytes { get; set; }
        public IList<string> NetworkInterfaces { get; set; }
    }
}
