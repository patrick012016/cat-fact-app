namespace CatFactApp.Formatters;

public interface ITextFormatter<T>
{
    string Format(T model);
}
