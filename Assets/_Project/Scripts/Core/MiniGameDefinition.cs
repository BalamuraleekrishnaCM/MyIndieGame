using System;
using UnityEngine;

namespace MyIndieGame.Core
{
    [Serializable]
    public class MiniGameDefinition
    {
        public string Id;
        public string Title;
        [TextArea] public string Description;
        public string SceneName;
        public bool IsAvailable = true;
    }
}
