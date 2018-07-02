using AutoMapper;
using life_Comme;
using Life_Paragraph_Core;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Life_Web.Life_Profile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Node, NodeViewModel>();
            CreateMap<Paragraph, ParagraphViewModel>();
            CreateMap<Paragraph, ParagraphAddModel>();
            CreateMap< ParagraphAddModel, Paragraph>();


            CreateMap<GameNodeViewModel, GameNode>();
            CreateMap<GameNode, GameNodeViewModel>();

            CreateMap<GameStrategyViewModel, GameStrategy>();
            CreateMap<GameStrategy, GameStrategyViewModel>();
        }
    }
}
