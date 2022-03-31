namespace OverTheBoard.Infrastructure.Common
{
    public interface ILocatorService<out TInterface, in TObject> where TInterface : ILocatorInterface<TObject>
    {
        TInterface Get(TObject parameter);
    }
}