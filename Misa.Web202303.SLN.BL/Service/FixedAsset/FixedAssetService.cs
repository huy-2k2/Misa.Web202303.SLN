using AutoMapper;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository.FixedAsset;
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


        /// <summary>
        /// phương thức khởi tạo
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetRepository"></param>
        /// <param name="mapper"></param>
        public FixedAssetService(IFixedAssetRepository fixedAssetRepository, IMapper mapper) : base(fixedAssetRepository, mapper)
        {
            _fixedAssetRepository = fixedAssetRepository;
        }

        /// <summary>
        /// kiểm tra mã tài sản đã tồn tại? khi thêm và sửa
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="fixedAssetCode"></param>
        /// <param name="fixedAssetId"></param>
        /// <returns></returns>
        public async Task<bool> CheckAssetCodeExisted(string fixedAssetCode, Guid? fixedAssetId)
        {
            //validate dữ liệu
            //...

            var result = await _fixedAssetRepository.CheckAssetCodeExistedAsync(fixedAssetCode, fixedAssetId);
            return result;
        }

        /// <summary>
        /// xóa nhiều tài sản
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <param name="listFixedAssetId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(IEnumerable<Guid> listFixedAssetId)
        {
            //validate dữ liệu
            //...
            /// nối các mã tài sản lại thành chuỗi
            string stringFixedAssetId = string.Join("", listFixedAssetId.ToArray());

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
            // ...
            var filterListFixedAsset = await _fixedAssetRepository.GetAsync(pageSize, currentPage, departmentId, fixedAssetCategoryId, textSearch);
            var listFixedAsset = filterListFixedAsset.List_fixed_asset.Select((fixedAsset) => _mapper.Map<FixedAssetDto>(fixedAsset));

            return new { listFixedAsset, totalAsset = filterListFixedAsset.Total_asset };
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

    }
}
