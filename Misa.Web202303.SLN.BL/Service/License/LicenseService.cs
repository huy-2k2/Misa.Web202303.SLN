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
        /// <summary>
        /// dùng để gọi repo của license
        /// </summary>
        private readonly ILicenseRepository _licenseRepository;

        /// <summary>
        /// dùng để tạo mã code gợi ý
        /// </summary>
        private readonly IRecommendCodeService _recommendCodeService;

        /// <summary>
        /// dùng để validate cho license
        /// </summary>
        private readonly ILicenseDomainService _licenseDomainService;

        /// <summary>
        /// dùng để validate cho license detail
        /// </summary>
        private readonly ILicenseDetailDomainService _licenseDetailDomainService;

        /// <summary>
        /// dùng để valdiate cho budget detail
        /// </summary>
        private readonly IBudgetDetailDomainService _budgetDetailDomainService;

        /// <summary>
        /// dùng để gọi repo license detail
        /// </summary>
        private readonly ILicenseDetailRepository _licenseDetailRepository;

        /// <summary>
        /// dùng để gọi repo budget detail
        /// </summary>
        private readonly IBudgetDetailRepository _budgetDetailRepository;


        /// <summary>
        /// hàm khởi tạo
        /// created by:NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="budgetDetailRepository">budgetDetailRepository</param>
        /// <param name="licenseDetailRepository">licenseDetailRepository</param>
        /// <param name="licenseRepository">licenseRepository</param>
        /// <param name="unitOfWork">unitOfWork</param>
        /// <param name="mapper">mapper</param>
        /// <param name="recommendCodeService">recommendCodeService</param>
        /// <param name="licenseDomainService">licenseDomainService</param>
        /// <param name="licenseDetailDomainService">licenseDetailDomainService</param>
        /// <param name="budgetDetailDomainService">budgetDetailDomainService</param>
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

            // gọi db
            var listLicenseModel = await _licenseRepository.GetListLicenseModelAsync(pageSize, currentPage, textSearch);

            await _unitOfWork.CommitAsync();

            // trả về kết quả
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

        /// <summary>
        /// thêm mới chứng từ
        /// created by: NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="cuLicense">đối tượng cuLicense chứa data từ client</param>
        /// <returns></returns>
        public async Task InsertModelAsync(CULicense cuLicense)
        {
            // tạo id cho license
            var licenseId = Guid.NewGuid();
            // lấy license từ request
            var licenseCreateDto = _mapper.Map<LicenseCreateDto>(cuLicense.license);
            // lấy ra license detail
            var listLicenseDetail = cuLicense.list_fixed_asset.Select(item => new LicenseDetailCreateDto()
            {
                fixed_asset_id = item.fixed_asset_id,
                license_id = licenseId,
            });

            // lấy ra budget detail từ request
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

            // validate dữ liệu license trước khi thêm mới
            await _licenseDomainService.CreateValidateAsync(licenseCreateDto);

            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    // thêm mới license
                    var licenseEntity = _mapper.Map<LicenseEntity>(licenseCreateDto);
                    licenseEntity.license_id = licenseId;
                    await _licenseRepository.InsertAsync(licenseEntity);

                    // validate list license_detail
                    await _licenseDetailDomainService.CreateListValidateAsync(listLicenseDetail);

                    // validate list budget_detail
                    await _budgetDetailDomainService.CreateListValidateAsync(listBudgetDetail);

                    // tạo entity LicenseDetail từ dto bằng mapper
                    var listLicenseDetailEntity = listLicenseDetail.Select(item => _mapper.Map<LicenseDetailEntity>(item));

                    // thêm mới license detail
                    await _licenseDetailRepository.InsertListAsync(listLicenseDetailEntity);

                    // tạo entity BudgetDetail từ dto bằng mapper
                    var listBudgetDetailEntity = listBudgetDetail.Select(item => _mapper.Map<BudgetDetailEntity>(item));

                    // thêm mới budget detail
                    if (listBudgetDetailEntity.Count() > 0)
                    {
                        await _budgetDetailRepository.InsertListAsync(listBudgetDetailEntity);
                    }

                    await _unitOfWork.CommitAsync();

                }
                catch
                {
                    // có lỗi thì rollback
                    await _unitOfWork.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listId">danh sách id cần xóa</param>
        /// <returns></returns>
        public override async Task DeleteListAsync(IEnumerable<Guid> listId)
        {
            // validate delete
            await _licenseDomainService.DeleteListValidateAsync(listId);

            // tạo chuỗi id từ list id
            var stringIds = string.Join(",", listId);

            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    // xóa budget detail liên quan đến danh sách license
                    await _budgetDetailRepository.DeleteListByListLicenseId(stringIds);
                    // xóa danh sách license
                    await _licenseRepository.DeleteListAsync(stringIds);
                    await transaction.CommitAsync();
                }
                catch
                {
                    // nếu có lỗi thì rollback
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

        /// <summary>
        /// cập nhật chứng từ
        /// created by: NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="cuLicense">đối tượng cuLicense chứa data từ client</param>
        /// <returns></returns>
        public async Task UpdateModelAsync(Guid licenseId, CULicense cuLicense)
        {
            // lấy dữ liệu license từ request (không kể các detail)
            var licenseUpdateDto = _mapper.Map<LicenseUpdateDto>(cuLicense.license);

            // lấy danh sách license_detail cần insert
            var listLicenseDetailInsert = cuLicense.list_fixed_asset.Where(item =>
            // nếu là insert thì không có license_detail_id
            item.license_detail_id == null)
                .Select(item => new LicenseDetailCreateDto()
                {
                    license_id = licenseId,
                    fixed_asset_id = item.fixed_asset_id,
                });

            // danh sách budget_detail insert
            var listBudgetDetailInsert = new List<BudgetDetailCreateDto>();
            // danh sách budget_detail update
            var listBudgetDetailUpdate = new List<BudgetDetailUpdateDto>();

            foreach (var fa in cuLicense.list_fixed_asset)
            {
                if (fa.budgets != null)
                {
                    foreach (var bd in fa.budgets)
                    {
                        // nếu không có budget_detail_id, add vào list insert
                        if (bd.budget_detail_id == null)
                        {
                            var dto = _mapper.Map<BudgetDetailCreateDto>(bd);
                            dto.fixed_asset_id = fa.fixed_asset_id;
                            listBudgetDetailInsert.Add(dto);
                        }
                        // nếu như trường is_changed là true và đã có budget_detail_id thì add vào list update
                        else if (bd.is_changed == true)
                        {
                            var dto = _mapper.Map<BudgetDetailUpdateDto>(bd);
                            dto.fixed_asset_id = fa.fixed_asset_id;
                            listBudgetDetailUpdate.Add(dto);
                        }
                    }
                }
            }

            // list license_detail delete
            var listIdLicenseDetailDelete = cuLicense.list_fixed_asset_id_delete;

            // list budget_detail_delete
            var listIdBudgetDetailDelete = cuLicense.list_budget_detail_id_delete;

            // tạo LicenseEntity từ dto
            var licenseEntity = _mapper.Map<LicenseEntity>(licenseUpdateDto);

            // tạo list LicenseDetailEntity từ dto
            var listLicenseDetailEntity = listLicenseDetailInsert.Select(item => _mapper.Map<LicenseDetailEntity>(item));

            // tạo list BudgetDetailEntity insert từ dto
            var listBudgetDetailEntity = listBudgetDetailInsert.Select(item => _mapper.Map<BudgetDetailEntity>(item));

            // tạo list BudgetDetailEntity update từ dto
            var listBudgetDetailEntityUpdate = listBudgetDetailUpdate.Select(item => _mapper.Map<BudgetDetailEntity>(item));

            // validate của xóa budget_detail
            await _budgetDetailDomainService.DeleteListValidateAsync(licenseId, listIdBudgetDetailDelete);

            // validate của xóa license_detail
            await _licenseDetailDomainService.DeleteListValidateAsync(licenseId, listIdLicenseDetailDelete);

            // validate của update license
            await _licenseDomainService.UpdateValidateAsync(licenseId, licenseUpdateDto);


            using (var transaction = await _unitOfWork.GetTransactionAsync())
            {
                try
                {
                    // xóa budget_detail
                    await _budgetDetailRepository.DeleteListAsync(string.Join(",", listIdBudgetDetailDelete));

                    // xóa budget_detail liên quan đến license_detail khi xóa license_detail
                    await _budgetDetailRepository.DeleteByListLicenseDetailId(string.Join(",", listIdLicenseDetailDelete));

                    // xóa license_detail
                    await _licenseDetailRepository.DeleteListAsync(string.Join(",", listIdLicenseDetailDelete));

                    // update license
                    await _licenseRepository.UpdateAsync(licenseId, licenseEntity);

                    // insert license_detail
                    if (listLicenseDetailEntity.Count() > 0)
                    {
                        await _licenseDetailDomainService.CreateListValidateAsync(listLicenseDetailInsert);
                        await _licenseDetailRepository.InsertListAsync(listLicenseDetailEntity);
                    }

                    // update budget_detail
                    if (listBudgetDetailEntityUpdate.Count() > 0)
                    {
                        await _budgetDetailDomainService.UpdateListValidateAsync(listBudgetDetailUpdate);
                        await _budgetDetailRepository.UpdateListAsync(listBudgetDetailEntityUpdate);
                    }

                    // insert budget_detail
                    if (listBudgetDetailEntity.Count() > 0)
                    {
                        await _budgetDetailDomainService.CreateListValidateAsync(listBudgetDetailInsert);
                        await _budgetDetailRepository.InsertListAsync(listBudgetDetailEntity);
                    }


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
    }
}
