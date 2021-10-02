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

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void OnDrawGizmos() {
        Utils.DrawCrossOnPoint(
            (Vector2)transform.position + wandTipOnSprite,
            1,
            Color.red,
            0.25f
            );
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

    void Move(Vector2 move)
    {
        move.Normalize();
        Vector2 normalized = move * moveSpeed * Time.deltaTime;
        float horizontal = normalized.x;
        float vertical = normalized.y;

        List<RaycastHit2D> horHits;
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

        List<RaycastHit2D> verHits;
        if (vertical > 0) {
            verHits = Utils.LinecastAlongLine(
                new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.max.y),
                new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.max.y),
                Vector2.up,
                Mathf.Abs(horizontal),
                rayCount,
                wallLayers,
                Utils.Axis.Vertical,
                Color.magenta,
                1
            );
        } else if (vertical < 0) {
            verHits = Utils.LinecastAlongLine(
                new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y),
                new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y),
                Vector2.down,
                Mathf.Abs(horizontal),
                rayCount,
                wallLayers,
                Utils.Axis.Vertical,
                Color.magenta,
                1
            );
        }

        transform.position = new Vector2(transform.position.x + horizontal, transform.position.y + vertical);
    }

    void FixedUpdate() {
        Move(movementInput);
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = mainCamera.ScreenToWorldPoint((Vector2)Mouse.current.position.ReadValue());
        wandTipPosition = (Vector2)transform.position + wandTipOnSprite;
    }
}
