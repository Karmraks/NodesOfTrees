using NodesOfTrees.Models;
using NodesOfTrees.Models.Dtos;

namespace NodesOfTrees.Abstractions.Interfaces
{
    public interface INodeRepository
    {
        Task<IEnumerable<TreeNodeDto>> Get();
        Task<TreeNode?> GetById(Guid id);
        Task Create(string name, Guid? parentId = null);
        Task<TreeNode?> Delete(Guid id);
        Task Update(Guid nodeId, string newName, Guid? newParentId = null);
        Task<List<TreeNodeDto>> Find(string name);
    }
}
