using Misa.Web202303.QLTS.BL.Service.BudgetDetail;
using Misa.Web202303.QLTS.BL.ValidateDto;
using Misa.Web202303.QLTS.Common.Const;
using Misa.Web202303.QLTS.Common.Error;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Repository.Budget;
using Misa.Web202303.QLTS.DL.Repository.BudgetDetail;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.DomainService.BudgetDetail
{
    public class BudgetDetailDomainService : IBudgetDetailDomainService
    {
        /// <summary>
        /// repo fixedAsset
        /// </summary>
        private readonly IFixedAssetRepository _fixedAssetRepository;

        /// <summary>
        /// repo budget detail
        /// </summary>
        private readonly IBudgetDetailRepository _budgetDetailRepository;

        /// <summary>
        /// repo budgegt
        /// </summary>
        private readonly IBudgetRepository _budgetRepository;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="fixedAssetRepository"></param>
        /// <param name="budgetRepository"></param>
        /// <param name="budgetDetailRepository"></param>
        public BudgetDetailDomainService(IFixedAssetRepository fixedAssetRepository, IBudgetRepository budgetRepository, IBudgetDetailRepository budgetDetailRepository)
        {
            _budgetDetailRepository = budgetDetailRepository;
            _budgetRepository = budgetRepository;
            _fixedAssetRepository = fixedAssetRepository;
        }

        /// <summary>
        /// hàm validate khi thêm nhiều
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="listBudgetDetailCreateDto">list các đối tượng BudgetDetailCreateDto</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">throw exception khi gặp lỗi</exception>
        public async Task CreateListValidateAsync(IEnumerable<BudgetDetailCreateDto> listBudgetDetailCreateDto)
        {
            var listError = new List<ValidateError>();
            // validate bằng attribute 
            foreach (var dto in listBudgetDetailCreateDto) {
                listError = listError.Concat(ValidateAttribute.Validate(dto)).ToList();
            
            }
            var listBudgetId = listBudgetDetailCreateDto.Select(dto => dto.budget_id);
            var stringBudgetId = string.Join(",", listBudgetId);

            // kiểm tra xem trong list budget_id có id nào không tồn tại không
            var listBudgetExisted = await _budgetRepository.GetListExistedAsync(stringBudgetId);

            if (listBudgetExisted.Count() != listBudgetId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                });
            }

            var listFixedAssetId = listBudgetDetailCreateDto.Select(dto => dto.fixed_asset_id);
            var stringFixedAssetId = string.Join(",", listFixedAssetId);
            // kiểm tra xem trong list fixed_asset_id có id nào không tồn tại hay không
            var listFixedAssetExisted = await _fixedAssetRepository.GetListExistedAsync(stringFixedAssetId);
            if (listFixedAssetExisted.Count() != listFixedAssetId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAsset),
                });
            }

            // kiểm tra trong list create thì fixed_asset_id và budget_id phải đôi 1 khác nhau
            var groupBF = listBudgetDetailCreateDto.GroupBy(dto => dto.budget_id + "." + dto.fixed_asset_id);

            if (groupBF.Count() != listBudgetDetailCreateDto.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = ErrorMessage.BFExisted
                });
            }
            else
            {
                var listBFId = listBudgetDetailCreateDto.Select(dto => dto.budget_id + "." + dto.fixed_asset_id);
                var stringBFId = string.Join(",", listBFId);
                // kiểm tra list fixed_asset_id và budget_id đôi 1 khác nhau với database
                var listBFExisted = await _budgetDetailRepository.GetListExistedBFAsync(stringBFId);
                if (listBFExisted.Count() > 0)
                {
                    listError.Add(new ValidateError()
                    {
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                    });
                }
            }

            // nếu có lỗi thì throw exception
            if (listError.Count() > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.ValidateCreateError,
                    Data = listError
                };
            }

        }

        /// <summary>
        /// hàm validate khi delete nhiều
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="licenseId">license id tương ứng của list budget detail</param>
        /// <param name="listDetailId">list budget detail id</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">nếu có lỗi thì throw exception</exception>
        public async Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId)
        {
            var listError = new List<ValidateError>();
            // kiểm tra xem trong list xóa có id nào không tồn không, và tất cả budget_detail phải thuộc 1 chứng từ cho trước
            var listExistedOfLicense = await _budgetDetailRepository.GetListExistedOfLicenseAsync(licenseId, string.Join(",", listDetailId));
            if (listExistedOfLicense.Count() != listDetailId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget)
                });
            }

            // nếu có lỗi thì throw exception
            if(listError.Count > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.DataError,
                    Data = listError
                };
            }

        }

        /// <summary>
        /// validate update nhiều
        /// created by: NQ Huy (10/07/2023)
        /// </summary>
        /// <param name="listBudgetDetailUpdateDto">danh sách đối tượng BudgetDetailUpdateDto</param>
        /// <returns></returns>
        /// <exception cref="ValidateException">nếu có lỗi thì throw exception</exception>
        public async Task UpdateListValidateAsync(IEnumerable<BudgetDetailUpdateDto> listBudgetDetailUpdateDto)
        {
            var listError = new List<ValidateError>();
            // validate bằng attribute
            foreach (var dto in listBudgetDetailUpdateDto)
            {
                listError = listError.Concat(ValidateAttribute.Validate(dto)).ToList();

            }
            var listBudgetId = listBudgetDetailUpdateDto.Select(dto => dto.budget_id);
            var stringBudgetId = string.Join(",", listBudgetId);
            var listBudgetExisted = await _budgetRepository.GetListExistedAsync(stringBudgetId);
            var listDetailId = listBudgetDetailUpdateDto.Select(dto => dto.budget_detail_id);
            var listDetailExisted = await _budgetDetailRepository.GetListExistedAsync(string.Join(",", listDetailId));

            // kiểm tra trong list update có id nào không tồn tại hay không
            if(listDetailExisted.Count() != listDetailId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                });
            }

            // kiểm tra xem foreign key đến budget có id nào không tồn tại không
            if (listBudgetExisted.Count() != listBudgetId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                });
            }

            var listFixedAssetId = listBudgetDetailUpdateDto.Select(dto => dto.fixed_asset_id);
            var stringFixedAssetId = string.Join(",", listFixedAssetId);
            var listFixedAssetExisted = await _fixedAssetRepository.GetListExistedAsync(stringFixedAssetId);
            // kiểm tra xem foreign key đến fixed_asset có id nào không tồn tại không
            if (listFixedAssetExisted.Count() != listFixedAssetId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAsset),
                });
            }

            var groupBF = listBudgetDetailUpdateDto.GroupBy(dto => dto.budget_id + "." + dto.fixed_asset_id);

            // kiểm tra trong list update fixed_asset_id và budget_id phải đôi 1 khác nhau
            if (groupBF.Count() != listBudgetDetailUpdateDto.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = ErrorMessage.BFExisted
                });
            }
            else
            {
                var listBFId = listBudgetDetailUpdateDto.Select(dto => dto.budget_id + "." + dto.fixed_asset_id);
                var stringBFId = string.Join(",", listBFId);
                var listBFExisted = await _budgetDetailRepository.GetListExistedBFAsync(stringBFId);

                listBFExisted = listBFExisted.Where(entity => !listDetailId.Contains(entity.budget_detail_id));

                // kiểm tra trong list update fixed_asset_id và budget_id phải đôi 1 khác nhau với database
                if (listBFExisted.Count() > 0)
                {
                    listError.Add(new ValidateError()
                    {
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                    });
                }
            }

            // nếu có lỗi thì throw exception
            if (listError.Count() > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.ValidateUpdateError,
                    Data = listError
                };
            }
        }
    }
}
