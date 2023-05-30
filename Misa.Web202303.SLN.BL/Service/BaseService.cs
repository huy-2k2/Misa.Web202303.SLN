using AutoMapper;
using Misa.Web202303.SLN.BL.Service.FixedAssetCategory;
using Misa.Web202303.SLN.BL.ValidateDto.Attributes;
using Misa.Web202303.SLN.BL.ValidateDto.Decorators;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Repository;
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
        /// xóa 1 bản ghi theo id
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            // kiểm tra bản ghi có tồn tại không
            var entity = await _baseRepository.GetAsync(id);
            if(entity == null)
            {
                throw new NotFoundException()
                {
                    DevMessage = ErrorMessage.NotFoundDeleteError,
                    UserMessage = ErrorMessage.NotFoundDeleteError,
                    ErrorCode = ErrorCode.NotFound
                };
            }

            var result = await _baseRepository.DeleteAsync(id);
            return result;
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
                    DevMessage = ErrorMessage.NotFoundError,
                    UserMessage = ErrorMessage.NotFoundError,
                    ErrorCode = ErrorCode.NotFound
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
        public virtual async Task<TEntityDto> InsertAsync(TEntityCreateDto entityCreateDto)
        {
            BaseValidate(entityCreateDto);
            await CreateValidateAsync(entityCreateDto);
            var entity = _mapper.Map<TEntity>(entityCreateDto);
            var newEntity = await _baseRepository.InsertAsync(entity);
            return _mapper.Map<TEntityDto>(newEntity);
        }

        /// <summary>
        /// update 1 bản ghi
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<TEntityDto> UpdateAsync(Guid entityId, TEntityUpdateDto entityUpdateDto)
        {
            BaseValidate(entityUpdateDto);
            var oldEntity =  await _baseRepository.GetAsync(entityId);
            if(oldEntity == null)
            {
                throw new ValidateException()
                {
                    DevMessage = ErrorMessage.NotFoundUpdateError,
                    UserMessage = ErrorMessage.NotFoundUpdateError,
                    ErrorCode = ErrorCode.NotFound
                };
            }
            await UpdateValidateAsync(entityId, entityUpdateDto);
            
            var entity = _mapper.Map<TEntity>(entityUpdateDto);
            var newEntity = await _baseRepository.UpdateAsync(entityId, entity);
            return _mapper.Map<TEntityDto>(newEntity);
        }

        /// <summary>
        /// phương thức validate các attribute được đánh dấu trong entity, sử dụng decorator parttern, phương thức khởi tạo decorator parttern
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <typeparam name="TValiadteEntity"></typeparam>
        /// <param name="entity"></param>
        protected void BaseValidate<TValiadteEntity>(TValiadteEntity entity)
        {
            var props = entity.GetType().GetProperties();

            // khởi tạo decorator init, phương thức handle rỗng
            BaseDecorator decorator = new InitDecorator();

            // lấy ra các prop có chứa attribute validate
            var markValidateProps = props.Where(p => Attribute.IsDefined(p, typeof(BaseAttribute), true));
            foreach (var prop in markValidateProps)
            {
                // lấy ra các attribute validate ở mỗi prop
                var attributes = prop.GetCustomAttributes(typeof(BaseAttribute), true);
                // lấy ra tên của trường dữ liệu
                var nameAttribute = (NameAttribute)prop.GetCustomAttributes(typeof(NameAttribute), true)[0];
                
                // duyệt từng  attribute validate ở mỗi prop
                foreach (var attribute in attributes)
                {
                    // lấy ra tên decorator tương ứng với attribte 
                    var decoratorName = $"{attribute.GetType().Name}Decorator";

                    // tạo mới decorator
                    object obj = Activator.CreateInstance(Type.GetType($"{decorator.GetType().Namespace}.{decoratorName}"));

                    var newDecorator = (BaseDecorator)obj;

                    // dùng decorator mới tạo để wrap các decorator trưoc đó 
                    newDecorator.nextDecorator = decorator;
                    newDecorator.attribute = (BaseAttribute)attribute;
                    newDecorator.propValue = prop.GetValue(entity);
                    newDecorator.Name = nameAttribute.Name;
                    decorator = newDecorator;
                }
            }
            // thực hiện validate
            decorator.Validate();
        }


        /// <summary>
        /// kiểm tra mã code bị trùng khi thêm hoạc sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="code"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckCodeExisted(string code, Guid? id)
        {
            if (string.IsNullOrEmpty(code)) {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.RequiredError, "mã code"),
                    DevMessage = string.Format(ErrorMessage.RequiredError, "mã code"),
                    ErrorCode = ErrorCode.DataValidate
                };
            };
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
        protected abstract Task UpdateValidateAsync(Guid id, TEntityUpdateDto entityUpdateDto);

        /// <summary>
        /// validate khi thêm dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns></returns>
        protected abstract Task CreateValidateAsync(TEntityCreateDto entityCreateDto);
    }
}
