namespace NodesOfTrees.Models.Dtos
{
    public class TreeNodeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
