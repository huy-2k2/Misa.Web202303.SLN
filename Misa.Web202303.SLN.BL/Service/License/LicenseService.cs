using AutoMapper;
using Misa.Web202303.QLTS.BL.BodyRequest.License;
using Misa.Web202303.QLTS.BL.DomainService.BudgetDetail;
using Misa.Web202303.QLTS.BL.DomainService.License;
using Misa.Web202303.QLTS.BL.DomainService.LicenseDetail;
using Misa.Web202303.QLTS.BL.RecommendCode;
using Misa.Web202303.QLTS.BL.Service.BudgetDetail;
using Misa.Web202303.QLTS.BL.Service.LicenseDetail;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.Filter;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.BudgetDetail;
using Misa.Web202303.QLTS.DL.Repository.License;
using Misa.Web202303.QLTS.DL.Repository.LicenseDetail;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseEntity = Misa.Web202303.QLTS.DL.Entity.License;
using LicenseModel = Misa.Web202303.QLTS.DL.Model.License;
using LicenseDetailEntity = Misa.Web202303.QLTS.DL.Entity.LicenseDetail;
using BudgetDetailEntity = Misa.Web202303.QLTS.DL.Entity.BudgetDetail;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace Misa.Web202303.QLTS.BL.Service.License
{
    public class LicenseService : BaseService<LicenseEntity, LicenseDto, LicenseUpdateDto, LicenseCreateDto>, ILicenseService
    {

        private readonly ILicenseRepository _licenseRepository;

        private readonly IRecommendCodeService _recommendCodeService;

        private readonly ILicenseDomainService _licenseDomainService;

        private readonly ILicenseDetailDomainService _licenseDetailDomainService;

        private readonly IBudgetDetailDomainService _budgetDetailDomainService;

        private readonly ILicenseDetailRepository _licenseDetailRepository;

        private readonly IBudgetDetailRepository _budgetDetailRepository;


        /// <summary>
        /// hàm khởi tạo
        /// created by:NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="licenseRepository">licenseRepository</param>
        /// <param name="mapper">mapper</param>
        /// <param name="recommendCodeService">recommendCodeService</param>
        /// <param name="unitOfWork">unitOfWork</param>
        public LicenseService(IBudgetDetailRepository budgetDetailRepository, ILicenseDetailRepository licenseDetailRepository, ILicenseRepository licenseRepository, IUnitOfWork unitOfWork, IMapper mapper, IRecommendCodeService recommendCodeService, ILicenseDomainService licenseDomainService, ILicenseDetailDomainService licenseDetailDomainService, IBudgetDetailDomainService budgetDetailDomainService) : base(licenseRepository, unitOfWork, mapper)
        {
            _recommendCodeService = recommendCodeService;
            _licenseRepository = licenseRepository;
            _licenseDomainService = licenseDomainService;
            _licenseDetailDomainService = licenseDetailDomainService;
            _budgetDetailDomainService = budgetDetailDomainService;
            _licenseDetailRepository = licenseDetailRepository;
            _budgetDetailRepository = budgetDetailRepository;
        }

        /// <summary>
        /// phân trang, Filter cho chứng từ
        /// </summary>
        /// <param name="pageSize">kích thước 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <returns>danh chứng license Model thỏa mãn điều kiện phân trang, Filter</returns>
        public async Task<FilterLicenseModel> GetListLicenseModelAsync(int pageSize, int currentPage, string? textSearch)
        {
            // validate dữ liệu
            _licenseDomainService.FilterInputValdiate(pageSize, currentPage);


            var listLicenseModel = await _licenseRepository.GetListLicenseModelAsync(pageSize, currentPage, textSearch);

            await _unitOfWork.CommitAsync();

            return listLicenseModel;
        }

        /// <summary>
        /// hàm lấy mã gợi ý cho chứng từ
        /// created by: NQ Huy(27/06/2023)
        /// </summary>
        /// <returns>mã code gợi ý</returns>
        public async Task<string> GetRecommendAsync()
        {
            // lấy ra tiền tố và mã tài sản có hậu tố lớn nhất
            var tempList = await _licenseRepository.GetRecommendCodeAsync();
            // lấy ra tiền tố ở ví trị 0
            var prefixCode = tempList.First();
            // lấy ra hậu tố sản ở vị trí 1
            var postFix = tempList.Last();

            var newCode = _recommendCodeService.CreateRecommendCode(prefixCode, postFix);

            await _unitOfWork.CommitAsync();

            return newCode;
        }

        public async Task InsertModelAsync(CULicense cuLicense)
        {
            var licenseId = Guid.NewGuid();
            var licenseCreateDto = _mapper.Map<LicenseCreateDto>(cuLicense.license);
            var listLicenseDetail = cuLicense.list_fixed_asset.Select(item => new LicenseDetailCreateDto()
            {
                fixed_asset_id = item.fixed_asset_id,
                license_id = licenseId,
            });

            var listBudgetDetail = new List<BudgetDetailCreateDto>();

            foreach (var fa in cuLicense.list_fixed_asset)
            {
                if (fa.budgets != null)
                {
                    foreach (var bd in fa.budgets)
                    {
                        var dto = _mapper.Map<BudgetDetailCreateDto>(bd);
                        dto.fixed_asset_id = fa.fixed_asset_id;
                        listBudgetDetail.Add(dto);
                    }
                }
            }

            await _licenseDomainService.CreateValidateAsync(licenseCreateDto);

            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    var licenseEntity = _mapper.Map<LicenseEntity>(licenseCreateDto);
                    licenseEntity.license_id = licenseId;
                    await _licenseRepository.InsertAsync(licenseEntity);

                    await _licenseDetailDomainService.CreateListValidateAsync(listLicenseDetail);

                    await _budgetDetailDomainService.CreateListValidateAsync(listBudgetDetail);

                    var listLicenseDetailEntity = listLicenseDetail.Select(item => _mapper.Map<LicenseDetailEntity>(item));

                    await _licenseDetailRepository.InsertListAsync(listLicenseDetailEntity);

                    var listBudgetDetailEntity = listBudgetDetail.Select(item => _mapper.Map<BudgetDetailEntity>(item));

                    if (listBudgetDetailEntity.Count() > 0)
                    {
                        await _budgetDetailRepository.InsertListAsync(listBudgetDetailEntity);
                    }

                    await _unitOfWork.CommitAsync();

                }
                catch
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }
            }
        }

        public override async Task DeleteListAsync(IEnumerable<Guid> listId)
        {
            await _licenseDomainService.DeleteListValidateAsync(listId);

            var stringIds = string.Join(",", listId);

            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    await _budgetDetailRepository.DeleteListByListLicenseId(stringIds);
                    await _licenseRepository.DeleteListAsync(stringIds);
                    await transaction.CommitAsync();
                }
                catch
                {
                    await _unitOfWork.RollbackAsync();
                    throw;
                }
            }


        }

        /// <summary>
        /// lấy ra tên tài nguyên
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns>tên tài nguyên</returns>
        protected override string GetAssetName()
        {
            return AssetName.License;
        }

        public async Task UpdateModelAsync(Guid licenseId, CULicense cuLicense)
        {
            var licenseUpdateDto = _mapper.Map<LicenseUpdateDto>(cuLicense.license);

            var listLicenseDetailInsert = cuLicense.list_fixed_asset.Where(item => item.license_detail_id == null).Select(item => new LicenseDetailCreateDto()
            {
                license_id = licenseId,
                fixed_asset_id = item.fixed_asset_id,
            });

            var listBudgetDetailInsert = new List<BudgetDetailCreateDto>();
            var listBudgetDetailUpdate = new List<BudgetDetailUpdateDto>();



            foreach (var fa in cuLicense.list_fixed_asset)
            {
                if (fa.budgets != null)
                {
                    foreach (var bd in fa.budgets)
                    {
                        if (bd.budget_detail_id == null)
                        {
                            var dto = _mapper.Map<BudgetDetailCreateDto>(bd);
                            dto.fixed_asset_id = fa.fixed_asset_id;
                            listBudgetDetailInsert.Add(dto);
                        }
                        else if(bd.is_changed == true)
                        {
                            var dto = _mapper.Map<BudgetDetailUpdateDto>(bd);
                            dto.fixed_asset_id = fa.fixed_asset_id;
                            listBudgetDetailUpdate.Add(dto);
                        }
                    }
                }
            }

            var listIdLicenseDetailDelete = cuLicense.list_fixed_asset_id_delete;

            var listIdBudgetDetailDelete = cuLicense.list_budget_detail_id_delete;

            var licenseEntity = _mapper.Map<LicenseEntity>(licenseUpdateDto);

            var listLicenseDetailEntity = listLicenseDetailInsert.Select(item => _mapper.Map<LicenseDetailEntity>(item));

            var listBudgetDetailEntity = listBudgetDetailInsert.Select(item => _mapper.Map<BudgetDetailEntity>(item));

            var listBudgetDetailEntityUpdate = listBudgetDetailUpdate.Select(item => _mapper.Map<BudgetDetailEntity>(item));

            await _budgetDetailDomainService.DeleteListValidateAsync(licenseId, listIdBudgetDetailDelete);

            await _licenseDetailDomainService.DeleteListValidateAsync(licenseId, listIdLicenseDetailDelete);

            await _licenseDomainService.UpdateValidateAsync(licenseId, licenseUpdateDto);


            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    await _budgetDetailRepository.DeleteListAsync(string.Join(",", listIdBudgetDetailDelete));

                    await _budgetDetailRepository.DeleteByListLicenseDetailId(string.Join(",", listIdLicenseDetailDelete));

                    await _licenseDetailRepository.DeleteListAsync(string.Join(",", listIdLicenseDetailDelete));

                    await _licenseRepository.UpdateAsync(licenseId, licenseEntity);


                    if (listLicenseDetailEntity.Count() > 0)
                    {
                        await _licenseDetailDomainService.CreateListValidateAsync(listLicenseDetailInsert);
                        await _licenseDetailRepository.InsertListAsync(listLicenseDetailEntity);
                    }

                    if (listBudgetDetailEntity.Count() > 0){
                        await _budgetDetailDomainService.CreateListValidateAsync(listBudgetDetailInsert);
                        await _budgetDetailRepository.InsertListAsync(listBudgetDetailEntity);
                    }


                    if (listBudgetDetailEntityUpdate.Count() > 0)
                    {
                        await _budgetDetailDomainService.UpdateListValidateAsync(listBudgetDetailUpdate);
                        await _budgetDetailRepository.UpdateListAsync(listBudgetDetailEntityUpdate);
                    }
                    await _unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    throw ex;
                }
            }
        }
    }
}
