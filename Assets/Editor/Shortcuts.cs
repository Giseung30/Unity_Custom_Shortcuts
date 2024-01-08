/*

 - 조합키
 % : Ctrl or Cmd
 ^ : Ctrl
 # : Shift
 & : Alt
 _ : 조합키가 필요하지 않은 경우
 
 - 특수문자
 LEFT, RIGHT, UP, DOWN, F1부터 F12, HOME, END, PGUP, PGDN, INS, DEL, TAB, SPACE
 
 Ex) Shift+Alt+G 단축키를 가진 메뉴를 만들려면 "MyMenu/Do Something #&g"를 사용하고, G키만 사용하려면 "MyMenu/Do Something _g"를 사용한다.

*/

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class Shortcuts : Editor
{
    [MenuItem("Shortcuts/Group %g")]
    public static void Group()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        Transform root = new GameObject("Root").transform;

        Dictionary<Transform, List<Transform>> parentList = new Dictionary<Transform, List<Transform>>();
        for (int i = 0, l = gameObjects.Length; i < l; ++i)
        {
            Transform parent = gameObjects[i].transform.parent;
            if (!parent) parent = root;
            if (!parentList.ContainsKey(parent))
                parentList.Add(parent, new List<Transform>());
            parentList[parent].Add(gameObjects[i].transform);
        }

        foreach (List<Transform> selections in parentList.Values)
        {
            Transform parent = selections[0].parent;
            bool isRectTransform = false;
            if (parent != null)
                isRectTransform = parent.GetComponentInParent<Canvas>(true) != null;

            Vector3 position = default;
            for (int i = 0, l = selections.Count; i < l; ++i)
                position += selections[i].position / l;

            GameObject groupObject = new GameObject("Group");
            Undo.RegisterCreatedObjectUndo(groupObject, groupObject.name);
            if (isRectTransform) groupObject.AddComponent<RectTransform>();
            Transform groupTransform = groupObject.transform;
            Undo.SetTransformParent(groupTransform, parent, groupTransform.name);
            Undo.SetSiblingIndex(groupTransform, selections[0].GetSiblingIndex(), groupTransform.name);
            groupTransform.position = position;

            for (int i = 0, l = selections.Count; i < l; ++i)
                Undo.SetTransformParent(selections[i], groupTransform, selections[i].name);

            Selection.activeGameObject = groupObject;
        }

        DestroyImmediate(root.gameObject);
    }
    [MenuItem("Shortcuts/Ungroup %#g")]
    public static void Ungroup()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        Transform[] transforms = Array.ConvertAll(gameObjects, go => go.transform);

        for (int i = 0, l = transforms.Length; i < l; ++i)
        {
            Transform transform = transforms[i];
            if (transform.childCount > 0)
            {
                Transform parent = transform.parent;
                while (transform.childCount > 0)
                {
                    Transform child = transform.GetChild(0);
                    Undo.SetTransformParent(child, parent, child.name);
                    Undo.SetSiblingIndex(child, transform.GetSiblingIndex(), child.name);
                }
                Undo.DestroyObjectImmediate(transform.gameObject);
            }
        }
    }
}
#endif