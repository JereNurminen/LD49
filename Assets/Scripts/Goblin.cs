using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public Vector2 patrolStartPos;
    public Vector2 patrolEndPos;
    public LayerMask layersThatBlockVision;
    public LayerMask wallLayers;

    private bool isComingBackToStart = false;
    private Vector2 lastSeenPlayerPos;
    private Vector2 moveTarget;
    private bool hasSeenPlayer = false;
    private bool seesPlayer = false;
    private bool isPatrolling = true;
    private GameObject player;
    private Melee melee;
    private List<RaycastHit2D> lastFrameHits = new List<RaycastHit2D>();
    private Vector2 lastPos;
    private Animator animator;

    private bool alive = true;

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
        melee = GetComponentInChildren<Melee>();
        animator = GetComponent<Animator>();
        InvokeRepeating("CheckSightToPlayer", 1f, .25f);
    }

    public void Die() {
        alive = false;
        animator.SetTrigger("Death");
    }

    void CheckSightToPlayer()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, layersThatBlockVision);
        Debug.DrawLine(transform.position, player.transform.position, Color.green, .25f);

        if (hit.collider.gameObject.tag == "Player") {
            hasSeenPlayer = true;
            seesPlayer = true;
            lastSeenPlayerPos = hit.point;
            moveTarget = lastSeenPlayerPos;
            isPatrolling = false;
        } else {
            seesPlayer = false;
        }
    }

    void Move()
    {
        Vector2 target =
            hasSeenPlayer
                ? lastSeenPlayerPos
                : isComingBackToStart
                    ? patrolStartPos
                    : patrolEndPos;

        Vector2 move = ( target - (Vector2)transform.position ).normalized;
        List<RaycastHit2D> hits = Utils.Move(gameObject, !hasSeenPlayer ? walkSpeed : runSpeed, wallLayers, move);

        if (!hasSeenPlayer && Vector2.Distance(transform.position, isComingBackToStart ? patrolStartPos : patrolEndPos) < 8) {
            isComingBackToStart = !isComingBackToStart;
        } 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive) { 
            if ( hasSeenPlayer ) {
                CheckSightToPlayer();
            }
            Move();
            if (Vector2.Distance(transform.position, player.transform.position) <= melee.meleeRange) {
                melee.Swing(player.transform.position);
            }
            
            if ( lastPos.x < transform.position.x) {
                transform.localScale = new Vector2(1, 1); 
            } else {
                transform.localScale = new Vector2(-1, 1); 
            }

            lastPos = transform.position;
        }
    }

    void Update()
    {
        if (alive) {
            if ( lastPos != (Vector2)transform.position ) {
                animator.SetBool("Walking", true);
            } else {
                animator.SetBool("Walking", false);
            }
        }
    }
}
