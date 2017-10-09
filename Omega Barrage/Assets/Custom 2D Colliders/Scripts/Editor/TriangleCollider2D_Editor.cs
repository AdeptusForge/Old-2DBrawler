#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(TriangleCollider2D))]
public class TriangleCollider2D_Editor : Editor {

    TriangleCollider2D sc;
    EdgeCollider2D edgeCollider;
    Vector2 off;

    void OnEnable()
    {
        sc = (TriangleCollider2D)target;

        edgeCollider = sc.GetComponent<EdgeCollider2D>();
        if (edgeCollider == null) {
            sc.gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider = sc.GetComponent<EdgeCollider2D>();
        }
        edgeCollider.points = sc.getPoints(edgeCollider.offset);
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();

        sc.rotation = EditorGUILayout.IntSlider("Rotation", sc.rotation, 0, 360);


        if (GUI.changed || !off.Equals(edgeCollider.offset))
        {
            edgeCollider.points = sc.getPoints(edgeCollider.offset);
        }

        off = edgeCollider.offset;
    }

}
#endif