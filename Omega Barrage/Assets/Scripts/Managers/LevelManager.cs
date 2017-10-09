using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject currentCheckPoint;

    private PlayerMovement player;

    public GameObject deathParticle;
    public float respawnDelay;

    public int pointDeathPenalty;

    // Use this for initialization
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        player = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespawnPlayer()
    {
        StartCoroutine("RespawnPlayerCo");
    }

    public IEnumerator RespawnPlayerCo()
    {
        //Instantiate(deathParticle, player.transform.position, player.transform.rotation);
        player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        player.enabled = false;
        player.GetComponent<Renderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ScoreManager.AddPoints(-pointDeathPenalty);
        Debug.Log("Player Respawn");
        yield return new WaitForSeconds(respawnDelay);
        player.transform.position = currentCheckPoint.transform.position;
        player.enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 0f;
        player.GetComponent<Renderer>().enabled = true;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
