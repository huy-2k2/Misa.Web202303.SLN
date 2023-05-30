using AutoMapper;
using ClosedXML.Excel;
using Misa.Web202303.SLN.Common.Emum;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.Common.Resource;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository.Department;
using Misa.Web202303.SLN.DL.Repository.FixedAsset;
using Misa.Web202303.SLN.DL.Repository.FixedAssetCategory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// vì FixedAsset trong Entity ở DL trùng với tên của namespace nên đặt bí danh
using FixedAssetEntity = Misa.Web202303.SLN.DL.Entity.FixedAsset;

namespace Misa.Web202303.SLN.BL.Service.FixedAsset
{
    /// <summary>
    /// Lớp định nghĩa các dịch vụ của FixedAsset, gồm các phương thức của IFixedAssetService, IBaseService, sử dụng lại các phương thức của BaseService
    /// created by: nqhuy(21/05/2023)
    /// </summary
    public class FixedAssetService : BaseService<FixedAssetEntity, FixedAssetDto, FixedAssetUpdateDto, FixedAssetCreateDto>, IFixedAssetService
    {
        /// <summary>
        /// sử dụng để gọi phương thức của IFixedAssetRepository
        /// </summary>
        private readonly IFixedAssetRepository _fixedAssetRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IFixedAssetCategoryRepository _fixedAssetCategoryRepository;


        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetRepository"></param>
        /// <param name="mapper"></param>
        public FixedAssetService(IFixedAssetRepository fixedAssetRepository, IMapper mapper, IDepartmentRepository departmentRepository, IFixedAssetCategoryRepository fixedAssetCategoryRepository) : base(fixedAssetRepository, mapper)
        {
            _fixedAssetRepository = fixedAssetRepository;
            _departmentRepository = departmentRepository;
            _fixedAssetCategoryRepository= fixedAssetCategoryRepository;
        }


        /// <summary>
        /// xóa nhiều tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(IEnumerable<Guid> listFixedAssetId)
        {
           
            /// nối các mã tài sản lại thành chuỗi
            string stringFixedAssetId = string.Join("", listFixedAssetId.ToArray());

            int numberOfFixedAsset = await _fixedAssetRepository.CountFixedAssetInListIdAsync(stringFixedAssetId);
            if(numberOfFixedAsset != listFixedAssetId.Count()) { 
                throw new NotFoundException() 
                {
                    UserMessage = ErrorMessage.NotFoundDeleteError,
                    DevMessage = ErrorMessage.NotFoundDeleteError,
                    ErrorCode = ErrorCode.NotFound,
                };
            }


            var result = await _fixedAssetRepository.DeleteAsync(stringFixedAssetId);
            return result;
        }

