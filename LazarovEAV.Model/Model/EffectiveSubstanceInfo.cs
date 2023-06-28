using Newtonsoft.Json;

namespace LazarovEAV.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class EffectiveSubstanceInfo
    {
        public string Name { get; set; }
        public SubstanceType Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        public string Quantity { get; set; }
    }
}
