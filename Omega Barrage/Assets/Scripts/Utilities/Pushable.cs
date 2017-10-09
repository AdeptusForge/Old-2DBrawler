using UnityEngine;
using System.Collections;

public class Pushable : MonoBehaviour {

    private Rigidbody2D myBody;
    private CreatureMovement myMover;

    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();

    }

    public void Push(float knockBackAmount, Vector3 direction, float dur)
    {
        if(GetComponent<Creature>() != null)
        {
            GetComponent<Creature>().knockBackDuration = dur;
        }
        if(GetComponent<CreatureMovement>() != null)
        {
            GetComponent<CreatureMovement>().Move(CreatureMovement.MoveType.SetVelocity, direction, knockBackAmount);
        }

    }
}
