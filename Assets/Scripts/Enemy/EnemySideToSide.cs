using UnityEngine;
using System.Collections;



public class EnemySideToSide : MonoBehaviour
{

    private Rigidbody2D _rb;

    public float HorizontalDirection;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HorizontalDirection = 1;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector2(transform.position.x + HorizontalDirection * speed * Time.deltaTime, transform.position.y);
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
