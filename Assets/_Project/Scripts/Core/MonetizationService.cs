using System;
namespace MyIndieGame.Core
{
 public interface IStoreProvider{bool IsInitialized{get;}void Initialize(Action<bool,string> completed);void Purchase(string productId,Action<bool,string> completed);void Restore(Action<bool,string> completed);}
 public static class MonetizationService
 {
  public static IStoreProvider Provider{get;private set;} public static bool IsInitialized=>Provider!=null&&Provider.IsInitialized;
  public static void Configure(IStoreProvider provider){Provider=provider;}
  public static void Purchase(string productId,Action<bool,string> completed){if(Provider==null){completed?.Invoke(false,"Store provider is not configured.");return;}if(StoreCatalog.Find(productId)==null){completed?.Invoke(false,"Unknown store product.");return;}Provider.Purchase(productId,completed);}
  public static void Restore(Action<bool,string> completed){if(Provider==null){completed?.Invoke(false,"Store provider is not configured.");return;}Provider.Restore(completed);}
 }
}