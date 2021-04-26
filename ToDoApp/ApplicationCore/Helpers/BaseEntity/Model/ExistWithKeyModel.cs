using System.Collections.Generic;

namespace ApplicationCore.Helpers.BaseEntity.Model
{
    public class ExistWithKeyModel
    {
        public string KeyName { get; set; }
        public object KeyValue { get; set; }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public IDictionary<string, object> WhereData { get; set; }
    }
}
