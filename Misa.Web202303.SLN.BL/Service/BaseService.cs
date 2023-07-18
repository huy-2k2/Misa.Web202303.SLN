using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Misa.Web202303.QLTS.BL.Service.FixedAssetCategory;
using Misa.Web202303.QLTS.BL.ValidateDto;
using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.BL.ValidateDto.Decorators;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.License;
using Misa.Web202303.QLTS.DL.unitOfWork;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.Service
{
    /// <summary>
    /// abstract class định nghĩa các phương thức dùng chung để tái sử dụng cho các service
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    /// <typeparam name="TEntity">enityty để giao tiếp với tầng DL</typeparam>
    /// <typeparam name="TEntityDto">dto để lấy dữ liệu</typeparam>
    /// <typeparam name="TEntityUpdateDto">dto để update dữ liệu</typeparam>
    /// <typeparam name="TEntityCreateDto">dto để thêm dữ liệu</typeparam>
    public abstract class BaseService<TEntity, TEntityDto, TEntityUpdateDto, TEntityCreateDto> : IBaseService<TEntityDto, TEntityUpdateDto, TEntityCreateDto>
    {
        #region
        /// <summary>
        /// sử dụng dịch vị của  IBaseRepository
        /// </summary>
        protected readonly IBaseRepository<TEntity> _baseRepository;

        protected readonly IMapper _mapper;

        protected readonly IUnitOfWork _unitOfWork;

        #endregion

        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="baseRepository">baseRepository</param>
        /// <param name="mapper">mapper</param>
        public BaseService(IBaseRepository<TEntity> baseRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region
        /// <summary>
        /// lấy ra 1 bản thi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id">id tài nguyên cần lấy</param>
        /// <returns>bản ghi có id là id đã cho</returns>
        /// <exception cref="NotFoundException">exception trong trường hợp không tìm thấy tài nguyên</exception>
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

            await _unitOfWork.CommitAsync();

            return entityDto;
        }

        /// <summary>
        /// lấy ra tất cả bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>tất cả tài nguyên trong 1 bảng</returns>
        public virtual async Task<IEnumerable<TEntityDto>> GetAsync()
        {
            var listTEntity = await _baseRepository.GetAsync();

            var result = listTEntity.Select(entity => _mapper.Map<TEntityDto>(entity));
            await _unitOfWork.CommitAsync();

            return result;
        }

        /// <summary>
        /// thêm 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto">dữ liệu tài nguyên thêm mới</param>
        /// <exception cref="ValidateException">throw exception khi validate lỗi</exception>
        /// <returns></returns>
        public virtual async Task InsertAsync(TEntityCreateDto entityCreateDto)
        {
            // validate Attr
            var attributeErrors = ValidateAttribute.Validate(entityCreateDto);

            // nếu validate có lỗi thì throw exception
            if (attributeErrors.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = attributeErrors,
                    UserMessage = ErrorMessage.ValidateCreateError

                };
            }
            // validate riêng
            await CreateValidateAsync(entityCreateDto);

            var entity = _mapper.Map<TEntity>(entityCreateDto);

            // mở transaction
            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    await _baseRepository.InsertAsync(entity);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    // nếu có lỗi thì rollback
                    await _unitOfWork.RollbackAsync();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// phương thức update 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityId">id tài sản cần sửa</param>
        /// <param name="entityUpdateDto">dữ liệu tài sản cần sửa</param>
        /// <exception cref="ValidateException">throw exception khi validate lỗi</exception>
        /// <returns></returns>
        public virtual async Task UpdateAsync(Guid entityId, TEntityUpdateDto entityUpdateDto)
        {
            var attributeErrors = ValidateAttribute.Validate(entityUpdateDto);

            if (attributeErrors.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = attributeErrors,
                    UserMessage = ErrorMessage.ValidateUpdateError
                };
            }
            // validate riêng
            await UpdateValidateAsync(entityId, entityUpdateDto);

            var entity = _mapper.Map<TEntity>(entityUpdateDto);
            await _baseRepository.UpdateAsync(entityId, entity);

            await _unitOfWork.CommitAsync();

        }


        /// <summary>
        /// kiểm tra mã code bị trùng khi thêm hoạc sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="code">mã code</param>
        /// <param name="id">id (là rỗng trong trường hợp thêm mới)</param>
        /// <returns>false nếu không tồn tại, true nếu tồn tại</returns>
        public virtual async Task<bool> CheckCodeExisted(string code, Guid? id)
        {

            var result = await _baseRepository.CheckCodeExistedAsync(code, id);
            await _unitOfWork.CommitAsync();

            return result;
        }

        /// <summary>
        /// validate khi update dữ liệu, kiểm tra mã trùng
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id">id tài nguyên</param>
        /// <param name="entityUpdateDto">dữ liệu entity cần valdiate</param>
        /// <returns>danh sách lỗi</returns>
        protected virtual async Task UpdateValidateAsync(Guid id, TEntityUpdateDto entityUpdateDto)
        {
        }

        /// <summary>
        /// validate khi update dữ liệu, kiểm tra mã trùng
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto">dữ liệu entity cần validate</param>
        /// <returns>danh sách lỗi</returns>
        protected virtual async Task CreateValidateAsync(TEntityCreateDto entityCreateDto)
        {
        }

        /// <summary>
        /// xóa nhiều bản ghi cùng lúc dựa vào danh sách id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listId">danh sách id tài nguyên cần xóa</param>
        /// <exception cref="ValidateException">throw exception khi không tồn tại tài nguyên</exception>
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

            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    await _baseRepository.DeleteListAsync(listIdString);
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    // nếu có lỗi thì rollback
                    await _unitOfWork.RollbackAsync();
                    throw ex;
                }
            }
        }

        #endregion

        #region
        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>tên tài nguyên</returns>
        protected virtual string GetAssetName()
        {
            return "";
        }

        #endregion

    }

}
