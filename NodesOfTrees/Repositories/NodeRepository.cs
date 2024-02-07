using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodesOfTrees.Abstractions.Interfaces;
using NodesOfTrees.Data;
using NodesOfTrees.Models;
using System.Xml.Linq;
using NodesOfTrees.Abstractions.Extenstions;
using NodesOfTrees.Models.Dtos;
using static NpgsqlTypes.NpgsqlTsQuery;

namespace NodesOfTrees.Repositories
{
    public class NodeRepository : INodeRepository
    {
        private readonly DataContext _context;

        public NodeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TreeNodeDto>> Get()
        {
            return await _context.TreeNodes
                .Select(node => node.ToDto())
                .ToListAsync();
        }

        public async Task<TreeNode?> GetById(Guid id)
        {
            return await _context.TreeNodes.FindAsync(id);
        }

        public async Task<List<TreeNodeDto>> Find(string name)
        {
            return await _context.TreeNodes
                .Where(n => EF.Functions.ILike(n.Name, $"%{name}%"))
                .Select(node => node.ToDto())
                .ToListAsync();
        }

        public async Task Create(string name, Guid? parentId = null)
        {
            var id = Guid.NewGuid();
            if (parentId.HasValue)
            {
                var parent = await _context.TreeNodes
                    .Include(n => n.Children)
                    .FirstOrDefaultAsync(n => n.Id == parentId);

                if (parent == null)
                {
                    throw new ArgumentException("Parent node not found.");
                }

                var node = new TreeNode { Id = id, Name = name, ParentId = parentId };
                parent.Children.Add(node);
                await _context.TreeNodes.AddAsync(node);
            }
            else
            {
                var node = new TreeNode { Id = id, Name = name, ParentId = parentId };
                await _context.TreeNodes.AddAsync(node);
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<TreeNode?> Delete(Guid id)
        {
            var node = await _context.TreeNodes
                .Include(n => n.Children)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (node != null)
            {
                if (node.Children.Any())
                {
                    throw new SecureException("You have to delete all children nodes first.");
                }

                _context.TreeNodes.Remove(node);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Node not found.");
            }

            return node;
        }

        public async Task Update(Guid nodeId, string newName, Guid? newParentId = null)
        {
            var nodeToUpdate = await GetById(nodeId);
            if (nodeToUpdate == null)
            {
                throw new ArgumentException("Node not found.");
            }

            if (!string.IsNullOrEmpty(newName))
            {
                nodeToUpdate.Name = newName;
            }

            if (newParentId.HasValue && newParentId.Value != nodeToUpdate.ParentId)
            {
                var newParent = await _context.TreeNodes.FindAsync(newParentId.Value);
                if (newParent == null)
                {
                    throw new ArgumentException("New parent node not found.");
                }

                nodeToUpdate.ParentId = newParentId;
            }
            else if (newParentId == null)
            {
                nodeToUpdate.ParentId = null;
            }

            _context.TreeNodes.Update(nodeToUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
