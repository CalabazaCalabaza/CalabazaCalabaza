using UnityEngine;
using System.Collections;



public class EnemySideToSide : MonoBehaviour
{

    private Rigidbody2D _rb;
    public SpriteRenderer _spriteRenderer;
    public SpriteRenderer _spriteRenderer2;
    public float HorizontalDirection;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HorizontalDirection = 1;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector2(transform.position.x + HorizontalDirection * speed * Time.deltaTime, transform.position.y);
        if (HorizontalDirection != 0)
        {
            _spriteRenderer.flipX = HorizontalDirection > 0;
            if(_spriteRenderer2 != null) _spriteRenderer2.flipX = HorizontalDirection > 0;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (HorizontalDirection == 1)
        {
            HorizontalDirection = -1;
        }
        else if (HorizontalDirection == -1)
        {
            HorizontalDirection = 1;
        }
    }
}
