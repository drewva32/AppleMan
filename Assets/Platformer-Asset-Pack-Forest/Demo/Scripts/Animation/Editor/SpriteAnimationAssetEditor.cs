using UnityEngine;
using UnityEditor;
using System.Linq;

namespace AppleBoy
{
    public class CustomDragData
    {
        public int originalIndex = -1;
        public int animationIdx = -1;
    }

    [CustomEditor(typeof(SpriteAnimationAsset))]
    public class SpriteAnimationAssetInspector : Editor
    {
        protected static Event currentEvent;
        protected static Vector2 mousePosition;
        protected bool isDragginKeyframe = false;

        protected const int FRAMEQUAD_WIDTH = 25;
        protected const int FRAMEQUAD_HEIGHT = 25;
        protected const int FRAMEQUAD_WIDTH_SEPARATION = 5;
        protected const int FRAMEQUAD_HEIGHT_SEPARATION = 5;
        protected float usableWidth;
        protected SerializedProperty spEditorConfirmations;

        [MenuItem("GameObject/2D Object/Animated Sprite")]
        public static void CreateSpriteGameObject()
        {
            GameObject n = new GameObject();
            n.AddComponent<SpriteRenderer>();
            n.AddComponent<SpriteAnimation>();
            n.name = "New Animated Sprite";
        }

        protected static string DRAG_DATA_KEY = "testkey";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            spEditorConfirmations = serializedObject.FindProperty("editorConfirmations");

            currentEvent = Event.current;

            if (currentEvent.isMouse)
            {
                mousePosition = currentEvent.mousePosition;
            }

            usableWidth = Screen.width - 50;
            Rect buttonsSize = GUILayoutUtility.GetRect(usableWidth, 20);
            GUI.Box(buttonsSize, string.Empty);
            buttonsSize.width = 30;
            SerializedProperty p = serializedObject.FindProperty("animations");

            if (GUI.Button(buttonsSize, "+"))
            {
                int idx = p.arraySize;
                p.InsertArrayElementAtIndex(idx);
                SerializedProperty newItem = p.GetArrayElementAtIndex(idx);
                newItem.FindPropertyRelative("newFramesTime").floatValue = 0.08f;
                newItem.FindPropertyRelative("speedRatio").floatValue = 1;
                newItem.FindPropertyRelative("name").stringValue = "new animation";
                newItem.FindPropertyRelative("loop").enumValueIndex = 0;
                newItem.FindPropertyRelative("frameToLoop").intValue = 0;
                newItem.FindPropertyRelative("frameDatas").arraySize = 0;
                newItem.FindPropertyRelative("selectedIndex").intValue = -1;
                serializedObject.ApplyModifiedProperties();
                return;
            }
            buttonsSize.xMin = usableWidth - 80;
            buttonsSize.width = 95;
            spEditorConfirmations.boolValue = EditorGUI.ToggleLeft(buttonsSize, new GUIContent("Confirmations"), spEditorConfirmations.boolValue);

            float currentHeight = 20;
            float lastMinY = 75;
            for (int i = 0; i < p.arraySize; i++)
            {
                SerializedProperty item = p.GetArrayElementAtIndex(i);

                SerializedProperty spframeDatas = item.FindPropertyRelative("frameDatas");

                float aditional = 0;

                if (spframeDatas != null && spframeDatas.arraySize > 0)
                {
                    aditional = ((spframeDatas.arraySize) * (FRAMEQUAD_WIDTH + FRAMEQUAD_WIDTH_SEPARATION));
                    aditional = (Mathf.Ceil(aditional / usableWidth)) * (FRAMEQUAD_HEIGHT + FRAMEQUAD_HEIGHT_SEPARATION);
                    aditional += 160;
                }
                currentHeight = 175 + aditional;

                Rect totalSize = GUILayoutUtility.GetRect(usableWidth, currentHeight);
                totalSize.yMin = lastMinY;
                totalSize.height = currentHeight;
                DrawListItem(i, totalSize, item, (target as SpriteAnimationAsset).animations[i], new GUIContent(string.Empty));
                lastMinY = totalSize.yMax;
                Rect supressButton = new Rect(totalSize.xMax - 40, totalSize.yMin + 5, 40, 18);
                if (GUI.Button(supressButton, "X"))
                {
                    p.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }
            }

