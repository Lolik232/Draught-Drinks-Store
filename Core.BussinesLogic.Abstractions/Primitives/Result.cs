namespace Core.BussinesLogic.Abstractions.Primitives
{
    public sealed record Error(string Code, string? Description = null)
    {
        public static readonly Error None = new(string.Empty);
        public static readonly Error NullValue = new("Error.Null", "null");

        public static implicit operator Result(Error error) => Result.Failture(error);
    }

    public class Result
    {
        protected internal Result(bool isSucsess, Error error)
        {
            if (isSucsess && error != Error.None ||
                !isSucsess && error == Error.None)
            {
                throw new ArgumentException("Invalid error value", nameof(error));
            }

            IsSucsess = isSucsess;
            Error = error;
        }

        public bool IsSucsess { get; }
        public bool IsFailture => !IsSucsess;

        public Error Error { get; }

        public static Result Sucsess() => new(true, Error.None);
        public static Result Failture(Error error) => new(false, error);

        public static Result<TValue> Sucsess<TValue>(TValue? value) => new(value, true, Error.None);
        public static Result<TValue> Failture<TValue>(Error error) => new(default, false, error);

        public static implicit operator Result(Error error) => Failture(error);
    }


    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected internal Result(TValue? value, bool isSucsess, Error error) : base(isSucsess, error) => _value = value;

        public TValue Value => IsSucsess ? _value! : throw new InvalidOperationException("The value of a failture result can't be accessed");

        public static implicit operator Result<TValue>(TValue? value) =>
            value is not null ? Sucsess(value) : Failture<TValue>(Error.NullValue);

        public static implicit operator Result<TValue>(Error error) => Failture<TValue>(error);

    }
}

