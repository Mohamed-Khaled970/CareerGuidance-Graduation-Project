using FluentValidation;

namespace CareerGuidance.Api.DTO.Validator
{
    /*
     * File Name: LoginUserDtoValidator.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies:
     *      - FluentValidation
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     *
     * Description:
     *      This class provides validation rules for the LoginUserDto record.
     *      It uses FluentValidation to ensure that the provided email and password
     *      meet the required criteria before further processing.
     *      - Email must be a valid email address and cannot be empty.
     *      - Password cannot be empty.
     */
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            /*
        * Rule: EmailOrUsername
        * Description: Validates that the EmailOrUsername property is not empty 
        * and is either a valid email address or a valid username.
        */
            RuleFor(x => x.EmailOrUsername)
                .NotEmpty()
                .Must(IsValidEmailOrUsername)
                .WithMessage("The field must be a valid email address or username.");

            /*
             * Rule: Password
             * Description: Validates that the Password property is not empty.
             */
            RuleFor(x => x.Password).NotEmpty();
        }

        private bool IsValidEmailOrUsername(string emailOrUsername)
        {
            return IsValidEmail(emailOrUsername) || IsValidUsername(emailOrUsername);
        }

        private bool IsValidEmail(string email)
        {
            // استخدم الدالة المدمجة للتحقق من صحة البريد الإلكتروني
            return new EmailAddressAttribute().IsValid(email);
        }

        private bool IsValidUsername(string username)
        {
            // تعريف قواعد التحقق من صحة الـ username، هنا مثلاً يكون على الأقل 3 أحرف
            return username.Length >= 3;
        }
    }
}