        /// <summary>
        /// filter, search, phân trang tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <param name="departmentId"></param>
        /// <param name="fixedAssetCategoryId"></param>
        /// <param name="textSearch"></param>
        /// <returns></returns>
        public async Task<object> GetAsync(int pageSize, int currentPage, Guid? departmentId, Guid? fixedAssetCategoryId, string? textSearch)
        {
            // validate dữ liệu
            if(pageSize <= 0) {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.PositiveNumberError, "kích thước page"),
                    DevMessage = string.Format(ErrorMessage.PositiveNumberError, "kích thước page"),
                    ErrorCode= ErrorCode.DataValidate,
                };
            }
            if(currentPage <= 0)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.PositiveNumberError, "page hiện tại"),
                    DevMessage = string.Format(ErrorMessage.PositiveNumberError, "page hiện tại"),
                    ErrorCode = ErrorCode.DataValidate,
                };
            }
            var filterListFixedAsset = await _fixedAssetRepository.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);
            var listFixedAsset = filterListFixedAsset.List_fixed_asset.Select((fixedAsset) => _mapper.Map<FixedAssetDto>(fixedAsset));

            return new { listFixedAsset, totalAsset = filterListFixedAsset.Total_asset, totalQuantity=filterListFixedAsset.Total_quantity, totalCost=filterListFixedAsset.Total_cost };
        }

        /// <summary>
        /// lấy dữ liệu tài sản đề xuất file excel
        /// Created by: NQ Huy(20/05/2023)
        /// </summary>
        /// <returns></returns>
        public async Task<MemoryStream> GetFixedAssetsExcelAsync()
        {
            var fixedAssetsExcel = await _fixedAssetRepository.GetFixedAssetsExcelAsync();
            using(var workbook = new XLWorkbook())
            {
                // tạo thead cho bảng
                var worksheet = workbook.Worksheets.Add("fixed_assets");
                var currentRow = 1;
                double totalQuantity = 0;
                double totalCost = 0;
                worksheet.Cell(currentRow, 1).Value = "mã tài sản";
                worksheet.Cell(currentRow, 2).Value = "tên tài sản";
                worksheet.Cell(currentRow, 3).Value = "bộ phận sử dụng";
                worksheet.Cell(currentRow, 4).Value = "loại tài sản";
                worksheet.Cell(currentRow, 5).Value = "nguyên giá";
                worksheet.Cell(currentRow, 6).Value = "số lượng";
                worksheet.Cell(currentRow, 7).Value = "HM/KH lũy kế";
                worksheet.Cell(currentRow, 8).Value = "giá trị còn lại";
                worksheet.Cell(currentRow, 9).Value = "tỷ lệ hao mòn";
                worksheet.Cell(currentRow, 10).Value = "hao mòn năm";
                worksheet.Cell(currentRow, 11).Value = "số năm sử dụng";
                worksheet.Cell(currentRow, 12).Value = "ngày mua";
                worksheet.Cell(currentRow, 13).Value = "ngày sử dụng";
                worksheet.Cell(currentRow, 14).Value = "năm theo dõi";

                // add từng ô cho bảng dữ liệu
                foreach (var fixedAsset in fixedAssetsExcel)
                {
                    currentRow++;
                    totalCost += fixedAsset.Cost;
                    totalQuantity += fixedAsset.Quantity;
                    worksheet.Cell(currentRow, 1).Value = fixedAsset.Fixed_asset_code;
                    worksheet.Cell(currentRow, 2).Value = fixedAsset.Fixed_asset_name;
                    worksheet.Cell(currentRow, 3).Value = fixedAsset.Department_name;
                    worksheet.Cell(currentRow, 4).Value = fixedAsset.Fixed_asset_category_name;
                    worksheet.Cell(currentRow, 5).Value = fixedAsset.Cost;
                    worksheet.Cell(currentRow, 6).Value = fixedAsset.Quantity;
                    worksheet.Cell(currentRow, 7).Value = 0;
                    worksheet.Cell(currentRow, 8).Value = fixedAsset.Cost;
                    worksheet.Cell(currentRow, 9).Value = fixedAsset.Depreciation_rate;
                    worksheet.Cell(currentRow, 10).Value = fixedAsset.Depreciation_annual;
                    worksheet.Cell(currentRow, 11).Value = fixedAsset.Life_Time;
                    worksheet.Cell(currentRow, 12).Value = fixedAsset.Purchase_date;
                    worksheet.Cell(currentRow, 13).Value = fixedAsset.Use_date;
                    worksheet.Cell(currentRow, 14).Value = fixedAsset.Tracked_year;
                }

                currentRow++;

                worksheet.Cell(currentRow, 5).Value = totalCost;
                worksheet.Cell(currentRow, 6).Value = totalQuantity;
                worksheet.Cell(currentRow, 7).Value = 0;
                worksheet.Cell(currentRow, 8).Value = totalCost;

                worksheet.Row(1).Style.Font.SetBold(true);
                worksheet.Row(currentRow).Style.Font.SetBold(true);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream;
                }
            }
        }

        /// <summary>
        /// tự động sinh mà tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRecommendFixedAssetCodeAsync()
        {
            // lấy ra danh sách các mã tài sản và tiền tố có cùng tiền tố với tài sản thêm hoạc sửa gần nhất
            var listFixedAssetCode = await _fixedAssetRepository.GetListAssetCodeAsync();
            // lấy ra tiền tố từ cuối list
            var prefixCode = listFixedAssetCode.Last();
            // xóa tiền tố để có danh sách chỉ có mã tài sản
            listFixedAssetCode.RemoveAt(listFixedAssetCode.Count() - 1);

            // lấy danh sách hậu tố từ danh sách mã tài sản
            var listPostfixCode = listFixedAssetCode.Select((fixedAssetCode) => fixedAssetCode.Substring(prefixCode.Length));

            var index = -1;
            var max = -1;
            // tìm giá trị hậu tố lớn nhất và ví trị đó
            for (int i = 0; i < listPostfixCode.Count(); i++)
            {
                var intValue = listPostfixCode.ElementAt(i) == "" ? -1 : int.Parse(listPostfixCode.ElementAt(i));
                if (intValue >= max)
                {
                    max = intValue;
                    index = i;
                }
            }

            // hậu tố mới = hậu tố max + 1
            var postfixCodeResult = $"{max + 1}";
            // lấy độ dài của hậu tố max
            var postfixCodeLength = listPostfixCode.ElementAt(index).Length;
            // thêm các chữ số 0 vào hậu tố mới
            for (int i = 0; i < postfixCodeLength; i++)
            {
                // kiểm tra đến chữ số đầu tiên của hậu tố max thì break
                if (listPostfixCode.ElementAt(index).ElementAt(i) != '0')
                    break;
                // nếu độ dài hậu tố mới < độ dài hậu tố cũ => thêm 0 vào trước hậu tố mới
                if (postfixCodeResult.Length < postfixCodeLength)
                    postfixCodeResult = "0" + postfixCodeResult;
            }

            // mã tài sản mới = tiền tố + hậu tố mới
            var result = prefixCode + postfixCodeResult;

            return result;
        }

        /// <summary>
        /// validate khi thêm mới dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCreateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task CreateValidateAsync(FixedAssetCreateDto fixedAssetCreateDto)
        {
            // gọi hàm validate chung
            var fixedAsset = _mapper.Map<FixedAssetEntity>(fixedAssetCreateDto);
            await CommonValidateAsync(_mapper.Map<FixedAssetEntity>(fixedAsset));
            // kiểm tra mã tài sản bị trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(fixedAssetCreateDto.Fixed_asset_code, null);
            if(isCodeExisted) {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã tài sản"),
                    DevMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã tài sản"),
                    ErrorCode = ErrorCode.DuplicateCode
                };
            }
        }

        /// <summary>
        /// validate khi update dữ liệu
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetId"></param>
        /// <param name="fixedAssetUpdateDto"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        protected async override Task UpdateValidateAsync(Guid fixedAssetId, FixedAssetUpdateDto fixedAssetUpdateDto)
        {
            // gọi hàm validate chung
            var fixedAsset = _mapper.Map<FixedAssetEntity>(fixedAssetUpdateDto);
            await CommonValidateAsync(_mapper.Map<FixedAssetEntity>(fixedAsset));
            // kiểm tra mã tài sản bị trùng
            var isCodeExisted = await _baseRepository.CheckCodeExistedAsync(fixedAssetUpdateDto.Fixed_asset_code, fixedAssetId);
            if (isCodeExisted)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã tài sản"),
                    DevMessage = string.Format(ErrorMessage.DuplicateCodeError, "mã tài sản"),
                    ErrorCode = ErrorCode.DuplicateCode
                };
            }
        }

        /// <summary>
        /// hàm validate chung cho cả insert và update, thực hiện các logic nghiệp vụ
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAsset"></param>
        /// <returns></returns>
        /// <exception cref="ValidateException"></exception>
        private async Task CommonValidateAsync(FixedAssetEntity fixedAsset) {
            // làm tròn số
            var depreciationAnnual = Math.Round((double)fixedAsset.Depreciation_rate * fixedAsset.Cost / 100, 2);
            var depreciationRate = Math.Round((double)1 / fixedAsset.Life_Time * 100, 2);
            // hao mòn năm  = tỷ lệ hao mòn * nguyên giá
            if (fixedAsset.Depreciation_annual != depreciationAnnual) { 
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.DepAnnualCostDepRateError,
                    DevMessage = ErrorMessage.DepAnnualCostDepRateError,
                    ErrorCode = ErrorCode.BusinessValidate
                };
            }
            // tỷ lệ hao mòn = 1 / số năm sử dụng
            if(fixedAsset.Depreciation_rate != depreciationRate)
            {
                throw new ValidateException()
                {
                    UserMessage = ErrorMessage.DepRateLifeTimeError,
                    DevMessage = ErrorMessage.DepRateLifeTimeError,
                    ErrorCode = ErrorCode.BusinessValidate
                };
            }
            // kiểm tra department_id tồn tại
            var department = await _departmentRepository.GetAsync(fixedAsset.Department_id);
            if(department == null) {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.InvalidError, "mã loại tài sản"),
                    DevMessage = string.Format(ErrorMessage.InvalidError, "mã bộ phận sử dụng"),
                    ErrorCode = ErrorCode.BusinessValidate
                };
            }
            // kiểm tra fixed_asset_category_id tồn tại
            var fixedAssetCategory = await _fixedAssetCategoryRepository.GetAsync(fixedAsset.Fixed_asset_category_id);
            if(fixedAssetCategory == null)
            {
                throw new ValidateException()
                {
                    UserMessage = string.Format(ErrorMessage.InvalidError, "mã loại tài sản"),
                    DevMessage = string.Format(ErrorMessage.InvalidError, "mã loại tài sản"),
                    ErrorCode = ErrorCode.BusinessValidate
                };
            }
            
        }
    }
}
