namespace OverTheBoard.Infrastructure.Common
{
    public interface ILocatorInterface<in TObject>
    {
        bool CanSelect(TObject parameter, bool isDefault);
    }
}