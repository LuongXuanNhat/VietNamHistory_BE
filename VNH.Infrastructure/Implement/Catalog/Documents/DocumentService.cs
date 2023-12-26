using AutoMapper;
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
using VNH.Infrastructure.Implement.Common;
using DocumentFormat.OpenXml.Office2010.Excel;
using VNH.Application.DTOs.Catalog.Posts;
using VNH.Application.DTOs.Common;

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
            document.CreatedAt = DateTime.Now;
            string formattedDateTime = document.CreatedAt.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
            document.SubId = HandleCommon.SanitizeString(document.Title) + "-" + formattedDateTime;
            document.FilePath = await _document.SaveFile(requestDto.FileName, document.SubId);
            document.FileName = document.SubId;
            document.UserId = user.Id;
           
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


        public async Task<ApiResult<DocumentReponseDto>> Update(CreateDocumentDto requestDto, string name)
        {
            var user = await _userManager.FindByEmailAsync(name);
            
            var updateDocument = _dataContext.Documents.FirstOrDefault(x => x.SubId.Equals(requestDto.SubId));
            if (updateDocument is null)
            {
                return new ApiErrorResult<DocumentReponseDto>("Lỗi :Tài liệu không được cập nhập (không tìm thấy tài liệu)");
            }
            updateDocument.Title = requestDto.Title;
            string formattedDateTime = DateTime.Now.ToString("HHmmss.fff") + HandleCommon.GenerateRandomNumber().ToString();
            var Id = HandleCommon.SanitizeString(updateDocument.Title);
            updateDocument.SubId = Id.Trim().Replace(" ", "-") + "-" + formattedDateTime;
            if (requestDto.FileName is not null)
            {
                await _storageService.DeleteDocFileAsync(updateDocument.FileName);
                updateDocument.FilePath = await _document.SaveFile(requestDto.FileName, updateDocument.SubId);
            }
            updateDocument.UpdatedAt = DateTime.Now;
            updateDocument.Description = requestDto.Description;
            updateDocument.FileName = updateDocument.SubId;
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
            
            documentResponse.ViewNumber += 1;
            document.ViewNumber += 1;

            documentResponse.UserShort = new()
            {
                FullName = user.Fullname,
                Id = user.Id,
                Image = user.Image
            };

            _dataContext.Documents.Update(document);
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

        public async Task<ApiResult<NumberReponse>> GetSave(DocumentFpkDto docsFpk)
        {
            var docs = _dataContext.Documents.First(x => x.SubId.Equals(docsFpk.DocumentId));
            var check = await _dataContext.DocumentSaves.Where(x => x.DocumentId.Equals(docs.Id) && x.UserId == Guid.Parse(docsFpk.UserId)).FirstOrDefaultAsync();
            var numberSave = await _dataContext.DocumentSaves.Where(x => x.DocumentId.Equals(docs.Id)).CountAsync();

            if (check != null)
            {
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = numberSave });
            }
            return new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = numberSave});
        }

        public async Task<ApiResult<NumberReponse>> AddOrRemoveSaveDocs(DocumentFpkDto docsFpk)
        {
            var docs = await _dataContext.Documents.FirstOrDefaultAsync(x => x.SubId.Equals(docsFpk.DocumentId));
            if (docs is null)
            {
                return new ApiErrorResult<NumberReponse>("Không tìm thấy bài viết");
            }
            var check = _dataContext.DocumentSaves.Where(x => x.DocumentId == docs.Id && x.UserId == Guid.Parse(docsFpk.UserId)).FirstOrDefault();
            var mess = "";
            var saveNumber = await _dataContext.DocumentSaves.Where(x => x.DocumentId == docs.Id).CountAsync();
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
                return new ApiSuccessResult<NumberReponse>(new() { Check = true, Quantity = saveNumber + 1 });
            }
            else
            {
                _dataContext.DocumentSaves.Remove(check);
                await _dataContext.SaveChangesAsync();
                return new ApiSuccessResult<NumberReponse>(new() { Check = false, Quantity = saveNumber - 1 });
            }


        }
        public async Task<ApiResult<List<DocumentReponseDto>>> Search(string keyWord)
        {
            var users = await _dataContext.User.Where(x=>!x.IsDeleted).ToListAsync();
            var documents = new List<DocumentReponseDto>();
            string[] searchKeywords = keyWord.ToLower().Split(' ');
            var result = from document in _dataContext.Documents as IEnumerable<Document>
                         let titleWords = document.Title.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                         let searchPhrases = HandleCommon.GenerateSearchPhrases(searchKeywords)
                         let matchingPhrases = searchPhrases
                            .Where(phrase => titleWords.Contains(phrase))
                         where matchingPhrases.Any()
                         let matchCount = matchingPhrases.Count()
                         orderby matchCount descending
                         select new Document()
                         {
                             Id = document.Id,
                             UserId = document.UserId,
                             SubId = document.SubId,
                             Title = document.Title,
                             CreatedAt = document.CreatedAt,
                             UpdatedAt = document.UpdatedAt,
                             Description = document.Description
                         };

            foreach (var document in result)
            {
                var item = _mapper.Map<DocumentReponseDto>(document);
                var userShort = users.FirstOrDefault(x => x.Id == document.UserId);
                if (userShort is not null)
                {
                    item.UserShort.FullName = userShort.Fullname;
                    item.UserShort.Id = userShort.Id;
                    item.UserShort.Image = userShort.Image;
                }
                documents.Add(item);
            }
            return new ApiSuccessResult<List<DocumentReponseDto>>(documents);
        }

        public async Task<ApiResult<List<DocumentReponseDto>>> GetMyDocument(string userId)
        {
            var documents = await _dataContext.Documents.Where(x => x.UserId.Equals(Guid.Parse(userId)) && !x.IsDeleted).ToListAsync();
            var result = new List<DocumentReponseDto>();
            foreach (var item in documents)
            {
                var post = _mapper.Map<DocumentReponseDto>(item);
                post.SaveNumber = await _dataContext.DocumentSaves.Where(x => x.DocumentId.Equals(post.Id)).CountAsync();
                post.ViewNumber = await _dataContext.PostComments.Where(x => x.PostId.Equals(post.Id)).CountAsync();
                result.Add(post);
            }

            return new ApiSuccessResult<List<DocumentReponseDto>>(result);
        }

        public async Task<ApiResult<List<DocumentReponseDto>>> GetMySave(string userId)
        {
            Guid UserId = Guid.Parse(userId);
            var users = await _dataContext.User.Where(x => !x.IsDeleted).ToListAsync();

            var documents = await (
                from postSave in _dataContext.DocumentSaves
                join post in _dataContext.Documents on postSave.DocumentId equals post.Id
                where postSave.UserId == UserId
                select post
            ).ToListAsync();

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
                result.Add(document);
            }
            return new ApiSuccessResult<List<DocumentReponseDto>>(result);
        }

        public async Task SaveDownloads(Guid documentId)
        {
            var document = await _dataContext.Documents.FirstOrDefaultAsync(x=>!x.IsDeleted && x.Id == documentId);
            if(document is null)
            {
                return;
            }

            document.ViewNumber += 1;
            _dataContext.Documents.Update(document);
            await _dataContext.SaveChangesAsync();
        }
    }
}
