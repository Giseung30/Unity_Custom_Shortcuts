/*

 - ����Ű
 % : Ctrl or Cmd
 ^ : Ctrl
 # : Shift
 & : Alt
 _ : ����Ű�� �ʿ����� ���� ���
 
 - Ư������
 LEFT, RIGHT, UP, DOWN, F1���� F12, HOME, END, PGUP, PGDN, INS, DEL, TAB, SPACE
 
 Ex) Shift+Alt+G ����Ű�� ���� �޴��� ������� "MyMenu/Do Something #&g"�� ����ϰ�, GŰ�� ����Ϸ��� "MyMenu/Do Something _g"�� ����Ѵ�.

*/

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class Shortcuts : Editor
{
    [MenuItem("Shortcut/Group %g")]
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
            if (isRectTransform) groupObject.AddComponent<RectTransform>();
            Transform groupTransform = groupObject.transform;
            groupTransform.SetParent(parent, false);
            groupTransform.SetSiblingIndex(selections[0].GetSiblingIndex());
            groupTransform.position = position;
            Undo.RegisterCreatedObjectUndo(groupObject, groupObject.name);

            for (int i = 0, l = selections.Count; i < l; ++i)
                Undo.SetTransformParent(selections[i], groupTransform, selections[i].name);

            Selection.activeGameObject = groupObject;
        }
        DestroyImmediate(root.gameObject);
    }
    [MenuItem("Shortcut/Ungroup %#g")]
    public static void Ungroup()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        Transform[] transforms = Array.ConvertAll(gameObjects, go => go.transform);

        for (int i = 0, l = transforms.Length; i < l; ++i)
        {
            Transform parent = transforms[i].parent;
            for (int j = 0, l2 = transforms[i].childCount; j < l2; ++j)
            {
                Transform child = transforms[i].GetChild(i);
                Undo.SetTransformParent(child, parent, child.name);
                child.SetSiblingIndex(transforms[i].GetSiblingIndex());
            }
            Undo.DestroyObjectImmediate(transforms[i].gameObject);
        }
    }
}
#endif