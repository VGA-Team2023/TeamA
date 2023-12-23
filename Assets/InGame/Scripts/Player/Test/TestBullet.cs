//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;
    [TagName]
    [SerializeField] private string _groundTag;
    [SerializeField] private int _damage;
    [SerializeField] private GameObject _effect;

    private Vector3 _preTransform;
    private Vector2 _playerForward;

    private void Start()
    {
        _preTransform = transform.position;
    }

    private void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.left, _rb.velocity.normalized * -1);
        _preTransform = transform.position;
    }

    public void SetShotDirection(Vector2 direction)
    {
        _playerForward = direction;
        Move();
    }

    private void Move()
    {
        _rb.AddForce(_playerForward * _speed, ForceMode2D.Force);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_groundTag))
        {
            Instantiate(_effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (collision.TryGetComponent<IReceiveWater>(out var damage))
        {
            Instantiate(_effect, transform);
            damage.ReceiveWater();
            Destroy(gameObject);
        }

        Destroy(gameObject, 2f);
    }
}
