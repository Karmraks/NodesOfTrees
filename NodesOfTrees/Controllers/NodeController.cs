using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Primitives;
using NodesOfTrees.Abstractions.Interfaces;
using NodesOfTrees.Models;

namespace NodesOfTrees.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly INodeRepository _nodeRepository;

        public NodeController(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var nodes = await _nodeRepository.Get();
            return Ok(nodes);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Get(string name)
        {
            var nodes = await _nodeRepository.Find(name);
            return Ok(nodes);
        }

        //[HttpGet("test")]
        //public async Task<IActionResult> GetNodes()
        //{
        //    var tree = new PrefixTree(new []
        //    {
        //        "banana",
        //        "banan",
        //        "ololo"
        //    });
        //    Console.WriteLine(tree);
        //    return Ok(tree);
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NodeRequest request)
        {
            await _nodeRepository.Create(request.Name, request.ParentId);
            return Created("", request.Name);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var node = await _nodeRepository.Delete(id);
            return Ok(node);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] NodeRequest request, Guid nodeId)
        {
            await _nodeRepository.Update(nodeId, request.Name, request.ParentId);
            return Ok(request.Name);
        }
    }
}
