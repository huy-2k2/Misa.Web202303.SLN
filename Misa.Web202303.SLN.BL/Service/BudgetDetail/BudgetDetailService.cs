using AutoMapper;
using Misa.Web202303.QLTS.DL.Repository;
using Misa.Web202303.QLTS.DL.Repository.BudgetDetail;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetDetailEntity = Misa.Web202303.QLTS.DL.Entity.BudgetDetail;

namespace Misa.Web202303.QLTS.BL.Service.BudgetDetail
{
    public class BudgetDetailService : BaseService<BudgetDetailEntity, BudgetDetailDto, BudgetDetailUpdateDto, BudgetDetailCreateDto>, IBudgetDetailService
    {
        private readonly IBudgetDetailRepository _budgetDetailRepository;

        /// <summary>
        /// hàm khởi tạo
        /// created by: NQ Huy(10/07/2023)
        /// </summary>
        /// <param name="budgetDetailRepository">budgetDetailRepository</param>
        /// <param name="unitOfWork">unitOfWork</param>
        /// <param name="mapper">mapper</param>
        public BudgetDetailService(IBudgetDetailRepository budgetDetailRepository, IUnitOfWork unitOfWork, IMapper mapper) : base(budgetDetailRepository, unitOfWork, mapper)
        {
            _budgetDetailRepository = budgetDetailRepository;
        }

        


    }
}
