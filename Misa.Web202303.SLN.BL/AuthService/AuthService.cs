using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Misa.Web202303.QLTS.BL.JwtService;
using Misa.Web202303.QLTS.BL.ValidateDto;
using Misa.Web202303.QLTS.Common.Emum;
using Misa.Web202303.QLTS.Common.Exceptions;
using Misa.Web202303.QLTS.Common.Resource;
using Misa.Web202303.QLTS.DL.AuthRepository;
using Misa.Web202303.QLTS.DL.Entity;
using Misa.Web202303.QLTS.DL.Repository.Department;
using Misa.Web202303.QLTS.DL.unitOfWork;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.AuthService
{
    public class AuthService : IAuthService
    {
        #region
        /// <summary>
        /// authrepository
        /// </summary>
        private readonly IAuthRepository _authRepository;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper _mapper;


        /// <summary>
        /// JWT
        /// </summary>
        private readonly IJwtService _jwt;
        #endregion

        private readonly IDepartmentRepository _departmentRepository;

        private readonly IUnitOfWork _unitOfWork;

        #region
        /// <summary>
        /// hàm khởi tạo
        /// created by: nq Huy (20/06/2023)
        /// </summary>
        /// <param name="authRepository">authRepository</param>
        /// <param name="mapper">mapper</param>
        /// <param name="configuration">configuration</param>
        public AuthService(IAuthRepository authRepository, IMapper mapper, IJwtService jwt, IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _jwt = jwt;
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///  kiểm tra tên đăng nhập và mật khẩu hợp lệ, nếu hợp lệ trả về token
        ///  created by: nq huy(20/06/2023)
        /// </summary>
        /// <param name="authDto">auth dto chứa thông tin tên đăng nhập và mật khẩu</param>
        /// <returns>json web token</returns>
        /// <exception cref="ValidateException">throw exception khi tên đăng nhập và mật khẩu không hợp lệ</exception>
        public async Task<string> GetAuthAsync(AuthDto authDto)
        {
            // validate attribute
            var errors = ValidateAttribute.Validate(authDto);
            if (errors.Count > 0)
            {
                throw new ValidateException()
                {
                    Data = errors,
                    UserMessage = ErrorMessage.DataError
                };
            }

            var email = authDto.email;
            var encryptedPassword = ToMd5(authDto.password);

            var user = await _authRepository.GetAuthAsync(email);


            // nếu người dùng không tồn tại thì throw exception
            if (user == null || user.password != encryptedPassword)
            {
                throw new ValidateException()
                {
                    ErrorCode = ErrorCode.CredentialNotValid,
                    UserMessage = ErrorMessage.CredentialnvalidError
                };
            }

            // nếu người dùng tồn tại thì tạo và trả về token
            var token = _jwt.CreateToken(user);

            await _unitOfWork.CommitAsync();
            return token;
        }

        /// <summary>
        /// hàm mã hóa 1 chuỗi bằng thuật toán md5
        /// created by: Nguyen Quoc Huy(22/06/2023)
        /// </summary>
        /// <param name="input">chuỗi cần mã hóa</param>
        /// <returns>chuỗi sau khi được mã hóa</returns>
        private static string ToMd5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
        #endregion
    }
}
