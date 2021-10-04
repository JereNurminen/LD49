using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;

    public float moveSpeed;
    public List<GameObject> spells;

    public Vector2 wandTipOnSprite;
    public int rayCount;
    public LayerMask wallLayers;

    private Vector2 mousePos;
    private Vector2 wandTipPosition;
    private BoxCollider2D boxCollider;
    private Vector2 movementInput;
    private LineRenderer targetingLine;
    private Animator animator;
    private Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        targetingLine = GetComponentInChildren<LineRenderer>();
        animator = GetComponent<Animator>();
        Cursor.visible = false;
    }

    public void OnDrawGizmos() {
        #if UNITY_EDITOR
        Utils.DrawCrossOnPoint(
            (Vector2)transform.position + wandTipOnSprite,
            1,
            Color.red,
            0.25f
            );
        #endif
    }

    public void Die() {
        animator.SetTrigger("Death");
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void CastNextSpell()
    {
        GameObject nextSpell = spells[Random.Range(0, spells.Count)];
        GameObject newSpell = Instantiate(nextSpell, wandTipPosition, Quaternion.identity);
        newSpell.GetComponent<IProjectileSpell>().target = mousePos;
    }

    public void OnFire(InputValue value)
    {
        Debug.DrawLine(wandTipPosition, mousePos, Color.red, 1f);
        CastNextSpell();
    }

    void FixedUpdate() {
        lastPos = transform.position;
        Utils.Move(gameObject, moveSpeed, wallLayers, movementInput);
    }

    // Update is called once per frame
    void Update()
    {
        if ( lastPos != (Vector2)transform.position ) {
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }

        mousePos = mainCamera.ScreenToWorldPoint((Vector2)Mouse.current.position.ReadValue());

        if ( mousePos.x < transform.position.x) {
             transform.localScale = new Vector2(-1, 1); 
             wandTipPosition.x = transform.position.x - wandTipOnSprite.x;
        } else {
             transform.localScale = new Vector2(1, 1); 
             wandTipPosition.x = transform.position.x + wandTipOnSprite.x;
        }
        wandTipPosition.y = transform.position.y + wandTipOnSprite.y;
        Vector3[] linePoints = new Vector3[] { wandTipPosition, mousePos };
        targetingLine.SetPositions(linePoints);
    }
}
