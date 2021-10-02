using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;

    public float moveSpeed;

    public Vector2 wandTipOnSprite;
    public int rayCount;
    public LayerMask wallLayers;

    private Vector2 mousePos;
    private Vector2 wandTipPosition;
    private BoxCollider2D boxCollider;
    private Vector2 movementInput;
    private LineRenderer targetingLine;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        targetingLine = GetComponentInChildren<LineRenderer>();
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

    public void OnMove(InputValue value)
    {
        Debug.Log(value.Get<Vector2>());
        movementInput = value.Get<Vector2>();
    }

    public void OnFire(InputValue value)
    {
        Debug.Log(mousePos);
        Debug.DrawLine(wandTipPosition, mousePos, Color.red, 1f);
    }

    // TODO: Find out why all the magic numbers work 
    void Move(Vector2 move)
    {
        move.Normalize();
        Vector2 normalized = move * moveSpeed * Time.deltaTime;
        float horizontal = normalized.x;
        float vertical = normalized.y;
        Vector2 newPos = (Vector2)transform.position + normalized;

        List<RaycastHit2D> horHits = new List<RaycastHit2D>();
        if (horizontal > 0) {
            horHits = Utils.LinecastAlongLine(
                new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y),
                new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y),
                Vector2.right,
                Mathf.Abs(horizontal),
                rayCount,
                wallLayers,
                Utils.Axis.Horizontal,
                Color.magenta,
                1
            );
        } else if (horizontal < 0) {
            horHits = Utils.LinecastAlongLine(
                new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y),
                new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y),
                Vector2.left,
                Mathf.Abs(horizontal),
                rayCount,
                wallLayers,
                Utils.Axis.Horizontal,
                Color.magenta,
                1
            );
        }

        if (horHits.Count > 0) {
            Debug.Log("h hit");
            Vector2 horHitPos = horHits[0].point;
            newPos.x = horHitPos.x < transform.position.x
                ? horHitPos.x + boxCollider.bounds.extents.x
                : horHitPos.x - boxCollider.bounds.extents.x - 1;
        }

        List<RaycastHit2D> verHits = new List<RaycastHit2D>();
        if (vertical > 0) {
            verHits = Utils.LinecastAlongLine(
                new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y),
                new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y),
                Vector2.up,
                Mathf.Abs(vertical),
                rayCount,
                wallLayers,
                Utils.Axis.Vertical,
                Color.magenta,
                1,
                true
            );
        } else if (vertical < 0) {
            verHits = Utils.LinecastAlongLine(
                new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y),
                new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y),
                Vector2.down,
                Mathf.Abs(vertical),
                rayCount,
                wallLayers,
                Utils.Axis.Vertical,
                Color.magenta,
                1,
                true
            );
        }
        
        if (verHits.Count > 0) {
            Debug.Log("v hit");
            Vector2 verHitPos = verHits[0].point;
            newPos.y = verHitPos.y < transform.position.y
                ? verHitPos.y + boxCollider.bounds.extents.y + 3
                : verHitPos.y - boxCollider.bounds.extents.y + 2;
        }

        transform.position = newPos;
    }

    void FixedUpdate() {
        Move(movementInput);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint((Vector2)Mouse.current.position.ReadValue());
        wandTipPosition = (Vector2)transform.position + wandTipOnSprite;
        Vector3[] linePoints = new Vector3[] { wandTipPosition, mousePos };
        targetingLine.SetPositions(linePoints);
    }
}
