using System.Text.Json;

namespace NodesOfTrees.Models
{
    public class ErrorDetails
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public object Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
