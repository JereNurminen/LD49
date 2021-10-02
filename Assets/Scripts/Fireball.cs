using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, IProjectileSpell
{
    public Vector2 target { get; set; }
    public LayerMask layers;

    [SerializeField]
    private float _speed;
    public float speed { get { return _speed; } set { _speed = value; } }

    Vector2 direction;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        direction = (target - (Vector2)transform.position).normalized;
        animator = gameObject.GetComponent<Animator>();
    }

    public void Hit()
    {
        direction = Vector2.zero;
        animator.SetTrigger("Hit");
    }

    public void CheckForHit(Vector2 newPos) {

    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Move()
    {
        Vector2 newPos = (Vector2)transform.position + direction * speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, newPos, layers);
        if (hit.collider != null) {
            newPos = hit.point;
            Hit();
        }
        transform.position = newPos;
    }

    public void AnimationDone() {
        Kill();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
}
