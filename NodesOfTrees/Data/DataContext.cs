using Microsoft.EntityFrameworkCore;
using NodesOfTrees.Models;
using System.Data;

namespace NodesOfTrees.Data
{
    public class DataContext : DbContext
    {
        private readonly Guid _rootTreeId = Guid.NewGuid();
        private readonly Guid _firstChildRootTreeId = Guid.NewGuid();
        private readonly Guid _secondChildRootTreeId = Guid.NewGuid();
        private readonly Guid _firstChildTreeId = Guid.NewGuid();

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<TreeNode> TreeNodes { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TreeNode>()
                .HasOne(n => n.Parent)
                .WithMany(n => n.Children)
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedNodeTrees(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedNodeTrees(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TreeNode>()
                .HasData(new TreeNode()
                {
                    Id = _rootTreeId,
                    Name = "RootTree",
                    ParentId = null
                });

            modelBuilder.Entity<TreeNode>()
                .HasData(new TreeNode()
                {
                    Id = _firstChildRootTreeId,
                    Name = "1 - childRoot",
                    ParentId = _rootTreeId
                });

            modelBuilder.Entity<TreeNode>()
                .HasData(new TreeNode()
                {
                    Id = _secondChildRootTreeId,
                    Name = "2 - childRoot",
                    ParentId = _rootTreeId
                });

            modelBuilder.Entity<TreeNode>()
                .HasData(new TreeNode()
                {
                    Id = _firstChildTreeId,
                    Name = "1 - child - 2 - childRoot",
                    ParentId = _secondChildRootTreeId
                });
        }
    }
}
