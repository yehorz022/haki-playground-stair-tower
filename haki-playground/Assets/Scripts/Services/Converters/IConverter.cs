namespace Assets.Scripts.Services.Converters
{
    public interface IConverter<TIn, TOut>
    {
        TOut Convert(TIn input);
    }
}