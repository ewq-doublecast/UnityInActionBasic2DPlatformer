using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string SpeedAnimationKey = "speed";

    [SerializeField]
    private float _speed = 250.0f;

    [SerializeField]
    private float _jumpForce = 12.0f;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;
    private Animator _animator;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float deltaX = Input.GetAxis(Horizontal) * _speed * Time.deltaTime;
        Vector2 movement = new Vector2 (deltaX, _rigidbody.velocity.y);
        _rigidbody.velocity = movement;

        _animator.SetFloat(SpeedAnimationKey, Mathf.Abs(deltaX));

        if (Mathf.Approximately(deltaX, 0) == false)
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }

        Vector3 maxBounds = _collider.bounds.max;
        Vector3 minBounds = _collider.bounds.min;

        Vector2 corner1 = new Vector2(maxBounds.x, minBounds.y - 0.1f);
        Vector2 corner2 = new Vector2(minBounds.x, minBounds.y - 0.2f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool isGrounded = false;

        if (hit != null)
        {
            isGrounded = true;
        }

        _rigidbody.gravityScale = isGrounded && deltaX == 0 ? 0 : 1;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        MovingPlatform movingPlatform = null;

        if (hit != null)
        {
            movingPlatform = hit.GetComponent<MovingPlatform>();
        }

        Vector3 platformScale = Vector3.one;

        if (movingPlatform != null)
        {
            platformScale = movingPlatform.transform.localScale;
        }

        if (deltaX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / platformScale.x, 1 / platformScale.y, 1);
        }

        if (movingPlatform != null)
        {
            transform.parent = movingPlatform.transform;
        }
        else
        {
            transform.parent = null;
        }
    }
}
