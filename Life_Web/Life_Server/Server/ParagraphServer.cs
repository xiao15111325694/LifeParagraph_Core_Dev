using AutoMapper;
using life_Comme;
using Life_Core_Repositories;
using Life_Untiy;
using Life_Web.Data;
using Life_Web.Models;
using Microsoft.Extensions.Caching.Memory;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Server
{
    /// <summary>
    /// 
    /// </summary>
    public class ParagraphServer: Repository<Paragraph>, IParagraphServer
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IParagrphNodeServer _paragrphNodeServer;
        private readonly IHtmlPack _htmlPack;
        public ParagraphServer(ApplicationDbContext dbContext, IMapper mapper,
            IMemoryCache memoryCache,
            IParagrphNodeServer paragrphNodeServer,
            IHtmlPack htmlPack) : base(dbContext)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _paragrphNodeServer = paragrphNodeServer;
            _htmlPack = htmlPack;
        }

        public async Task<List<ParagraphViewModel>> FindAllAsync()
        {
            var entity = await GetAllAsync();
            var entityViewModel = new List<ParagraphViewModel>();
            if (entity != null)
            {
                entityViewModel = _mapper.Map<List<ParagraphViewModel>>(entity);
            }
            return entityViewModel;
        }

        
        public async Task<List<Paragraph>> FindAsync()
        {
            return await GetAllAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ParagraphViewModel>> GetAllParagraphAsync(SelectParagrphEntity select)
        {
            string cacheKey = "ParagraphList";
            if (_memoryCache.TryGetValue(cacheKey, out List<ParagraphViewModel> result))
            {
                return (List<ParagraphViewModel>)_memoryCache.Get(cacheKey);
            }
            else
            {
                var entity = await GetAllAsync();
                var ParModel = entity.Where(x => x.Type == ParagraphType.Hot && x.CreateTime >= select.Time).ToList();
                result = _mapper.Map<List<ParagraphViewModel>>(ParModel);
                _memoryCache.Set(cacheKey, result, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30)));
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ParagraphViewModel> GetByIDAsync(int id)
        {
            var entity = await GetByIdAsync(id);
           
            var entityViewModel = new ParagraphViewModel();
            if (entity != null)
            {
                entity.ThumbsupCount++;
                Update(entity);
                entityViewModel = _mapper.Map<ParagraphViewModel>(entity);
            }
            return entityViewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task<List<ParagraphViewModel>> GetByNodeId(int nodeId)
        {

            var viewEntity = (from par in await FindAsync()
                              where par.NodeId == nodeId
                              select new ParagraphViewModel()
                              {
                                  ID = par.Id,
                                  Title = par.Title,
                                  CommentCount = par.CommentCount,
                                  ThumbsupCount = par.ThumbsupCount,
                                  Content = par.Content,
                                  Type = (int)par.Type,
                                  CreateTime = par.CreateTime
                              }).OrderByDescending(x => x.CreateTime).ToList();
            return viewEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ParagraphViewModel>> GetParagraphMostThumbsupCount()
        {
            var beginTime = DateTime.Now.Date.AddDays(-30);
            var endTime = DateTime.Now.Date;
            var result = (from p in await FindAllAsync()
                          where p.CreateTime >= beginTime && p.CreateTime <= endTime
                          select new ParagraphViewModel
                          {
                              Title = p.Title,
                              ThumbsupCount = p.ThumbsupCount,
                              CreateTime = p.CreateTime,
                              CommentCount = p.CommentCount,
                              ID = p.ID,
                              UserId = p.UserId
                          }).OrderByDescending(x => x.ThumbsupCount).Take(20).ToList();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public void HtmlPackPostParagraph()
        {
            HtmlPackHelp htmlPackHelp = new HtmlPackHelp();
            var htmlPackInfo = _htmlPack.GetAll();
            foreach (var item in htmlPackInfo)
            {
                var results = htmlPackHelp.PostPackHtmlData(item.HtmlUrl);
                foreach (var result in results)
                {
                    result.CreateTime = DateTime.Now.Date;
                    result.NodeId = item.NodeId.GetValueOrDefault();
                    result.LastUpdateTime = DateTime.Now.Date;
                    result.Type = (int)ParagraphType.Hot;
                    result.ThumbsupCount = 0;
                    ParagraphNode paragraphNode = new ParagraphNode();
                    PostParagraph(result);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addModels"></param>
        public void PostBachParagraph(List<ParagraphAddModel> addModels)
        {
            var entity = _mapper.Map<List<Paragraph>>(addModels);
            if (entity != null)
            {
                SaveList(entity);
            }
        }

        public void PostParagraph(ParagraphAddModel addmodel)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var entity = _mapper.Map<Paragraph>(addmodel);
                ParagraphNode paragraphNode = new ParagraphNode();
                if (entity != null)
                {
                    var Parentity = Save(entity);
                    paragraphNode.NodeId = Parentity.NodeId;
                    paragraphNode.ParagraphId = Parentity.Id;
                    paragraphNode.CreateTime = DateTime.Now;
                    _paragrphNodeServer.Save(paragraphNode);
                    unitOfWork.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void HtmlPackByUrlToContnt()
        {
            var entitys = GetAll().Where(x => x.Content == null).AsQueryable();
            HtmlPackHelp htmlPackHelp = new HtmlPackHelp();
            List<Paragraph> paragraphs = new List<Paragraph>();
            List<Dictionary<int, string>> dics = new List<Dictionary<int, string>>();
            foreach (var item in entitys)
            {
                var restul = htmlPackHelp.PostPackHtmlContnt(item.VidUrl, item.Id);
                dics.Add(restul);
            }
            foreach (var dic in dics)
            {
                foreach (KeyValuePair<int,string> item in dic)
                {
                    var entity = GetById(item.Key);
                    entity.Content = item.Value;
                    paragraphs.Add(entity);
                }
            }
            UpdateList(paragraphs);
        }
    }
}
