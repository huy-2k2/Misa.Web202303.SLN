using Misa.Web202303.QLTS.BL.ValidateDto.Attributes;
using Misa.Web202303.QLTS.BL.ValidateDto.Decorators;
using Misa.Web202303.QLTS.Common.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.BL.ValidateDto
{
    /// <summary>
    /// lớp static để gọi phướng thức validate cho Attr
    /// created by: nqhuy(21/05/2023)
    /// </summary>
    public static class ValidateAttribute
    {
        /// <summary>
        /// phương thức validate các Attr được đánh dấu trong entity, sử dụng decorator parttern, phương thức khởi tạo decorator parttern
        /// created by: nqhuy(21/05/2023)
        /// </summary>
        /// <typeparam name="TValidateEntity">Kiểu cần kiểu tra</typeparam>
        /// <param name="entity">đối tượng kiêm tra</param>
        ///<returns>danh sách lỗi khi validate attribute đặt trong properties</returns>
        
        public static List<ValidateError> Validate<TValidateEntity>(TValidateEntity entity)
        {
            var props = entity.GetType().GetProperties();

            // danh sách lỗi
            List<ValidateError> errors = new();
            // lấy ra các prop có chứa Attr validate
            var markValidateProps = props.Where(p => Attribute.IsDefined(p, typeof(BaseAttribute), true));
            foreach (var prop in markValidateProps)
            {
                // khởi tạo decorator init, phương thức handle rỗng
                BaseDecorator decorator = new InitDecorator();
                // lấy ra các Attr validate ở mỗi prop
                var attributes = prop.GetCustomAttributes(typeof(BaseAttribute), true);
                // lấy ra tên của trường dữ liệu
                var nameAttribute = (NameAttribute)prop.GetCustomAttributes(typeof(NameAttribute), true)[0];

                // duyệt từng  Attr validate ở mỗi prop
                foreach (var attribute in attributes)
                {
                    // lấy ra tên decorator tương ứng với attribte 
                    var decoratorName = $"{attribute.GetType().Name}Decorator";

                    // tạo mới decorator
                    object obj = Activator.CreateInstance(Type.GetType($"{decorator.GetType().Namespace}.{decoratorName}"));

                    var newDecorator = (BaseDecorator)obj;

                    // dùng decorator mới tạo để wrap các decorator trưoc đó 
                    newDecorator.nextDecorator = decorator;
                    newDecorator.Attr = (BaseAttribute)attribute;
                    newDecorator.PropValue = prop.GetValue(entity);
                    newDecorator.Name = nameAttribute.Name;
                    newDecorator.FieldNameError = prop.Name;
                    decorator = newDecorator;
                }
                // thực hiện validate
                decorator.Validate();
                if (decorator.Error != null)
                {
                    errors.Add(decorator.Error);
                }
            }
            return errors;

        }
    }
}
