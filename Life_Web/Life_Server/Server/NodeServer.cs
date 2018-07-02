using AutoMapper;
using life_Comme;
using Life_Core_Repositories;
using Life_Paragraph_Core;
using Life_Web.Data;
using Microsoft.Extensions.Caching.Memory;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    public class NodeServer : Repository<Node>, INodeServer
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        public NodeServer(ApplicationDbContext dbContext,IMapper mapper,
            IMemoryCache memoryCache) : base(dbContext)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<List<NodeViewModel>> GetAllNode()
        {
            string cacheKey = "NodeList";
            var result = new List<NodeViewModel>();
            if (_memoryCache.TryGetValue(cacheKey, out result))
            {
                return _memoryCache.Get(cacheKey) as List<NodeViewModel>;
            }
            else
            {
                var entity = await GetAllAsync();
                result = _mapper.Map<List<NodeViewModel>>(entity);
                _memoryCache.Set(cacheKey, result);
                return result;
            }
        }

        public string GetNodeNameById(int id)
        {
            var entity = GetById(id);
            var viewMode = _mapper.Map<NodeViewModel>(entity);
            return viewMode.NodeName;
        }

    }
}
