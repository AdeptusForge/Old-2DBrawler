using UnityEngine;
using System.Collections;

public static class RotateToTarget
{

    public static Quaternion SmoothRotation(Vector3 targetPos, Transform myTransform, float rotateSpeed, float error = 0f)
    {
        Quaternion newRotation = new Quaternion();

        Vector2 direction = (targetPos - myTransform.position);

        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f + error;

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        newRotation = Quaternion.Slerp(myTransform.rotation, q, Time.deltaTime * rotateSpeed);

        return newRotation;
    }
}