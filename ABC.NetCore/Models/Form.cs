using System.ComponentModel;
using Newtonsoft.Json;

namespace ABC.NetCore.Models
{
    public class Form : Collection<FormField>
    {
        public const string Relation = "form";
        public const string EditRelation = "edit-form";
        public const string CreateRelation = "create-form";
        public const string QueryRelation = "query-form";
    }

    public class FormField
    {
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FormFieldOption[] Options { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Pattern { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Required { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Secret { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
        [DefaultValue(DefaultType)]
        public string Type { get; set; } = DefaultType;
        public const string DefaultType = "string";

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MinLength { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxLength { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Value { get; set; }
    }

    public class FormFieldOption
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        public object Value { get; set; }
    }
}
