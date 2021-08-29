using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _hitDirection;

    private Rigidbody _rb;

    private const float _bulletSpeed = 15f;

    [SerializeField] private ParticleSystem _explosion;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyBullet());
    }


    void FixedUpdate()
    {
        MoveBullet();
    }


    public void SetHitDirection(Vector3 newValue)
    {
        _hitDirection = newValue;
    }



    private void MoveBullet()
    {
        if (_hitDirection != new Vector3(0, 0, 0))
        {
            _rb.velocity = _hitDirection * _bulletSpeed;
        }
    }



    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            ExplosionPlay(collision);
            collision.gameObject.GetComponent<IHittable>().ApllayDamage(1);
            Destroy(this.gameObject);
        }
    }

    private void ExplosionPlay(Collision collision)
    {
        ParticleSystem hit = Instantiate(_explosion, collision.contacts[0].point, Quaternion.identity);
        hit.Play();
        hit.GetComponent<DeleteParticleSystem>().Delete();
    }

}
