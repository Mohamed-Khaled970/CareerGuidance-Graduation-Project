namespace CareerGuidance.Api.Abstractions
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; } = default!;

        
        public static Result Success() => new(true, Error.None);
        public static Result Failuer(Error error) => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
        public static Result<TValue> Failuer<TValue>(Error error) => new(default, false, error);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Failure results cannot have value");

    }

    /* Result<TValue> Explaination:
     * if successed , it will make object of Result<AuthService>
     * which containt : IsSuccess = true , Error = none , value type => AuthResponse which contains its properties and its values
     * the generic result class have constructor recieves the object and it's type , isSuccess and error 
     * by using constructor chaining , it send isSuccess and error to the parent class to assign fileds with this vlaues and make validations
     * in  Result<TValue> it assign the object type and value to _value
     * at the end there is a property Value return this object only if the result is success else it throw an exception
     */


}
