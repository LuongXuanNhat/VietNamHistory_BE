﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using VNH.Application.DTOs.Catalog.Document;
using VNH.Application.DTOs.Catalog.Users;
using VNH.Application.DTOs.Common.ResponseNotification;
using VNH.Application.Interfaces.Documents;
using VNH.Application.Interfaces.Common;
using VNH.Domain;
using VNH.Infrastructure.Presenters.Migrations;
using Microsoft.EntityFrameworkCore;
using VNH.Application.DTOs.Catalog.Posts;

namespace VNH.Infrastructure.Implement.Catalog.Documents
{
    public class DocumentService : IDocumentService
    {

        private readonly UserManager<User> _userManager;
        private readonly VietNamHistoryContext _dataContext;
        private readonly IFileService _document;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public DocumentService(UserManager<User> userManager,IMapper mapper,IFileService document,
            VietNamHistoryContext vietNamHistoryContext,IStorageService storageService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _document = document;
            _dataContext = vietNamHistoryContext;
            _storageService = storageService;
        }

        public async Task<ApiResult<DocumentReponseDto>> Create(CreateDocumentDto requestDto,string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            var document = _mapper.Map<Document>(requestDto);
            document.Id = Guid.NewGuid();
            document.FileName = await _document.SaveFile(requestDto.FileName);
            document.CreatedAt = DateTime.Now;
            document.UserId = user.Id;
            string formattedDateTime = document.CreatedAt.ToString("HH:mm:ss.fff-dd-MM-yyyy");
            document.SubId = SanitizeString(document.Title) + "-" + formattedDateTime;
            try
            {
                _dataContext.Documents.Add(document);
                await _dataContext.SaveChangesAsync();
                var documentReponse = _mapper.Map<DocumentReponseDto>(document);

                documentReponse.FileName = document.FileName;
                var userDto = new UserShortDto()
                {
                    FullName = user.Fullname,
                    Id = user.Id,
                    Image = user.Image,
                };
                documentReponse.UserShort = userDto;


                return new ApiSuccessResult<DocumentReponseDto>(documentReponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<DocumentReponseDto>("Lỗi lưu tài liệu : " + ex.Message);
            }

        }


        private static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        private static string SanitizeString(string input)
        {
            string withoutDiacritics = RemoveDiacritics(input).Trim().Replace(" ", "-");
            string sanitizedString = Regex.Replace(withoutDiacritics, "[^a-zA-Z0-9-]", "");

            return sanitizedString;
        }

        public async Task<ApiResult<DocumentReponseDto>> Update(CreateDocumentDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            
            var updateDocument = _dataContext.Documents
                .FirstOrDefault(x => x.SubId.Equals(requestDto.SubId));
            if (updateDocument is null)
            {
                return new ApiErrorResult<DocumentReponseDto>("Lỗi :Tài liệu không được cập nhập (không tìm thấy tài liệu)");
            }
            if (updateDocument.FileName != string.Empty)
            {
                await _storageService.DeleteFileAsync(updateDocument.FileName);
            }
            updateDocument.FileName = await _document.SaveFile(requestDto.FileName);
            updateDocument.UpdatedAt = DateTime.Now;
            updateDocument.Description = requestDto.Description;
            updateDocument.Title = requestDto.Title;    
            string formattedDateTime = DateTime.Now.ToString("HH:mm:ss.fff-dd-MM-yyyy");
            var Id = SanitizeString(updateDocument.Title);
            updateDocument.SubId = Id.Trim().Replace(" ", "-") + "-" + formattedDateTime;
            try
            {
                _dataContext.Documents.Update(updateDocument);
                await _dataContext.SaveChangesAsync();


                var documentResponse = _mapper.Map<DocumentReponseDto>(updateDocument);

                documentResponse.FileName = updateDocument.FileName;
                var useDto = new UserShortDto()
                {
                    FullName = user.Fullname,
                    Id = user.Id,
                    Image = user.Image
                };
                documentResponse.UserShort = useDto;

              

                return new ApiSuccessResult<DocumentReponseDto>(documentResponse);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<DocumentReponseDto>("Lỗi lưu tài liệu : " + ex.Message);
            }
        }

        public async Task<ApiResult<List<DocumentReponseDto>>> GetAll()
        {
            var documents = await _dataContext.Documents.ToListAsync();
            var users = await _dataContext.User.ToListAsync();

            var result = new List<DocumentReponseDto>();
            foreach (var item in documents)
            {
                var document = _mapper.Map<DocumentReponseDto>(item);
                var userShort = users.First(x => x.Id == item.UserId);
                if (userShort is not null)
                {
                    document.UserShort.FullName = userShort.Fullname;
                    document.UserShort.Id = userShort.Id;
                    document.UserShort.Image = userShort.Image;
                }
                document.FileName = item.FileName;

                result.Add(document);
            }

            return new ApiSuccessResult<List<DocumentReponseDto>>(result);
        }



        public async Task<ApiResult<DocumentReponseDto>> Detail(string Id)
        {
            var document = await _dataContext.Documents.FirstOrDefaultAsync(x=>x.SubId.Equals(Id));
            if (document is null)
            {
                return new ApiErrorResult<DocumentReponseDto>("Không tìm thấy tài liệu");
            }
            var user = await _userManager.FindByIdAsync(document
                .UserId.ToString());
            var documentResponse = _mapper.Map<DocumentReponseDto>(document);
            documentResponse.FileName = document.FileName;

                               
       
            documentResponse
                .UserShort = new()
            {
                FullName = user.Fullname,
                Id = user.Id,
                Image = user.Image
            };

           // _dataContext.Documents.Update(document);
            await _dataContext.SaveChangesAsync();
            return new ApiSuccessResult<DocumentReponseDto>(documentResponse);
        }

        public async Task<ApiResult<string>> Delete(string id, string userId)
        {
            var document = await _dataContext.Documents.FirstOrDefaultAsync(x => x.SubId.Equals(id) && x.UserId.ToString().Equals(userId));
            if (document is null)
            {
                return new ApiErrorResult<string>("Không tìm thấy tài liệu");
            }
            if (document.FileName != string.Empty)
            {
                await _storageService.DeleteFileAsync(document.FileName
                    );
            }
            _dataContext.Documents.Remove(document
                );

            await _dataContext.SaveChangesAsync();

            return new ApiSuccessResult<string>("Đã xóa tài liệu");
        }

        public async Task<ApiResult<bool>> GetSave(DocumentFpkDto docsFpk)
        {
            var docs = _dataContext.Documents.First(x => x.SubId.Equals(docsFpk.DocumentId));
            var check = await _dataContext.DocumentSaves.Where(x => x.DocumentId.Equals(docs.Id) && x.UserId == Guid.Parse(docsFpk.UserId)).FirstOrDefaultAsync();
            var reuslt = check != null;
            return new ApiSuccessResult<bool>(reuslt);
        }

        public async Task<ApiResult<bool>> AddOrRemoveSaveDocs(DocumentFpkDto docsFpk)
        {
            var docs = await _dataContext.Documents.FirstOrDefaultAsync(x => x.SubId.Equals(docsFpk.DocumentId));
            if (docs is null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy bài viết");
            }
            var check = _dataContext.DocumentSaves.Where(x => x.DocumentId == docs.Id && x.UserId == Guid.Parse(docsFpk.UserId)).FirstOrDefault();
            var mess = "";
            if (check is null)
            {
                var save = new DocumentSave()
                {
                    Id = Guid.NewGuid(),
                    DocumentId = docs.Id,
                    UserId = Guid.Parse(docsFpk.UserId)
                };
                _dataContext.DocumentSaves.Add(save);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<bool>(true);
            }
            else
            {
                _dataContext.DocumentSaves.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<bool>(false);
            }


        }


    }
}