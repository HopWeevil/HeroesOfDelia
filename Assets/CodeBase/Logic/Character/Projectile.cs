using CodeBase.Logic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private LayerMask _targetLayer;
    private float _damage;
    private Vector3 _direction;

    public void Initialize(float damage, Vector3 direction, LayerMask targetLayer)
    {
        _damage = damage;
        _targetLayer = targetLayer;
        _direction = direction;
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if ((_targetLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            other.transform.GetComponentInChildren<IHealth>()?.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}
