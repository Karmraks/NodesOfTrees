using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NodesOfTrees.Models
{
    public class TreeNode
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public TreeNode? Parent { get; set; }

        public HashSet<TreeNode> Children { get; set; } = new();
    }
}
