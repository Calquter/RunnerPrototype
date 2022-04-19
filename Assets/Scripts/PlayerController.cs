using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float _playerSpeed;
    [SerializeField] private GameObject _playerSkin;
    [SerializeField] private GameObject _playerPlatform;
    private Animator _animator;

    [SerializeField] private Vector3 _newPlatformPos;
    [SerializeField] private Vector3 _newPlatformScale;
    [SerializeField] private Vector3 _newSkinPos;

    [SerializeField] private bool _isChanging;

    private Vector3 _velocity;
    private Touch _touch;

    private float _playerCurrentPosRef;
    private float _platformCurrentScaleRef;
    private float _platformCurrentPosRef;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _animator = _playerSkin.GetComponent<Animator>();
        _velocity = transform.forward;

        _newPlatformPos = _playerPlatform.transform.localPosition;
        _newPlatformScale = _playerPlatform.transform.localScale;
        _newSkinPos = _playerSkin.transform.localPosition;
    } 

    private void Update()
    {
        GetInputs();
        Move();

        ChangePlayerTransforms();
    }


    private void GetInputs()
    {
        _velocity.x = Input.GetAxisRaw("Horizontal");

        //if (_touch.tapCount > 0)
        //{
        //    _velocity.x = _touch.deltaPosition.x;
        //}
    }


    private void Move()
    {
        if (!_playerPlatform.activeSelf)
        {
            _rigidBody.velocity = Vector3.zero;
            return;
        }

        _rigidBody.velocity = _velocity.normalized * _playerSpeed;
    }

    private void ScaleControl()
    {
        if (_newPlatformScale.y < 0)
        {
            _playerPlatform.SetActive(false);
            _newPlatformScale = new Vector3(0, 0, 0);
            _newSkinPos = new Vector3(0, -.5f, 0);
            _animator.SetTrigger("Defeat");
        }
    }

    public void CalculateBuffEffects(float amount)
    {
        _newPlatformScale += Vector3.up * amount;
        _newPlatformPos += Vector3.up * (amount / 2);
        _newSkinPos += Vector3.up * amount;

        StartCoroutine(PermissonToChange(.4f));

        if (amount < 0)
            ScaleControl();
    }

    private void ChangePlayerTransforms()
    {
        if (_isChanging)
        {
            _playerPlatform.transform.localScale = new Vector3(_playerPlatform.transform.localScale.x, Mathf.SmoothDamp(_playerPlatform.transform.localScale.y, _newPlatformScale.y, ref _platformCurrentScaleRef, .2f), _playerPlatform.transform.localScale.z);
            _playerPlatform.transform.localPosition = new Vector3(_playerPlatform.transform.localPosition.x, Mathf.SmoothDamp(_playerPlatform.transform.localPosition.y, _newPlatformPos.y, ref _platformCurrentPosRef, .2f), _playerPlatform.transform.localPosition.z);
            _playerSkin.transform.localPosition = new Vector3(_playerSkin.transform.localPosition.x, Mathf.SmoothDamp(_playerSkin.transform.localPosition.y, _newSkinPos.y, ref _playerCurrentPosRef, .2f), _playerSkin.transform.localPosition.z);
        }
    }

    private IEnumerator PermissonToChange(float time)
    {
        _isChanging = true;
        yield return new WaitForSeconds(time);
        _isChanging = false;
    }
}
