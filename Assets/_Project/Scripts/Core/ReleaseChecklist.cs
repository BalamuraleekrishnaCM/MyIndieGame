using System;
namespace MyIndieGame.Core
{
 public sealed class ReleaseChecklist
 {
  public bool UnityBuildVerified{get;set;} public bool DeviceSmokeTested{get;set;} public bool CrashFreeReview{get;set;} public bool StoreMetadataReady{get;set;} public bool BackendProductionReady{get;set;} public bool PrivacyConsentReady{get;set;}
  public bool IsReady=>UnityBuildVerified&&DeviceSmokeTested&&CrashFreeReview&&StoreMetadataReady&&BackendProductionReady&&PrivacyConsentReady;
  public string BlockingReason(){if(!UnityBuildVerified)return "Unity build verification is required.";if(!DeviceSmokeTested)return "Device smoke testing is required.";if(!CrashFreeReview)return "Crash/error review is required.";if(!StoreMetadataReady)return "Store metadata is required.";if(!BackendProductionReady)return "Backend production configuration is required.";if(!PrivacyConsentReady)return "Privacy consent configuration is required.";return string.Empty;}
 }
}