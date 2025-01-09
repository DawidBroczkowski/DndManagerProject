using AutoMapper;
using DndManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resource = DndManager.Models.Resource;

namespace DndManager.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, Character>();
            CreateMap<AbilityScore, AbilityScore>();
            CreateMap<SavingThrow, SavingThrow>();
            CreateMap<Skill, Skill>();
            CreateMap<Currency, Currency>();
            CreateMap<Resource, Resource>();
            CreateMap<Attack, Attack>();
            CreateMap<Spell, Spell>();
            CreateMap<Item, Item>();
            CreateMap<Feat, Feat>();
        }
    }
}
