using AutoMapper.Execution;
using DocumentFormat.OpenXml.Bibliography;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataType = Misa.Web202303.QLTS.Common.Emum.DataType;
namespace Misa.Web202303.QLTS.BL.ValidateDto
{
    /// <summary>
    /// lớp chứa phương thức static, validate dữ liệu đúng có kiểu theo quy định
    /// Created by: NQ Huy(10/05/2023)
    /// </summary>
    public static class ValidateDataType
    {
     
        /// <summary>
        /// valiadte kiểu dữ liệu
        /// </summary>
        /// <param name="dataType">kiểu dữ liệu</param>
        /// <param name="value">giá trị kiểm tra</param>
        /// <returns>true nếu giá trị thuộc kiểu dữ liệu cần kiểm tra, ngược lại trả về false</returns>
        public static bool Validate(int dataType, string value)
        {
            Type type = typeof(string);
            if (dataType == (int)DataType.StringType)
            {
            }
            else if(dataType == (int)DataType.YearType)
            {
                type= typeof(int);
            }
            else if (dataType == (int)DataType.DoubleType)
            {
                type= typeof(double);
            }
            else if (dataType == (int)DataType.IntType)
            {
               type= typeof(int);
            }
            else if (dataType == (int)DataType.DateTimeType)
            {
               type= typeof(DateTime);
            }
            else if (dataType == (int)DataType.GuidType)
            {
               type= typeof(Guid);
            }
            var converter = TypeDescriptor.GetConverter(type);
            return converter.IsValid(value);
        }
    }
}
