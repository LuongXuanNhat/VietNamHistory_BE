using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.Interfaces.Catalog.HashTags;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;

namespace VNH.Infrastructure.Implement.Catalog.HashTags
{
    public class TagService : IHashTag
    {
        private readonly VietNamHistoryContext _dataContext;

        public TagService(VietNamHistoryContext dataContext) { 
            _dataContext = dataContext;
        }
        public async Task AddTag(Tag tag)
        {
            _dataContext.Tags.Add(tag);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<TagDto>> GetAll()
        {
            var tags = await _dataContext.Tags
                .GroupBy(x => x.Name)
                .Select(group => group.First())
                .ToListAsync();
            List<TagDto> tagDtos = new List<TagDto>();
            foreach (var tag in tags)
            {
                tagDtos.Add(new()
                {
                    Name = tag.Name,
                    Id = tag.Id,
                });
            }
            return tagDtos;
        }
    }
}
