using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace AppleBoy
{
    [Serializable]
    public class AnimationListItem
    {
        public string animationName;
        public SpriteAnimationData animation;
        public SpriteAnimationAsset file;
    }

    public class AnimationParamaters
    {
        public string animationState;
        public object[] arguments;

        public AnimationParamaters(string animationState, params object[] arguments)
        {
            this.animationState = animationState;
            this.arguments = arguments;
        }
    }

    public interface IAnimationListener
    {
        void StartAnimation(SpriteAnimation animation, SpriteAnimationData data);

        void FinishAnimation(SpriteAnimation animation, SpriteAnimationData data);

        void KeyFrameUpdate(SpriteAnimation animation, SpriteAnimationData data, int frame, string id);
    }

    [DisallowMultipleComponent, RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        // Events
        public event Action<SpriteAnimation, SpriteAnimationData> OnStartAnimation;
        public event Action<SpriteAnimation, SpriteAnimationData> OnAnimationStartLoop;
        public event Action<SpriteAnimation, SpriteAnimationData> OnStopAnimation;
        public event Action<SpriteAnimation, SpriteAnimationData> OnFinishOrStopAnimation;

        public event Action OnFinishAnimation;
        public event Action<int> OnKeyFrameEvent;

        public Action cacheCompleteAction;
        public Action<int> cacheKeyFrameAction;

        public int CurrentFrame
        {
            get { return currentIdx; }
            set { currentIdx = value; }
        }

        public int CurrentFrameCount
        {
            get
            {
                SpriteAnimationData data;

                if (string.IsNullOrEmpty(currentAnimationName))
                    return 0;

                if (animationsByName.TryGetValue(currentAnimationName, out data))
                {
                    return data.frameDatas.Count;
                }

                return 0;
            }
        }

        public bool m_paused;
        public int animationsCount { get { return list.Count; } }

        [SerializeField]
        public SpriteRenderer spriteRenderer;
        public List<AnimationListItem> list = new List<AnimationListItem>();

        public List<SpriteAnimationAsset> assets = new List<SpriteAnimationAsset>();

        private List<IAnimationListener> _listeners = new List<IAnimationListener>();

        public SpriteAnimationTimeMode mode;
        protected bool _playing = false;

        [SerializeField]
        int animIdx = -1;

        protected SpriteAnimationData currentAnim { get { return (animIdx >= 0 && animIdx < list.Count) ? list[animIdx].animation : null; } }

        public string currentAnimationName { get { return currentAnim != null ? currentAnim.name : string.Empty; } }

        public int currentAnimationIdx { get { return animIdx; } }

        protected int currentIdx = 0;

        [Range(0.001f, 10f)]
        public float speedRatio = 1f;

        public int minPlayFrom = 0;
        public int maxPlayFrom = 0;

        public bool overrideTimeScale;
        public float timeScaleOverride;

        //[Range("minPlayFrom", "maxPlayFrom")]
        public int playFrom = 0;

        public Dictionary<string, SpriteAnimationData> animationsByName = new Dictionary<string, SpriteAnimationData>();
        public bool autoStart = false;

        void OnEnable()
        {
            UpdateAnimations();
        }

        void Start()
        {
            if (autoStart && currentIdx >= 0)
            {
                Play();
            }
        }

        void Update()
        {
            if (!overrideTimeScale)
            {
                if (Math.Abs(speedRatio - AnimTime.timeScale) > float.Epsilon)
                {
                    speedRatio = AnimTime.timeScale;

                    if (speedRatio < 0f)
                    {
                        speedRatio = 0f;
                    }
                }
            }
            else
            {
                if (Math.Abs(speedRatio - timeScaleOverride) > float.Epsilon)
                {
                    speedRatio = timeScaleOverride;

                    if (speedRatio < 0f)
                    {
                        speedRatio = 0f;
                    }
                }
            }
        }

        public void Register(IAnimationListener listener)
        {
            if (_listeners.Contains(listener))
                return;

            _listeners.Add(listener);
        }

        public void Deregister(IAnimationListener listener)
        {
            _listeners.Remove(listener);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
            _listeners = new List<IAnimationListener>();
        }

        public void SetCurrentAnimation(int idx)
        {
            Stop();
            animIdx = idx;
            UpdateAnimations();
            minPlayFrom = 0;
            maxPlayFrom = currentAnim != null ? currentAnim.frameDatas.Count - 1 : 0;
            //playFrom = 0;
            currentIdx = 0;

            if (Application.isPlaying && autoStart)
            {
                Play();
            }
        }

        public void PlayWithOnKeyFrameEvent(string animation, Action<int> action)
        {
            Play(animation);

            if (OnKeyFrameEvent != null && OnKeyFrameEvent.GetInvocationList() != null && OnKeyFrameEvent.GetInvocationList().Length > 0)
            {
                int length = OnKeyFrameEvent.GetInvocationList().Length;
                for (int i = length - 1; i > -1; i--)
                {
                    if (OnKeyFrameEvent.GetInvocationList()[i] != null)
                    {
                        OnKeyFrameEvent -= (Action<int>)OnKeyFrameEvent.GetInvocationList()[i];
                    }
                }
            }

            int frameCount = GetFrameCount();

            cacheKeyFrameAction = (frame) =>
            {
                if (frame == frameCount - 1)
                    OnKeyFrameEvent -= cacheKeyFrameAction;

                action.Invoke(frame);
            };

            OnKeyFrameEvent += cacheKeyFrameAction;


        }

        public void removeAllCallBacks()
        {
            if (OnFinishAnimation != null && OnFinishAnimation.GetInvocationList() != null && OnFinishAnimation.GetInvocationList().Length > 0)
            {
                int length = OnFinishAnimation.GetInvocationList().Length;
                for (int i = length - 1; i > -1; i--)
                {
                    if (OnFinishAnimation.GetInvocationList()[i] != null)
                    {
                        OnFinishAnimation -= (Action)OnFinishAnimation.GetInvocationList()[i];
                    }
                }
            }

            if (OnKeyFrameEvent != null && OnKeyFrameEvent.GetInvocationList() != null && OnKeyFrameEvent.GetInvocationList().Length > 0)
            {
                int length = OnKeyFrameEvent.GetInvocationList().Length;
                for (int i = length - 1; i > -1; i--)
                {
                    if (OnKeyFrameEvent.GetInvocationList()[i] != null)
                    {
                        OnKeyFrameEvent -= (Action<int>)OnKeyFrameEvent.GetInvocationList()[i];
                    }

                }
            }
        }

        public void PlayWithCallBack(object animation, Action action)
        {
            Play(animation);

            if (OnFinishAnimation != null && OnFinishAnimation.GetInvocationList() != null && OnFinishAnimation.GetInvocationList().Length > 0)
            {
                int length = OnFinishAnimation.GetInvocationList().Length;
                for (int i = length - 1; i > -1; i--)
                {
                    if (OnFinishAnimation.GetInvocationList()[i] != null)
                    {
                        OnFinishAnimation -= (Action)OnFinishAnimation.GetInvocationList()[i];
                    }

                }
            }

            cacheCompleteAction = () =>
            {
                OnFinishAnimation -= cacheCompleteAction;
                action();
            };

            OnFinishAnimation += cacheCompleteAction;
        }

        public float GetCurrentAnimationLength()
        {
            SpriteAnimationData data;
            float length = 0f;

            if (animationsByName.TryGetValue(currentAnimationName, out data))
            {
                for (int i = 0; i < currentAnim.frameDatas.Count; i++)
                {
                    length += currentAnim.frameDatas[i].time;
                }
            }

            return length;
        }

        public int GetFrameCount()
        {
            SpriteAnimationData data;
            int frameCount = 0;

            if (animationsByName.TryGetValue(currentAnimationName, out data))
            {
                frameCount = data.frameDatas.Count;
            }

            return frameCount;
        }

        public void SetCurrentAnimation(string aName)
        {
            Stop();
            UpdateAnimations();
            SpriteAnimationData adata = GetAnimationData(aName);
            if (adata != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].animation == adata)
                    {
                        animIdx = i;
                        break;
                    }
                }
            }
            else
            {
                animIdx = -1;
            }
            minPlayFrom = 0;
            maxPlayFrom = currentAnim != null ? currentAnim.frameDatas.Count - 1 : 0;
            //playFrom = 0;
            currentIdx = 0;

            if (Application.isPlaying && autoStart)
            {
                Play();
            }
        }

        public void UpdateAnimations()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animationsByName.Clear();
            list.Clear();
            foreach (SpriteAnimationAsset asset in assets)
            {
                if (asset != null && asset.animations != null && asset.animations.Count > 0)
                {
                    foreach (SpriteAnimationData data in asset.animations)
                    {
                        animationsByName[data.name] = data;
                        AnimationListItem item = new AnimationListItem();
                        item.animation = data;
                        item.animationName = data.name;
                        item.file = asset;
                        list.Add(item);
                    }
                }
            }
        }

        void Reset()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateAnimations();
        }

        public void PlayFormat(string animationState, params object[] args)
        {
            string l_animation = string.Format(animationState, args);

            Play(l_animation);
        }

        public bool Play(object anim)
        {
            return Play(anim.ToString());
        }

        public bool Play(string animName)
        {
            if (!gameObject.activeSelf)
            {
                return false;
            }

            SetCurrentAnimation(animName);

            if (currentAnim != null && currentAnim.Valid())
            {
                PlayCurrentAnim();
                return true;
            }
            return false;
        }

        public bool Play()
        {
            if (currentAnim != null && currentAnim.Valid())
            {
                PlayCurrentAnim();
                return true;
            }
            return false;
        }

        public bool Play(int idx)
        {
            SetCurrentAnimation(idx);
            if (currentAnim != null && currentAnim.Valid())
            {
                PlayCurrentAnim();
                return true;
            }
            return false;
        }

        public void Stop()
        {
            if (!_playing)
                return;
            if (Application.isPlaying && OnStopAnimation != null)
                OnStopAnimation(this, currentAnim);
            _playing = false;
            StopCoroutine("playAnimation");
        }

        protected SpriteAnimationData GetAnimationData(string animName)
        {
            if (animationsByName.ContainsKey(animName) && animationsByName[animName].Valid())
            {
                return animationsByName[animName];
            }
            return null;
        }

        protected SpriteAnimationData GetAnimationData(int idx)
        {
            if (list.Count > idx && list[idx].animation.Valid())
            {
                return list[idx].animation;
            }
            return null;
        }

        protected void PlayCurrentAnim()
        {
            if (!gameObject.activeInHierarchy)
                return;

            Stop();

            if (OnStartAnimation != null)
                OnStartAnimation(this, currentAnim);

            foreach (var listener in _listeners)
            {
                listener.StartAnimation(this, currentAnim);
            }

            //if (Application.isPlaying)
            StartCoroutine("playAnimation");
        }

        protected IEnumerator playAnimation()
        {
            _playing = true;
            currentIdx = playFrom - 1;
            float cfTime = 0f;
            bool loop = false;
            bool isFirstLoop = true;
            float cTime = GetTime();

            do
            {
                if (!isFirstLoop && OnAnimationStartLoop != null)
                    OnAnimationStartLoop(this, currentAnim);
                do
                {
                    cfTime = 0;
                    do
                    {
                        cTime += cfTime;
                        currentIdx++;
                        cfTime = GetCurrentFrameTime();
                    }
                    while (GetTime() > (cTime + cfTime) && !isEndFrame());

                    SetCurrentFrame();

                    foreach (var listener in _listeners)
                    {
                        listener.KeyFrameUpdate(this, currentAnim, currentIdx, currentAnim.frameDatas[currentIdx].eventName);
                    }

                    if (!string.IsNullOrEmpty(currentAnim.frameDatas[currentIdx].eventName))
                    {
                        SendMessage(currentAnim.frameDatas[currentIdx].eventName, SendMessageOptions.DontRequireReceiver);
                    }

                    if (OnKeyFrameEvent != null)
                    {
                        OnKeyFrameEvent(currentIdx);
                    }

                    while (GetTime() < cTime + cfTime)
                    {
                        cfTime = GetCurrentFrameTime();
                        yield return new WaitForEndOfFrame();
                    }
                    cTime += cfTime;
                }
                while (!isEndFrame());

                if (currentAnim.loop == SpriteAnimationLoopMode.LOOPTOSTART)
                {
                    loop = true;
                    currentIdx = -1;
                }
                else if (currentAnim.loop == SpriteAnimationLoopMode.LOOPTOFRAME)
                {
                    loop = true;
                    currentIdx = currentAnim.frameToLoop - 1;
                }
                if (OnFinishAnimation != null)
                {
                    OnFinishAnimation();
                }

                foreach (var listener in _listeners)
                {
                    listener.FinishAnimation(this, currentAnim);
                }

                isFirstLoop = false;
            }
            while (loop);
            yield return new WaitForEndOfFrame();
        }

        protected float GetTime()
        {
            if (m_paused)
                return 0f;
            if (mode == SpriteAnimationTimeMode.NORMAL)
                return AnimTime.time;
            else if (mode == SpriteAnimationTimeMode.TIMESCALEINDEPENDENT)
                return Time.realtimeSinceStartup;
            return 0;
        }

        protected void SetCurrentFrame()
        {
            if (currentIdx >= currentAnim.frameDatas.Count)
            {
                Debug.LogWarningFormat("trying to play frame {0} but only {1} frames.", currentIdx, currentAnim.frameDatas.Count);
                return;
            }

            spriteRenderer.sprite = currentAnim.frameDatas[currentIdx].sprite;
        }

        protected float GetCurrentFrameTime()
        {
            if (currentAnim == null)
                return 0f;

            if (currentIdx >= currentAnim.frameDatas.Count)
                return 0f;

            if (Mathf.Abs(speedRatio - 1) < float.Epsilon)
            {
                return currentAnim.frameDatas[currentIdx].time;
            }

            return currentAnim.frameDatas[currentIdx].time * (1 / speedRatio); ;
        }

        protected bool isEndFrame()
        {
            if (currentAnim == null)
                return true;

            if (currentAnim.frameDatas == null)
                return true;

            if (currentAnim.frameDatas.Count <= 0)
                return true;

            return currentIdx >= currentAnim.frameDatas.Count - 1;
        }

        void Destroy()
        {
            OnStopAnimation = null;
            OnFinishOrStopAnimation = null;
            OnFinishAnimation = null;
        }
    }
}
