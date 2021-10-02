using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float speed;
    public Vector2 patrolStartPos;
    public Vector2 patrolEndPos;
    public LayerMask layersThatBlockVision;

    private bool isComingBackToStart = false;
    private Vector2 lastSeenPlayerPos;
    private bool hasSeenPlayer = false;
    private GameObject player;

    public void OnDrawGizmos() {
        #if UNITY_EDITOR
        Utils.DrawCrossOnPoint(
            transform.position,
            1,
            Color.green,
            0.25f
            );
        Debug.DrawLine((Vector2)transform.position + patrolStartPos, (Vector2)transform.position + patrolEndPos, Color.green);
        Utils.DrawCrossOnPoint(
            (Vector2)transform.position + patrolEndPos,
            1,
            Color.green,
            0.25f
            );
        #endif
    }

    // Start is called before the first frame update
    void Start()
    {
        patrolStartPos = transform.position;
        patrolEndPos = (Vector2)transform.position + patrolEndPos;
        player = GameObject.FindWithTag("Player");
        InvokeRepeating("CheckSightToPlayer", 1f, .25f);
    }

    void CheckSightToPlayer()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, layersThatBlockVision);
        Debug.DrawLine(transform.position, player.transform.position, Color.green, .25f);
        if (hit.collider.gameObject.tag == "Player") {
            hasSeenPlayer = true;
            lastSeenPlayerPos = hit.point;
        }
    }

    void Move()
    {
        Vector2 target = hasSeenPlayer ? lastSeenPlayerPos : isComingBackToStart ? patrolStartPos : patrolEndPos;
        Vector2 newPos = Vector2.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
        if (Vector2.Distance(newPos, isComingBackToStart ? patrolStartPos : patrolEndPos) < 8) {
            isComingBackToStart = !isComingBackToStart;
        } else {
            transform.position = newPos;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
}
