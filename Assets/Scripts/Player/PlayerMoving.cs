using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoving : MonoBehaviour
{
    [SerializeField] private GameObject _winGameMenu;
    [SerializeField] private GameObject _gameCamera;

    [SerializeField] private List<GameObject> _wayPoint = new List<GameObject>();
    [SerializeField] private List<GameObject> _platforms = new List<GameObject>();

    [SerializeField] private Text _tapToMoveText;

    private Animator _playerAnimator;

    private Quaternion _rotation;

    private Vector3 _nextPointToMove;

    private bool _modeMove;



    void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _nextPointToMove = _wayPoint[0].transform.position;
    }


    void Update()
    {
        LookAtNextPoint();
        MoveToNextPoint();
        StartToMoveToNextWayPoint();
        ArrivalAtPoint();
    }



    public void NewPointForLookAt(Vector3 point)
    {
        _rotation = Quaternion.LookRotation(point - transform.position, Vector3.up);
    }




    private void MoveToNextPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, _nextPointToMove, 5 * Time.deltaTime);
    }

    private void LookAtNextPoint()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(0, _rotation.y, 0, _rotation.w), Time.deltaTime * 10);
    }



    private void StartToMoveToNextWayPoint()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _tapToMoveText.gameObject.activeSelf && !_winGameMenu.activeSelf)
        {
            TakeCamera();

            SetNextPoint();
            _modeMove = true;
            _platforms.RemoveAt(0);
            _tapToMoveText.gameObject.SetActive(false);
        }
    }

    private void TakeCamera()
    {
        Quaternion rotation = Quaternion.LookRotation(_platforms[0].transform.position - transform.position, Vector3.up);
        transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
        _gameCamera.transform.parent = transform;
    }

    private void SetNextPoint()
    {
        NewPointForMove();
        NewPointForLookAt(_nextPointToMove);

        _playerAnimator.SetBool("IsMove", true);
        _playerAnimator.SetBool("IsShoot", false);
    }

    private void NewPointForMove()
    {
        _wayPoint.RemoveAt(0);
        _nextPointToMove = _wayPoint[0].transform.position;
    }



    private void ArrivalAtPoint()
    {
        if (_modeMove && _wayPoint.Count > 0 && Vector3.Distance(transform.position, _wayPoint[0].transform.position) < 0.1f)
        {
            if (_wayPoint[0].name.Contains("Enter"))
            {
                _modeMove = false;
                NewPointForLookAt(_platforms[0].transform.position);
                _playerAnimator.SetBool("IsShoot", true);
                StartCoroutine(CameraFree());
                return;
            }
            else if (_wayPoint[0].name.Contains("Intermediate"))
            {
                SetNextPoint();
            }
            else if (_wayPoint[0].name.Contains("Start"))
            {
                return;
            }
        }
    }

    private IEnumerator CameraFree()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(2).parent = null;
        PlayerShoot.EnableModeShoot();
    }
}







