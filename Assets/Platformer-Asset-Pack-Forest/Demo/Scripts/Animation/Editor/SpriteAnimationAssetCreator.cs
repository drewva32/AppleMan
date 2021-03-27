using System.Linq;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using System.Collections.Generic;

namespace AppleBoy
{
    public class SpriteAnimationAssetCreation
    {
        public const float DEFAULT_FRAME_TIME = 0.06F;
        public const string ANIMATION_SAVE_PATH = "Assets/Source/Animations/";

        [MenuItem("Assets/Sprite/Create/Animation Asset")]
        public static void CreateCustom()
        {
            var asset = ScriptableObject.CreateInstance<SpriteAnimationAsset>();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                asset.GetInstanceID(),
                ScriptableObject.CreateInstance<EndSpriteAnimationAssetNameEdit>(),
                "SpriteAnimationAsset.asset",
                AssetPreview.GetMiniThumbnail(asset),
                null);

        }

        [MenuItem("Assets/Sprite/Create/Animation")]
        public static void CreateSpriteAnimation()
        {
            string[] l_guids = Selection.assetGUIDs;

            if (null == l_guids || l_guids.Length == 0)
                return;

            for (int i = 0; i < l_guids.Length; i++)
            {
                string l_path = AssetDatabase.GUIDToAssetPath(l_guids[i]);
                Texture2D l_texture = AssetDatabase.LoadAssetAtPath<Texture2D>(l_path);

                if (null == l_texture)
                    continue;

                string l_name = l_texture.name.Split('_')[0];

                if (!l_name.Contains("@"))
                    continue;

                string l_animationAssetName = l_name.Split('@')[0];
                string l_animationName = l_name.Split('@')[1];

                if (!System.IO.Directory.Exists(getAnimationAssetSavePath()))
                {
                    Debug.LogErrorFormat("No Path {0} Found!\nYou need to create this path or change the path inside SpriteAnimationAssetCreator.cs in the 'getAnimationAssetSavePath()' method.", getAnimationAssetSavePath());
                    break;
                }

                string l_animationAssetPath = getAnimationAssetSavePath() + l_animationAssetName + ".asset";
                SpriteAnimationAsset l_animationAsset = AssetDatabase.LoadAssetAtPath<SpriteAnimationAsset>(l_animationAssetPath);

                if (l_animationAsset == null)
                {
                    l_animationAsset = ScriptableObject.CreateInstance<SpriteAnimationAsset>();
                    l_animationAsset.name = l_animationAssetName;
                    l_animationAsset.animations = new List<SpriteAnimationData>();
                    AssetDatabase.CreateAsset(l_animationAsset, AssetDatabase.GenerateUniqueAssetPath(l_animationAssetPath));
                }

                SpriteAnimationData l_animationData = new SpriteAnimationData();
                l_animationData.name = l_animationName;
                l_animationData.loop = SpriteAnimationLoopMode.LOOPTOSTART;
                l_animationData.frameDatas = new List<SpriteAnimationFrameData>();

                Sprite[] l_sprites = AssetDatabase.LoadAllAssetsAtPath(l_path).OfType<Sprite>().ToArray();

                for (int j = 0; j < l_sprites.Length; j++)
                {
                    Sprite l_sprite = l_sprites[j];
                    SpriteAnimationFrameData l_frameData = new SpriteAnimationFrameData();
                    l_frameData.time = DEFAULT_FRAME_TIME;
                    l_frameData.sprite = l_sprite;
                    l_animationData.frameDatas.Add(l_frameData);
                }

                l_animationAsset.animations.Add(l_animationData);

                EditorUtility.SetDirty(l_animationAsset);
            }
        }

        private static string getAnimationAssetSavePath()
        {
            return "Assets/Platformer-Asset-Pack-Forest/Demo/Animations/";
        }
    }

    internal class EndSpriteAnimationAssetNameEdit : EndNameEditAction
    {
        public override void Action(int InstanceID, string path, string file)
        {
            AssetDatabase.CreateAsset(EditorUtility.InstanceIDToObject(InstanceID), AssetDatabase.GenerateUniqueAssetPath(path));
        }
    }
}
