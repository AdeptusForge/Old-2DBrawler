#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Physics 2D/Triangle 2D")]

[RequireComponent(typeof(EdgeCollider2D))]
public class TriangleCollider2D : MonoBehaviour
{

    [Range(1, 89)]
    public float height = 1f;

    [Range(1,50)]
    public float length = 1f;

    [HideInInspector]
    public int rotation = 0;

    Vector2 origin, center;

    public Vector2[] getPoints(Vector2 off)
    {
            List<Vector2> pts = new List<Vector2>();
            origin = transform.localPosition;
            center = origin + off;

            float ang = rotation;

            pts.Add(new Vector2(center.x + length, center.y));
            pts.Add(new Vector2(0, 0));
            pts.Add(new Vector2(0, height));
            pts.Add(pts[0]);
            return pts.ToArray();
    }

}
#endif