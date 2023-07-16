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

        private readonly IFixedAssetRepository _fixedAssetRepository;

        private readonly IBudgetDetailRepository _budgetDetailRepository;

        private readonly IBudgetRepository _budgetRepository;

        public BudgetDetailDomainService(IFixedAssetRepository fixedAssetRepository, IBudgetRepository budgetRepository, IBudgetDetailRepository budgetDetailRepository)
        {
            _budgetDetailRepository = budgetDetailRepository;
            _budgetRepository = budgetRepository;
            _fixedAssetRepository = fixedAssetRepository;
        }

        public async Task CreateListValidateAsync(IEnumerable<BudgetDetailCreateDto> listBudgetDetailCreateDto)
        {
            var listError = new List<ValidateError>();
            foreach (var dto in listBudgetDetailCreateDto) {
                listError = listError.Concat(ValidateAttribute.Validate(dto)).ToList();
            
            }
            var listBudgetId = listBudgetDetailCreateDto.Select(dto => dto.budget_id);
            var stringBudgetId = string.Join(",", listBudgetId);
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
            var listFixedAssetExisted = await _fixedAssetRepository.GetListExistedAsync(stringFixedAssetId);
            if (listFixedAssetExisted.Count() != listFixedAssetId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAsset),
                });
            }

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
                var listBFExisted = await _budgetDetailRepository.GetListExistedBFAsync(stringBFId);
                if (listBFExisted.Count() > 0)
                {
                    listError.Add(new ValidateError()
                    {
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                    });
                }
            }

            if (listError.Count() > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.ValidateCreateError,
                    Data = listError
                };
            }

        }

        public async Task DeleteListValidateAsync(Guid licenseId, IEnumerable<Guid> listDetailId)
        {
            var listError = new List<ValidateError>();
            var listExistedOfLicense = await _budgetDetailRepository.GetListExistedOfLicenseAsync(licenseId, string.Join(",", listDetailId));
            if (listExistedOfLicense.Count() != listDetailId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget)
                });
            }
            if(listError.Count > 0)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.DataError,
                    Data = listError
                };
            }

        }
        public async Task UpdateListValidateAsync(IEnumerable<BudgetDetailUpdateDto> listBudgetDetailUpdateDto)
        {
            var listError = new List<ValidateError>();
            foreach (var dto in listBudgetDetailUpdateDto)
            {
                listError = listError.Concat(ValidateAttribute.Validate(dto)).ToList();

            }
            var listBudgetId = listBudgetDetailUpdateDto.Select(dto => dto.budget_id);
            var stringBudgetId = string.Join(",", listBudgetId);
            var listBudgetExisted = await _budgetRepository.GetListExistedAsync(stringBudgetId);
            var listDetailId = listBudgetDetailUpdateDto.Select(dto => dto.budget_detail_id);
            var listDetailExisted = await _budgetDetailRepository.GetListExistedAsync(string.Join(",", listDetailId));
            if(listDetailExisted.Count() != listDetailId.Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                });
            }

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
            if (listFixedAssetExisted.Count() != listFixedAssetId.GroupBy(id => id).Count())
            {
                listError.Add(new ValidateError()
                {
                    Message = string.Format(ErrorMessage.InvalidError, FieldName.FixedAsset),
                });
            }

            var groupBF = listBudgetDetailUpdateDto.GroupBy(dto => dto.budget_id + "." + dto.fixed_asset_id);

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

                if (listBFExisted.Count() > 0)
                {
                    listError.Add(new ValidateError()
                    {
                        Message = string.Format(ErrorMessage.InvalidError, FieldName.Budget),
                    });
                }
            }
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
