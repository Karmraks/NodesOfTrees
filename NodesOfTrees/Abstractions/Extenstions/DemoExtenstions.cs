using NodesOfTrees.Models;
using NodesOfTrees.Models.Dtos;

namespace NodesOfTrees.Abstractions.Extenstions
{
    public static class DemoExtenstions
    {
        public static TreeNode ToEntity(this TreeNodeDto treeNodeDto)
        {
            return new TreeNode()
            {
                Id = treeNodeDto.Id,
                Name = treeNodeDto.Name,
                ParentId = treeNodeDto.ParentId
            };
        }
        

        public static TreeNodeDto ToDto(this TreeNode treeNode)
        {
            return new TreeNodeDto()
            {
                Id = treeNode.Id,
                Name = treeNode.Name,
                ParentId = treeNode.ParentId
            };
        }
    }
}
