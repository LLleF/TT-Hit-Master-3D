using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IHittable
{
    [SerializeField] private  int _health = 2;

    [SerializeField] private Text _healthText;


    private void Start()
    {
        _healthText.text = _health.ToString();
    }


    public void ApllayDamage(int value)
    {
        _health = _health - value;
        _healthText.text = _health.ToString();
        Death();
    }

    private void Death()
    {
        if (_health <= 0)
        {           
            PlayerShoot.MinusEnemyCount();
            _healthText.gameObject.SetActive(false);
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().enabled = false;
            StartCoroutine(DeleteBullet());
        }
    }

    private IEnumerator DeleteBullet()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

}
