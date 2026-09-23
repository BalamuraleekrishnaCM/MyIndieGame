using UnityEngine;
namespace MyIndieGame.Core
{
 public static class FeedbackOrchestrator
 {
  public static void Success(Transform target=null){AudioFeedback.Instance?.Success();HapticFeedback.Light();if(target!=null&&!AccessibilitySettings.ReducedMotion)GameFeel.Instance?.Punch(target);}
  public static void Fail(Transform target=null){AudioFeedback.Instance?.Fail();HapticFeedback.Light();if(target!=null&&!AccessibilitySettings.ReducedMotion)GameFeel.Instance?.Punch(target,1.04f,0.05f);}
  public static void Click(){AudioFeedback.Instance?.Click();}
 }
}