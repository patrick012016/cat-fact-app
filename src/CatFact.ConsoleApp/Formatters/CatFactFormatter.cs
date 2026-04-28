using CatFactApp.Models;

namespace CatFactApp.Formatters;

public class CatFactFormatter : ITextFormatter<CatFactModel>
{
    public string Format(CatFactModel model)
    {
        return
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Fact: {model.Fact} (Length: {model.Length}){Environment.NewLine}";
    }
}
