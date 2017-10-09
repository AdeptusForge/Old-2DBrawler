using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
}
//    public float moveSpeed;
//    public float jumpHeight;
//    private float moveVelocity;
//    public float dashDistance;

//    void FixedUpdate()
//    {
//        moveVelocity = 0f;

//        //basic movement
//        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftShift))
//        {
//            moveVelocity = moveSpeed;
//        }

//        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftShift))
//        {
//            moveVelocity = -moveSpeed;
//        }

//        playerBody.velocity = new Vector2(moveVelocity, playerBody.velocity.y);

//        //animation flip to the direction of movement. may need to be tweaked when knockback is included.
//        anim.SetFloat("Speed", Mathf.Abs(playerBody.velocity.x));


//        //calls the dash function
//        if (Input.GetKey(KeyCode.LeftShift))
//        {
//            if (Input.GetKeyDown(KeyCode.D))
//            {
//                Dash("DashForward", transform.position.x + dashDistance, transform.position.y);
//            }
//            else if (Input.GetKeyDown(KeyCode.A))
//            {
//                Dash("DashBackward", transform.position.x + -dashDistance, transform.position.y);
//            }
//            else if (Input.GetKeyDown(KeyCode.W))
//            {
//                Dash("DashUpward", transform.position.x, transform.position.y + dashDistance);
//            }
//            else if (Input.GetKeyDown(KeyCode.S))
//            {
//                Dash("DashDownward", transform.position.x, transform.position.y + -dashDistance);
//            }
//        }

//        if (playerBody.velocity.x > 0)
//        {
//            transform.localScale = new Vector3(1f, 1f, 1f);
//        }
//        else if (playerBody.velocity.x < 0)
//        {
//            transform.localScale = new Vector3(-1f, 1f, 1f);
//        }

//    }//end of FixedUpdate

//    //dash
//    public void Dash(string animDirection, float xDistance, float yDistance)
//    {
//        anim.SetTrigger(animDirection);
//        playerBody.velocity = Vector2.zero;
//        transform.position = new Vector3(xDistance, yDistance, transform.position.z);
//    }


    
//    //fallthrough platforms
//    public void Fallthrough(bool ignore)
//    {
//        Physics2D.IgnoreLayerCollision(9, 10, ignore);
//    }



//}
