using UnityEngine;
using System.Collections;

public class Pivot : CreatureMovement {

    public void FlipFacing()
    {
        transform.localScale = new Vector3(-transform.localScale.x, 1f, 1f);
    }
}
