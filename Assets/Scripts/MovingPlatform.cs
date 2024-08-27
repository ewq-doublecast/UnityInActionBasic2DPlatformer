using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector3 _finishPosition = Vector3.zero;

    [SerializeField]
    private float _speed = 0.5f;

    private Vector3 _startPosition;
    private float _trackPercent = 0;
    private int _direction = 1;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        _trackPercent += _direction * _speed * Time.deltaTime;

        float x = (_finishPosition.x - _startPosition.x) * _trackPercent + _startPosition.x;
        float y = (_finishPosition.y - _startPosition.y) * _trackPercent + _startPosition.y;

        transform.position = new Vector3(x, y, _startPosition.z);

        if (_direction == 1 && _trackPercent > 0.9f || _direction == -1 && _trackPercent < 0.1f)
        {
            _direction *= -1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _finishPosition);
    }
}
