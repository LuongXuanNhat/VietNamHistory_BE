using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Common.ResponseNotification;
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

        public async Task<ApiResult<List<string>>> GetAllTag(int numberTag)
        {
            if (numberTag <= 0)
            {
                var tags = await _dataContext.Tags
                                            .GroupBy(x => x.Name)
                                            .Select(group => new { Name = group.Key, Count = group.Sum(t => 1) })
                                            .OrderByDescending(tagName => tagName.Count)
                                            .Select(group => group.Name)
                                            .ToListAsync();
                return new ApiSuccessResult<List<string>>(tags);
            } else
            {
                var tags = await _dataContext.Tags
                            .GroupBy(x => x.Name)
                            .Select(group => new { Name = group.Key, Count = group.Sum(t => 1) })
                            .OrderByDescending(tagName => tagName.Count)
                            .Take(numberTag)
                            .Select(group => group.Name)
                            .ToListAsync();
                return new ApiSuccessResult<List<string>>(tags);
            } 
        }
    }
}
