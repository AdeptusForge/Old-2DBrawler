using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

    public float moveSpeed;
    public bool moveRight;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool walled;

    public Transform edgeCheck;
    public float edgeCheckRadius;
    public LayerMask whatIsGround;
    private bool nearEdge;

    public float knockBackDuration;
	
	// Update is called once per frame
	void Update ()
    {
        walled = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);
        nearEdge = !Physics2D.OverlapCircle(edgeCheck.position, edgeCheckRadius, whatIsGround);
        if (walled || nearEdge)
        {
            moveRight = !moveRight;
        }


        if (moveRight)
        {
            transform.localScale = new Vector3(-2f, 2f, 2f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (!moveRight)
        {
            transform.localScale = new Vector3(2f, 2f, 2f);
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }    
    }
    protected void KnockBackTimer()
    {
        if (knockBackDuration > 0f)
            knockBackDuration -= Time.deltaTime;
    }
}
