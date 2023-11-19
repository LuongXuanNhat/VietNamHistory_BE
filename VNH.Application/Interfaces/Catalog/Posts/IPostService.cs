﻿using VNH.Application.DTOs.Catalog.HashTags;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common.ResponseNotification;

namespace VNH.Application.Interfaces.Posts
{
    public interface IPostService
    {
        Task<ApiResult<PostResponseDto>> Create(CreatePostDto requestDto, string name);
        Task<ApiResult<PostResponseDto>> Update(CreatePostDto requestDto, string name);
        Task<ApiResult<PostResponseDto>> Detail(string Id);
        Task<ApiResult<List<PostResponseDto>>> GetAll();
        Task<ApiResult<string>> Delete(string id, string email);
        Task<ApiResult<string>> DeleteAdmin(string id);


        Task<ApiResult<int>> AddOrUnLikePost(PostFpkDto postFpk);
        Task<ApiResult<int>> AddOrRemoveSavePost(PostFpkDto postFpk);
        Task<ApiResult<string>> ReportPost(ReportPostDto reportPostDto);
        Task<List<ReportPostDto>> GetReport();
        Task<ApiResult<bool>> GetLike(PostFpkDto postFpk);
        Task<ApiResult<bool>> GetSave(PostFpkDto postFpk);
        Task<ApiResult<List<string>>> GetTopTags(int numberTag);
        Task<ApiResult<List<PostResponseDto>>> GetPostByTag(string tag);
    }
}