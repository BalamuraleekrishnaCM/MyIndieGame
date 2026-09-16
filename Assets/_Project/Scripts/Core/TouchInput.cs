using UnityEngine;

namespace MyIndieGame.Core
{
    public static class TouchInput
    {
        public static bool PressedThisFrame()
        {
            if (Input.touchCount > 0)
                for (int i = 0; i < Input.touchCount; i++)
                    if (Input.GetTouch(i).phase == TouchPhase.Began) return true;
            return Input.GetMouseButtonDown(0);
        }

        public static Vector2 ScreenPosition()
        {
            if (Input.touchCount > 0) return Input.GetTouch(0).position;
            return Input.mousePosition;
        }
    }
}
