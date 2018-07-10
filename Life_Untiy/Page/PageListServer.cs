﻿using Life_Paragraph_Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Untiy
{
    public class PageListServer<T> where T:class
    {
        public PageViewModel<T> GetPageList(List<T> entity, int pageIndex, int pageSize)
        {
            PageViewModel<T> pageViewModel = new PageViewModel<T>();
            if (entity.Count() > 0)
            {

                var pageModle = entity.Skip(pageIndex*pageSize).Take(pageSize).ToList();
                pageViewModel.ViewModel = pageModle;
                pageViewModel.Total = entity.Count();
                return pageViewModel;
            }
            else
            {
                return pageViewModel;
            }

        }

        public async Task<PageViewModel<T>> GetPageListAsync(List<T> entity, int pageIndex, int pageSize)
        {
            PageViewModel<T> pageViewModel = new PageViewModel<T>();
            if (entity.Count() > 0)
            {

                var pageModle = await Task.Run(() => entity.Skip(pageIndex * pageSize).Take(pageSize).ToList());
                pageViewModel.ViewModel = pageModle;
                pageViewModel.Total = entity.Count();
                return pageViewModel;
            }
            else
            {
                return pageViewModel;
            }
        }
    }
}
