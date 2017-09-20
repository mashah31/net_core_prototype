using Newtonsoft.Json;

using ABC.NetCore.Models;

namespace ABC.NetCore.Models
{
    public abstract class Resource : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
