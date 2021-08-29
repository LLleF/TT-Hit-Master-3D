using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private PlayerMoving _moving;

    [SerializeField] private Camera _gameCamera;

    [SerializeField] private Text _tapToMoveText;

    [SerializeField] private GameObject _gunBarrel;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _winGameMenu;

    [SerializeField] private ParticleSystem _shoot;

    private static List<int> _enemyOnPlatforms = new List<int> { 3, 5, 6, 1 };
    private List<int> _copyEnemyOnPlatforms = new List<int> { 3, 5, 6, 1 };

    private Vector3 _clickCoordinate;

    private static bool _modeShoot;


    private void Start()
    {
        _enemyOnPlatforms = _copyEnemyOnPlatforms;
    }

    void Update()
    {
        Shoot();
        CheckMinCountEnemy();
    }


    public static void EnableModeShoot()
    {
        _modeShoot = true;
    }

    public static bool GetListCountEnemyOnPlatforms()
    {
        return _enemyOnPlatforms.Count > 0;
    }

    public static void MinusEnemyCount()
    {
        _enemyOnPlatforms[0]--;
    }




    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _modeShoot)
        {
            Ray rayForClick = _gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(rayForClick, out hit, 19f) && hit.collider.gameObject.name.Contains("Enemy"))
            {
                _shoot.Play();
                _clickCoordinate = hit.point;
                SpawBullet();
                LookAtTarget();
            }
        }
    }

    private void SpawBullet()
    {
        GameObject bullet = Instantiate(_bullet, _gunBarrel.transform.position, Quaternion.identity);
        Ray rayForShoot = new Ray(_gunBarrel.transform.position, _clickCoordinate - _gunBarrel.transform.position);
        bullet.GetComponent<Bullet>().SetHitDirection(rayForShoot.direction);
    }

    private void LookAtTarget()
    {
        if (_clickCoordinate != new Vector3(0, 0, 0))
        {
            _moving.NewPointForLookAt(_clickCoordinate);
            Quaternion rotation = Quaternion.LookRotation(_clickCoordinate - transform.position, Vector3.up);
            transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
            _clickCoordinate = new Vector3(0, 0, 0);
        }
    }




    private void CheckMinCountEnemy()
    {
        if (_enemyOnPlatforms.Count > 0 && _enemyOnPlatforms[0] <= 0)
        {
            _enemyOnPlatforms.RemoveAt(0);
            _modeShoot = false;
            _tapToMoveText.gameObject.SetActive(true);
        }
        else if (_enemyOnPlatforms.Count == 0)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        _winGameMenu.SetActive(true);
        _tapToMoveText.gameObject.SetActive(false);
    }

}
