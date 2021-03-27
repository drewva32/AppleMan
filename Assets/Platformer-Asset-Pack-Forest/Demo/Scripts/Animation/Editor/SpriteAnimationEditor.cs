using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace AppleBoy
{
    [CustomEditor(typeof(SpriteAnimation))]
    public class SpriteAnimationEditor : Editor
    {
        protected SpriteAnimation sp;
        protected List<string> animationNames = new List<string>();
        protected bool cachedAnimationNames = false;
        protected int selectedAnimationIdx;

        protected bool changedAnim = false;
        protected int changedAnimIdx = -1;

        public override void OnInspectorGUI()
        {
            sp = target as SpriteAnimation;

            var obj = new SerializedObject(sp);

            obj.Update();

            CacheAnimationNames();

            var spAutoStart = obj.FindProperty("autoStart");
            var spMode = obj.FindProperty("mode");
            var spAssets = obj.FindProperty("assets");
            var spSpeedRatio = obj.FindProperty("speedRatio");

            EditorGUILayout.PropertyField(spAutoStart);
            EditorGUILayout.PropertyField(spSpeedRatio, new GUIContent("Speed Ratio: "));
            EditorGUILayout.PropertyField(spMode);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(spAssets, true);
            if (EditorGUI.EndChangeCheck())
            {
                obj.ApplyModifiedProperties();
                cachedAnimationNames = false;
                CacheAnimationNames();

                int newIdx = GetSelectedAnimationIdx();
                if (newIdx == -1)
                {
                    SetCurrentAnimation(string.Empty);
                }
            }
            selectedAnimationIdx = sp.currentAnimationIdx + 1; //GetSelectedAnimationIdx();

            int lastAnimIdx = selectedAnimationIdx;
            selectedAnimationIdx = EditorGUILayout.Popup("Selected Animation: ", selectedAnimationIdx, animationNames.ToArray());
            if (selectedAnimationIdx != lastAnimIdx)
            {
                obj.ApplyModifiedProperties();
                //SetSelectedAnimation();
                //sp.Play(selectedAnimationIdx-1);
                changedAnim = true;
                changedAnimIdx = selectedAnimationIdx;
            }
            if (sp.currentAnimationName != null)
                EditorGUILayout.PropertyField(obj.FindProperty("playFrom"), new GUIContent("Play from keyframe: "));

            // draw list
            GUILayout.Space(20);
            SerializedProperty animList = obj.FindProperty("list");
            float nameSize = 140;
            if (animList.arraySize > 0)
            {
                GUILayoutOption[] options = new GUILayoutOption[2];
                options[0] = GUILayout.MaxWidth(265);
                options[1] = GUILayout.MinWidth(265);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                Rect itemRect = GUILayoutUtility.GetRect(200, 18, options);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUI.Box(itemRect, new GUIContent(string.Empty));
                itemRect.width = nameSize;
                EditorGUI.LabelField(itemRect, "Animation Name");
                itemRect.xMin = itemRect.xMax;
                itemRect.width = 45;
                EditorGUI.LabelField(itemRect, "Index");

                for (int i = 0; i < animList.arraySize; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    itemRect = GUILayoutUtility.GetRect(200, 18, options);
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    itemRect.width = nameSize;
                    GUI.Box(itemRect, new GUIContent(string.Empty));
                    SerializedProperty item = animList.GetArrayElementAtIndex(i);
                    EditorGUI.LabelField(itemRect, item.FindPropertyRelative("animationName").stringValue);
                    itemRect.xMin = itemRect.xMax;
                    itemRect.width = 45;
                    GUI.Box(itemRect, new GUIContent(string.Empty));
                    EditorGUI.LabelField(itemRect, i.ToString());
                    itemRect.xMin = itemRect.xMax;
                    itemRect.width = 30;
                    if (i > 0 && GUI.Button(itemRect, "Up"))
                    {
                        animList.MoveArrayElement(i, i - 1);
                        if (sp.currentAnimationIdx == i)
                            obj.FindProperty("animIdx").intValue--;
                        else if (sp.currentAnimationIdx == i - 1)
                            obj.FindProperty("animIdx").intValue++;
                        obj.ApplyModifiedProperties();
                        return;
                    }
                    itemRect.xMin = itemRect.xMax;
                    itemRect.width = 50;
                    if (i < animList.arraySize - 1 && GUI.Button(itemRect, "Down"))
                    {
                        animList.MoveArrayElement(i, i + 1);
                        if (sp.currentAnimationIdx == i)
                            obj.FindProperty("animIdx").intValue++;
                        else if (sp.currentAnimationIdx == i + 1)
                            obj.FindProperty("animIdx").intValue--;
                        obj.ApplyModifiedProperties();
                        return;
                    }
                }
            }
            if (Event.current.type == EventType.Repaint && changedAnim)
            {
                changedAnim = false;
                sp.SetCurrentAnimation(changedAnimIdx - 1);
                //sp.Play(changedAnimIdx-1);
            }
            if (GUI.changed)
                obj.ApplyModifiedProperties();
        }

        protected void SetCurrentAnimation(string aName)
        {
            sp.SetCurrentAnimation(aName);
        }

        protected int GetSelectedAnimationIdx()
        {
            int i = 1;
            for (; i < animationNames.Count; i++)
            {
                if (animationNames[i] == sp.currentAnimationName)
                {
                    return i;
                }
            }
            return 0;
        }

        protected void SetSelectedAnimation()
        {
            sp.SetCurrentAnimation(selectedAnimationIdx - 1);
        }

        protected void CacheAnimationNames()
        {
            if (cachedAnimationNames == false)
            {
                sp.UpdateAnimations();
                animationNames.Clear();
                animationNames.Add("Not Set");
                foreach (SpriteAnimationAsset asset in sp.assets)
                {
                    if (asset != null && asset.animations != null && asset.animations.Count > 0)
                    {
                        foreach (SpriteAnimationData data in asset.animations)
                        {
                            animationNames.Add(data.name);
                        }
                    }
                }
                selectedAnimationIdx = GetSelectedAnimationIdx();
                cachedAnimationNames = true;
            }
        }
    }
}
