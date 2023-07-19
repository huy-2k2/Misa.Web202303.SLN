using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
using Misa.Web202303.QLTS.BL.DomainService.Department;
using Misa.Web202303.QLTS.BL.ImportService;
using Misa.Web202303.QLTS.BL.ImportService.Department;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.BL.ValidateDto;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DepartmentEntity = Misa.Web202303.QLTS.DL.Entity.Department;

namespace Misa.Web202303.QLTS.BL.Service.Department
{
    /// <summary>
    /// Lớp định nghĩa các dịch vụ của Department, gồm các phương thức của IDepartmentService, IBaseService, sử dụng lại các phương thức của BaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public class DepartmentService : BaseService<DepartmentEntity, DepartmentDto, DepartmentUpdateDto, DepartmentCreateDto>, IDepartmentService
    {
        #region
        /// <summary>
        /// dùng để nhập khẩu dữ liệu
        /// </summary>
        private readonly IDepartmentImportService _departmentImportService;

        /// <summary>
        /// dùng để validate
        /// </summary>
        private readonly IDepartmentDomainService _departmentDomainService;
        #endregion

        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="departmentRepository">departmentRepository</param>
        /// <param name="mapper">mapper</param>
        /// <param name="departmentImportService">departmentImportService</param>
        /// <param name="unitOfWork">unitOfWork</param>
        public DepartmentService(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork, IMapper mapper, IDepartmentImportService departmentImportService, IDepartmentDomainService departmentDomainService) : base(departmentRepository, unitOfWork, mapper)
        {
            _departmentImportService = departmentImportService;
            _departmentDomainService = departmentDomainService;
        }

        #endregion

        #region

        /// <summary>
        /// validate khi thêm
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="departmentCreateDto"></param>
        /// <returns></returns>
        protected async override Task CreateValidateAsync(DepartmentCreateDto departmentCreateDto)
        {
             await _departmentDomainService.CreateValidateAsync(departmentCreateDto);
        }

        /// <summary>
        /// valdiate khi sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="departmentUpdateDto"></param>
        /// <returns></returns>
        protected async override Task UpdateValidateAsync(Guid id, DepartmentUpdateDto departmentUpdateDto)
        {
             await _departmentDomainService.UpdateValidateAsync(id, departmentUpdateDto);
        }


        /// <summary>
        /// import dữ liệu tài sản từ file excel và db
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="stream">file import dưới dạng stream</param>
        /// <param name="isSubmit">biến kiểm tra người dùng có đang submit</param>
        /// <exception cref="ValidateException">throw exception khi validate lỗi</exception>
        /// <returns>dữ liệu về file excel và dữ liệu valdiate</returns>
        public async Task<ImportErrorEntity<DepartmentEntity>> ImportFileAsync(MemoryStream stream, bool isSubmit)
        {
            // validate dữ liệu
            var validateEntity = await _departmentImportService.ValidateAsync(stream);


            if (isSubmit && validateEntity.IsPassed || !isSubmit)
            {
                var listEntity = validateEntity.ListEntity;
                // nếu không có lỗi thì import
                if (isSubmit)
                    await _baseRepository.InsertListAsync(listEntity);
                await _unitOfWork.CommitAsync();
                return validateEntity;
            }
            else
            {
                await _unitOfWork.CommitAsync();
                // có lỗi thì throw exception
                throw new ValidateException()
                {
                    Data = validateEntity,
                    ErrorCode = ErrorCode.InvalidData,
                    UserMessage = ErrorMessage.FileDataError
                };
            }
        }


        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>tên tài nguyên</returns>
        protected override string GetAssetName()
        {
            return AssetName.Department;
        }

        #endregion
    }
}