            switch (currentEvent.type)
            {
                case EventType.MouseDrag:
                    CustomDragData existingDragData = DragAndDrop.GetGenericData("dragKeyframe") as CustomDragData;
                    if (existingDragData != null)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                        isDragginKeyframe = true;
                        DragAndDrop.StartDrag("Dragging List ELement");
                        currentEvent.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (isDragginKeyframe)
                    {
                        DragAndDrop.paths = null;
                        DragAndDrop.objectReferences = new UnityEngine.Object[0];
                        DragAndDrop.PrepareStartDrag();// reset data
                        isDragginKeyframe = false;
                    }
                    break;
                case EventType.DragExited:
                    if (isDragginKeyframe)
                    {
                        DragAndDrop.paths = null;
                        DragAndDrop.objectReferences = new UnityEngine.Object[0];
                        isDragginKeyframe = false;
                        DragAndDrop.PrepareStartDrag();
                        currentEvent.Use();
                    }
                    break;
            }

            if (GUI.changed)
                serializedObject.ApplyModifiedProperties();
        }

        public void DrawListItem(int animationIdx, Rect position, SerializedProperty property, SpriteAnimationData spriteAnimationData, GUIContent label)
        {
            int frameQuadRows = 1;
            frameQuadRows = 1;
            SerializedProperty spNewFramesTime = property.FindPropertyRelative("newFramesTime");
            SerializedProperty spSetFramesTime = property.FindPropertyRelative("setFramesTime");
            SerializedProperty spSpeedRatio = property.FindPropertyRelative("speedRatio");
            SerializedProperty spName = property.FindPropertyRelative("name");
            SerializedProperty spLoop = property.FindPropertyRelative("loop");
            SerializedProperty spFrameToLoop = property.FindPropertyRelative("frameToLoop");
            SerializedProperty spframeDatas = property.FindPropertyRelative("frameDatas");
            SerializedProperty spSelectedIdx = property.FindPropertyRelative("selectedIndex");


            Rect totalFrames = new Rect(position.xMin + 100, position.yMin + 100, 230, 40);


            object[] dragItems = EditorExtensions.DropZone<Object>("Drag Sprites here", totalFrames);


            // test sprites
            if (!isDragginKeyframe && dragItems != null && dragItems.Length > 0)
            {
                for (int i = 0; i < dragItems.Length; i++)
                {
                    Sprite l_sprite = dragItems[i] as Sprite;

                    if (null == l_sprite)
                    {
                        Texture2D l_tex = dragItems[i] as Texture2D;

                        if (null == l_tex)
                            continue;

                        string l_texPath = AssetDatabase.GetAssetPath(l_tex);
                        Sprite[] l_sprites = AssetDatabase.LoadAllAssetsAtPath(l_texPath).OfType<Sprite>().ToArray();

                        for (int j = 0; j < l_sprites.Length; j++)
                        {
                            Sprite o = l_sprites[j];

                            if (spframeDatas.arraySize == 0)
                            {
                                spframeDatas.InsertArrayElementAtIndex(0);
                                spSelectedIdx.intValue = 0;
                            }
                            else
                            {
                                spframeDatas.InsertArrayElementAtIndex(spSelectedIdx.intValue);
                                spSelectedIdx.intValue++;
                            }
                            SerializedProperty newItem = spframeDatas.GetArrayElementAtIndex(spSelectedIdx.intValue);
                            newItem.FindPropertyRelative("sprite").objectReferenceValue = o;
                            newItem.FindPropertyRelative("time").floatValue = spNewFramesTime.floatValue;
                        }
                    }
                    else
                    {
                        if (spframeDatas.arraySize == 0)
                        {
                            spframeDatas.InsertArrayElementAtIndex(0);
                            spSelectedIdx.intValue = 0;
                        }
                        else
                        {
                            spframeDatas.InsertArrayElementAtIndex(spSelectedIdx.intValue);
                            spSelectedIdx.intValue++;
                        }
                        SerializedProperty newItem = spframeDatas.GetArrayElementAtIndex(spSelectedIdx.intValue);
                        newItem.FindPropertyRelative("sprite").objectReferenceValue = l_sprite;
                        newItem.FindPropertyRelative("time").floatValue = spNewFramesTime.floatValue;
                    }

                }
                serializedObject.ApplyModifiedProperties();
                return;
            }


            // 0
            Rect newPos = new Rect(position.xMin, position.yMin, position.width, 20);
            newPos.height = 1;
            GUI.Box(newPos, new GUIContent(string.Empty));

            // show content
            newPos = new Rect(position.xMin, position.yMin + 5, 20, 20);
            bool showContent = EditorPrefs.GetBool(string.Format("Animx_{0}_{1}", animationIdx, spName.stringValue), false);
            showContent = EditorGUI.ToggleLeft(newPos, string.Empty, showContent);
            EditorPrefs.SetBool(string.Format("Animx_{0}_{1}", animationIdx, spName.stringValue), showContent);

            // 1
            newPos = new Rect(position.xMin + 20, position.yMin + 5, position.width, 20);
            EditorGUI.LabelField(newPos, "Name: ");
            newPos.xMin = 90;
            newPos.width = 150;
            EditorGUI.PropertyField(newPos, spName, new GUIContent(string.Empty));


            // play mode
            //            newPos = new Rect (position.xMin + 240, position.yMin + 5, 40, 20);
            //            bool playMode = EditorPrefs.GetBool ( string.Format ( "Animx_Play_{0}_{1}", animationIdx , spName.stringValue ) , false );
            //
            //            GUI.backgroundColor = playMode ? EditorColor.green : Color.white;
            //
            //            if ( GUI.Button ( newPos , "Play" ) )
            //            {
            //                playMode = !playMode;
            //
            //                EditorPrefs.SetBool ( string.Format ( "Animx_Play_{0}_{1}", animationIdx , spName.stringValue ) , playMode );
            //
            //                if ( playMode )
            //                {
            //                    var item = spframeDatas.GetArrayElementAtIndex (spSelectedIdx.intValue);
            //
            //                    var time = item.FindPropertyRelative ( "time" ).floatValue;
            //
            //                    var speed = spSpeedRatio.floatValue;
            //
            //                    EditorPrefs.SetFloat ( string.Format ( "Animx_ElapsedFrameTime_{0}_{1}", animationIdx , spName.stringValue ) , (float)(EditorApplication.timeSinceStartup + (time * speed)) );
            //                }
            //            }
            //
            //            GUI.backgroundColor = Color.white;


            if (!showContent)
            {
                int length = spframeDatas.arraySize;

                newPos = new Rect(position.xMin, position.yMin + 30, position.width, 20);

                if (length <= 0)
                {
                    EditorGUI.HelpBox(newPos, "No Frames in animation!", MessageType.Error);
                    return;
                }

                for (int i = 0; i < length; i++)
                {
                    var item = spframeDatas.GetArrayElementAtIndex(i);

                    var spriteProperty = item.FindPropertyRelative("sprite");

                    if (spriteProperty.objectReferenceValue == null)
                    {
                        EditorGUI.HelpBox(newPos, "One or more frames does not have a reference value!", MessageType.Error);
                        return;
                    }

                }

                EditorGUI.HelpBox(newPos, "Looks Great!", MessageType.Info);
                return;
            }

            // 2
            newPos = new Rect(position.xMin, position.yMin + 30, position.width, 20);
            EditorGUI.LabelField(newPos, "Loop Mode: ");
            newPos.xMin = 90;
            newPos.width = 120;
            EditorGUI.PropertyField(newPos, spLoop, new GUIContent(string.Empty));
            if (spriteAnimationData.loop == SpriteAnimationLoopMode.LOOPTOFRAME)
            {
                newPos.xMin = 220;
                newPos.width = 90;
                EditorGUI.LabelField(newPos, "To Frame: ");
                newPos.xMin = 294;
                newPos.width = 50;
                EditorGUI.PropertyField(newPos, spFrameToLoop, new GUIContent(string.Empty));
            }
            // 3
            newPos = new Rect(position.xMin, position.yMin + 53, position.width, 20);

            newPos.width = 90;
            EditorGUI.LabelField(newPos, new GUIContent("Speed Ratio:"));
            newPos.xMin = newPos.xMax + 20;
            newPos.width = 220;
            EditorGUI.PropertyField(newPos, spSpeedRatio, new GUIContent(string.Empty));
            //4
            newPos = new Rect(position.xMax, position.yMin + 76, position.width, 20);
            newPos.xMin = position.xMin;
            newPos.width = 120;
            EditorGUI.LabelField(newPos, new GUIContent("Set Frame Times: "));
            newPos.xMin = newPos.xMax + 10;
            newPos.width = 60;
            EditorGUI.PropertyField(newPos, spSetFramesTime, new GUIContent(string.Empty));
            newPos.xMin = newPos.xMax + 10;
            newPos.width = 60;

            if (GUI.Button(newPos, "Set"))
            {
                int l_count = spframeDatas.arraySize;

                for (int i = 0; i < l_count; i++)
                {
                    SerializedProperty item = spframeDatas.GetArrayElementAtIndex(i);
                    item.FindPropertyRelative("time").floatValue = spSetFramesTime.floatValue;
                }
            }
            // 5
            newPos = new Rect(position.xMin, position.yMin + 99, position.width, 20);
            newPos.width = 40;
            if (GUI.Button(newPos, "+"))
            {
                if (spframeDatas.arraySize == 0)
                {
                    spSelectedIdx.intValue = 0;
                    spframeDatas.InsertArrayElementAtIndex(spSelectedIdx.intValue);
                }
                else
                {
                    spframeDatas.InsertArrayElementAtIndex(spSelectedIdx.intValue);
                    spSelectedIdx.intValue++;
                }
                SerializedProperty citem = spframeDatas.GetArrayElementAtIndex(spSelectedIdx.intValue);
                EditorGUIUtility.PingObject(citem.FindPropertyRelative("sprite").objectReferenceValue);
                citem.FindPropertyRelative("sprite").objectReferenceValue = null;
                citem.FindPropertyRelative("time").floatValue = spNewFramesTime.floatValue;
                citem.FindPropertyRelative("eventEnabled").boolValue = false;
                citem.FindPropertyRelative("eventName").stringValue = string.Empty;
            }
            newPos.xMin += 50;
            newPos.width = 40;
            if (GUI.Button(newPos, "-"))
            {
                spframeDatas.DeleteArrayElementAtIndex(spSelectedIdx.intValue);
                if (spframeDatas.arraySize <= spSelectedIdx.intValue)
                {
                    spSelectedIdx.intValue--;
                }
            }
            newPos.xMin += 50;
            newPos.width = 60;

            float nextPos = newPos.yMax;

            int max = spframeDatas.arraySize;
            if (max > 0)
            {
                int idx = 0;
                newPos.xMin = position.xMin;
                newPos.yMin = position.yMin + 145;
                newPos.width = FRAMEQUAD_WIDTH;
                newPos.height = FRAMEQUAD_HEIGHT;

                int mouseOverIDx = -1;

                if (currentEvent.type == EventType.DragUpdated || currentEvent.isMouse)
                    mousePosition = currentEvent.mousePosition;

                for (; idx < max; idx++)
                {
                    SerializedProperty item = spframeDatas.GetArrayElementAtIndex(idx);

                    if (newPos.Contains(mousePosition))
                        mouseOverIDx = idx;

                    DrawBox(idx, property, item, newPos, spSelectedIdx.intValue == idx);

                    newPos.xMin += FRAMEQUAD_WIDTH + FRAMEQUAD_WIDTH_SEPARATION;
                    newPos.width = FRAMEQUAD_WIDTH;
                    if (newPos.xMin + FRAMEQUAD_WIDTH + FRAMEQUAD_WIDTH_SEPARATION > position.xMin + position.width)
                    {
                        newPos.xMin = position.xMin;
                        newPos.yMin += FRAMEQUAD_HEIGHT + FRAMEQUAD_HEIGHT_SEPARATION;
                        newPos.width = FRAMEQUAD_WIDTH;
                        newPos.height = FRAMEQUAD_HEIGHT;
                        frameQuadRows++;
                    }
                }
                if (mouseOverIDx != -1)
                {
                    Repaint();

                    switch (currentEvent.type)
                    {

                        case EventType.MouseDown:
                            DragAndDrop.PrepareStartDrag();// reset data
                            DragAndDrop.paths = null;
                            DragAndDrop.objectReferences = new UnityEngine.Object[0];
                            CustomDragData dragData = new CustomDragData();
                            dragData.originalIndex = mouseOverIDx;
                            dragData.animationIdx = animationIdx;
                            DragAndDrop.SetGenericData("dragKeyframe", dragData);
                            property.FindPropertyRelative("selectedIndex").intValue = mouseOverIDx;
                            serializedObject.ApplyModifiedProperties();
                            currentEvent.Use();
                            break;
                        case EventType.DragPerform:
                            if (isDragginKeyframe)
                            {
                                DragAndDrop.AcceptDrag();
                                CustomDragData receivedDragData = DragAndDrop.GetGenericData("dragKeyframe") as CustomDragData;
                                if (receivedDragData != null && receivedDragData.animationIdx == animationIdx)
                                {
                                    // changes data between keyframes
                                    SpriteAnimationFrameData tmp = new SpriteAnimationFrameData();
                                    SerializedProperty kfFrom = spframeDatas.GetArrayElementAtIndex(receivedDragData.originalIndex);
                                    tmp.eventEnabled = kfFrom.FindPropertyRelative("eventEnabled").boolValue;
                                    tmp.eventName = kfFrom.FindPropertyRelative("eventName").stringValue;
                                    tmp.sprite = kfFrom.FindPropertyRelative("sprite").objectReferenceValue as Sprite;
                                    tmp.time = kfFrom.FindPropertyRelative("time").floatValue;

                                    SerializedProperty kfTo = spframeDatas.GetArrayElementAtIndex(mouseOverIDx);
                                    kfFrom.FindPropertyRelative("eventEnabled").boolValue = kfTo.FindPropertyRelative("eventEnabled").boolValue;
                                    kfFrom.FindPropertyRelative("eventName").stringValue = kfTo.FindPropertyRelative("eventName").stringValue;
                                    kfFrom.FindPropertyRelative("sprite").objectReferenceValue = kfTo.FindPropertyRelative("sprite").objectReferenceValue;
                                    kfFrom.FindPropertyRelative("time").floatValue = kfTo.FindPropertyRelative("time").floatValue;

                                    kfTo.FindPropertyRelative("eventEnabled").boolValue = tmp.eventEnabled;
                                    kfTo.FindPropertyRelative("eventName").stringValue = tmp.eventName;
                                    kfTo.FindPropertyRelative("sprite").objectReferenceValue = tmp.sprite;
                                    kfTo.FindPropertyRelative("time").floatValue = tmp.time;

                                    serializedObject.ApplyModifiedProperties();
                                }
                                isDragginKeyframe = false;
                                currentEvent.Use();
                                DragAndDrop.PrepareStartDrag();// reset data
                            }
                            break;
                        case EventType.DragUpdated:
                            if (isDragginKeyframe)
                            {
                                CustomDragData receivedDragData = DragAndDrop.GetGenericData("dragKeyframe") as CustomDragData;
                                if (receivedDragData != null)
                                {
                                    if (receivedDragData.animationIdx == animationIdx && receivedDragData.originalIndex != mouseOverIDx)
                                    {
                                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                                        currentEvent.Use();
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (currentEvent.type == EventType.DragUpdated && isDragginKeyframe)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    }
                }

                //                if ( playMode && spframeDatas.arraySize > 0 && spSelectedIdx.intValue > -1)
                //                {
                //                    float elapsed = EditorPrefs.GetFloat ( string.Format ( "Animx_ElapsedFrameTime_{0}_{1}", animationIdx , spName.stringValue ) , 0f );
                //
                //                    if ( EditorApplication.timeSinceStartup > elapsed )
                //                    {
                //                        Repaint();
                //
                //                        spSelectedIdx.intValue = spSelectedIdx.intValue >= spframeDatas.arraySize ? 0 : spSelectedIdx.intValue++;
                //
                //                        var newItem = spframeDatas.GetArrayElementAtIndex (spSelectedIdx.intValue);
                //                        var time = newItem.FindPropertyRelative ( "time" ).floatValue;
                //                        var speed = spSpeedRatio.floatValue;
                //                        EditorPrefs.SetFloat ( string.Format ( "Animx_ElapsedFrameTime_{0}_{1}", animationIdx , spName.stringValue ) , (float)(EditorApplication.timeSinceStartup + (time * speed)) );
                //
                //                        serializedObject.ApplyModifiedProperties();
                //                        currentEvent.Use();
                //                    }
                //                }

                nextPos = newPos.yMax + 15f;
                newPos.xMin = position.xMin;
                newPos.xMax = position.xMax;
                newPos.yMin = nextPos;
                newPos.height = 16;

                SerializedProperty selectedItem = spframeDatas.GetArrayElementAtIndex(spSelectedIdx.intValue);

                newPos.width = 100;
                EditorGUI.LabelField(newPos, new GUIContent("Sprite:"));
                newPos.xMin = newPos.xMax + 20;
                newPos.width = 200;
                EditorGUI.PropertyField(newPos, selectedItem.FindPropertyRelative("sprite"), new GUIContent(string.Empty));
                newPos.xMin = position.xMin;
                newPos.yMin += 22f;
                newPos.height = 20;
                newPos.width = 100;
                EditorGUI.LabelField(newPos, new GUIContent("Time:"));
                newPos.xMin = newPos.xMax + 20;
                newPos.width = 200;
                EditorGUI.PropertyField(newPos, selectedItem.FindPropertyRelative("time"), new GUIContent(string.Empty));
                newPos.xMin = position.xMin;
                newPos.yMin += 22f;
                newPos.height = 20;
                newPos.width = 100;
                SerializedProperty spEventEnabled = selectedItem.FindPropertyRelative("eventEnabled");
                spEventEnabled.boolValue = EditorGUI.Toggle(newPos, spEventEnabled.boolValue, GUI.skin.button);
                EditorGUI.LabelField(newPos, new GUIContent("Event: "));

                SerializedProperty spriteProperty = selectedItem.FindPropertyRelative("sprite");
                newPos.xMin = newPos.xMax + 20;
                newPos.width = 200;
                EditorGUI.PropertyField(newPos, selectedItem.FindPropertyRelative("eventName"), new GUIContent(string.Empty));

                if (Event.current.type == EventType.Repaint)
                {
                    if (spriteProperty.objectReferenceValue != null)
                    {
                        newPos = new Rect(position.xMin, newPos.yMax + 15, position.width, 80);

                        Sprite spritePreview = spriteProperty.objectReferenceValue as Sprite;

                        Texture t = spritePreview.texture;
                        Rect tr = spritePreview.rect;
                        Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                        float spriteW = newPos.height / (tr.height / tr.width);
                        newPos.xMin = (newPos.width / 2) - (spriteW / 2);
                        newPos.width = spriteW;

                        GUI.DrawTextureWithTexCoords(newPos, t, r);
                    }
                }
            }
        }

        protected void DrawBox(int idx, SerializedProperty property, SerializedProperty item, Rect pos, bool selected)
        {
            SerializedProperty spriteProperty = item.FindPropertyRelative("sprite");
            Color old = GUI.backgroundColor;
            Color selectedColor = (spriteProperty.objectReferenceValue != null) ? Color.cyan : Color.red;
            Color unselectedColor = (spriteProperty.objectReferenceValue != null) ? Color.white : Color.red;
            GUI.backgroundColor = selected ? selectedColor : unselectedColor;
            GUI.Box(pos, string.Empty);
            GUI.backgroundColor = old;
            GUI.Label(pos, new GUIContent(idx.ToString()));
        }
    }
}