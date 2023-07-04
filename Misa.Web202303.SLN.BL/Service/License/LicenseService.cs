using AutoMapper;
using Misa.Web202303.QLTS.BL.RecommendCode;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.filter;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseEntity = Misa.Web202303.QLTS.DL.Entity.License;
using LicenseModel = Misa.Web202303.QLTS.DL.model.License;

namespace Misa.Web202303.QLTS.BL.Service.License
{
    public class LicenseService : BaseService<LicenseEntity, LicenseDto, LicenseUpdateDto, LicenseCreateDto>, ILicenseService
    {
        
        private readonly ILicenseRepository _licenseRepository;

        private readonly IRecommendCodeService _recommendCodeService;

        /// <summary>
        /// hàm khởi tạo
        /// created by:NQ Huy(27/06/2023)
        /// </summary>
        /// <param name="licenseRepository">licenseRepository</param>
        /// <param name="mapper">mapper</param>
        /// <param name="recommendCodeService">recommendCodeService</param>
        public LicenseService(ILicenseRepository licenseRepository, IMapper mapper, IRecommendCodeService recommendCodeService) : base(licenseRepository, mapper)
        {
            _recommendCodeService= recommendCodeService;
            _licenseRepository = licenseRepository;
        }

        /// <summary>
        /// phân trang, filter cho chứng từ
        /// </summary>
        /// <param name="pageSize">kích thước 1 trang</param>
        /// <param name="currentPage">trang hiện tại</param>
        /// <param name="textSearch">từ khóa tìm kiếm</param>
        /// <returns>danh chứng license model thỏa mãn điều kiện phân trang, filter</returns>
        public async Task<FilterLicenseModel> GetListLicenseModelAsync(int pageSize, int currentPage, string? textSearch)
        {
            // validate dữ liệu
            var listLicenseModel = await _licenseRepository.GetListLicenseModelAsync(pageSize, currentPage, textSearch);
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
            return newCode;
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
    }
}
