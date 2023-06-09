using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Misa.Web202303.SLN.BL.Service.FixedAssetCategory;
using Misa.Web202303.SLN.BL.ValidateDto;
using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.BL.ValidateDto.Decorators;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Error;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.BL.Service
{
    /// <summary>
    /// abstract class định nghĩa các phương thức dùng chung để tái sử dụng cho các service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TEntityUpdateDto"></typeparam>
    /// <typeparam name="TEntityCreateDto"></typeparam>
    public abstract class BaseService<TEntity, TEntityDto, TEntityUpdateDto, TEntityCreateDto> : IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto>
    {
        /// <summary>
        /// sử dụng dịch vị của  IBaseRepository
        /// </summary>
        protected readonly IBaseRepository<TEntity> _baseRepository;
        protected readonly IMapper _mapper;

        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="baseRepository"></param>
        /// <param name="mapper"></param>
        public BaseService(IBaseRepository<TEntity> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// phương thức lấy 1 bản ghi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public virtual async Task<TEntityDto> GetAsync(Guid id)
        {
            var entity = await _baseRepository.GetAsync(id);
            //bản ghi không tồn tại throw ra excception
            if (entity == null)
            {
                throw new NotFoundException()
                {
                    ErrorCode = ErrorCode.NotFound,
                    UserMessage = string.Format(ErrorMessage.NotFoundError, GetAssetName())
                };
            }
            var entityDto = _mapper.Map<TEntityDto>(entity);

            return entityDto;
        }

        /// <summary>
        /// lấy ra tất cả bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntityDto>> GetAsync()
        {
            var listTEntity = await _baseRepository.GetAsync();

            var result = listTEntity.Select(entity => _mapper.Map<TEntityDto>(entity));

            return result;
        }

        /// <summary>
        /// thêm mới 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task InsertAsync(TEntityCreateDto entityCreateDto)
        {
            // validate attribute
            var listError =  ValidateAttribute.Validate(entityCreateDto);
            // validate riêng
            listError = Enumerable.Concat(listError, await CreateValidateAsync(entityCreateDto)).ToList();
            // nếu validate có lỗi thì throw exception
            if(listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateCreateError
                    
                };
            }
            var entity = _mapper.Map<TEntity>(entityCreateDto);
            await _baseRepository.InsertAsync(entity);
        }

        /// <summary>
        /// update 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task UpdateAsync(Guid entityId, TEntityUpdateDto entityUpdateDto)
        {
            // kiểm tra bản ghi không tồn tại
            var oldEntity = await _baseRepository.GetAsync(entityId);
            if (oldEntity == null)
            {
                throw new NotFoundException()
                {
                    ErrorCode = ErrorCode.NotFound,
                    UserMessage = string.Format(ErrorMessage.NotFoundUpdateError, GetAssetName())
                };
            }
            // validate attribute
            var listError =  ValidateAttribute.Validate(entityUpdateDto);
            // validate riêng
            listError = Enumerable.Concat(listError, await UpdateValidateAsync(entityId, entityUpdateDto)).ToList();

            if (listError.Count > 0)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.DataValidate,
                    Data = listError,
                    UserMessage = ErrorMessage.ValidateUpdateError
                };
            }

            var entity = _mapper.Map<TEntity>(entityUpdateDto);
           await _baseRepository.UpdateAsync(entityId, entity);
        }


        /// <summary>
        /// kiểm tra mã code bị trùng khi thêm hoạc sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<bool> CheckCodeExisted(string code, Guid? id)
        {
            var result = await _baseRepository.CheckCodeExistedAsync(code, id);
            return result;
        }

        /// <summary>
        /// validate khi update dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityUpdateDto"></param>
        /// <returns></returns>
        protected virtual async Task<List<ValidateError>> UpdateValidateAsync(Guid id, TEntityUpdateDto entityUpdateDto) {
            return new List<ValidateError>();
        }

        /// <summary>
        /// validate khi thêm dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        protected virtual async Task<List<ValidateError>> CreateValidateAsync(TEntityCreateDto entityCreateDto) {
            return new List<ValidateError>();
        }

        /// <summary>
        /// xóa nhiều bản ghi cùng lúc dựa vào danh sách id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public virtual async Task DeleteListAsync(IEnumerable<Guid> listId)
        {
            // nối danh sách id lại thành string cách nhau bởi dấu ,
            var listIdString = string.Join(",", listId);
            // kiểm tra có ít nhất bản ghi không tồn tại
            var sumOfExisted = await _baseRepository.GetSumExistedOfListAsync(listIdString);
            if (sumOfExisted != listId.Count())
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.NotFound,
                    UserMessage = string.Format(ErrorMessage.NotFoundDeleteError, GetAssetName())
                };

            }
           await _baseRepository.DeleteListAsync(listIdString);
        }

        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        protected abstract string GetAssetName();
    
    }

}
