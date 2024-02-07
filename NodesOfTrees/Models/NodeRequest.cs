using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NodesOfTrees.Models
{
    public class NodeRequest
    {
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
