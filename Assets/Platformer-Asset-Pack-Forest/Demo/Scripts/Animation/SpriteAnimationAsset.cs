using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AppleBoy
{
    public enum SpriteAnimationTimeMode
    {
        NORMAL = 0,
        TIMESCALEINDEPENDENT = 1
    }

    ;

    public enum SpriteAnimationLoopMode
    {
        NOLOOP = 0,
        LOOPTOSTART = 1,
        LOOPTOFRAME = 2
    }

    ;

    [System.Serializable]
    public class SpriteAnimationFrameData
    {
        public Sprite sprite;
        public float time;
        public bool eventEnabled;
        public string eventName;
    }

    [System.Serializable]
    public class SpriteAnimationData
    {
        [Range(0.001f, 10f)]
        public float speedRatio = 1f;
        [SerializeField]
        public SpriteAnimationLoopMode loop;
        public int frameToLoop = 0;
        [SerializeField]
        public List<SpriteAnimationFrameData> frameDatas = new List<SpriteAnimationFrameData>();
        public string name;
        public int selectedIndex = 0;
        public float newFramesTime = 0.05f;
        public float setFramesTime = 0.1f;

        public bool Valid()
        {
            return speedRatio > 0 && frameToLoop >= 0 && frameDatas.Count > 0;
        }
    }

    public class SpriteAnimationAsset : ScriptableObject
    {
        public bool editorConfirmations = true;
        public List<SpriteAnimationData> animations = new List<SpriteAnimationData>();
    }
}

