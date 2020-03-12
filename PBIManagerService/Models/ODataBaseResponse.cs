using Newtonsoft.Json;
using System.Collections.Generic;

namespace Models
{
    public class ODataBaseResponse<T>
    {
        [JsonProperty(PropertyName = "value")]
        public IList<T> Value { get; set; }
    }
}
