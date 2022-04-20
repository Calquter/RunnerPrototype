using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    public float playerSpeed;
    [SerializeField] private GameObject _playerSkin;
    public ParticleSystem gatherParticle;
    private Material _playerPlatformMat;
    public GameObject playerPlatform;
    public Animator animator;

    private Vector3 _newPlatformPos;
    [SerializeField] private Vector3 _newPlatformScale;
    private Vector3 _newSkinPos;

    [SerializeField] private bool _isChanging;
    public bool isTurning;

    private Vector3 _velocity;
    private float _velocityX;
    public float newDirection = 0;

    private Touch _touch;

    private float _playerCurrentPosRef;
    private float _platformCurrentScaleRef;
    private float _platformCurrentPosRef;
    private float _platformMaterialTilingRef;

    private void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        animator = _playerSkin.GetComponent<Animator>();
        

        _newPlatformPos = playerPlatform.transform.localPosition;
        _newPlatformScale = playerPlatform.transform.localScale;
        _newSkinPos = _playerSkin.transform.localPosition;

        _playerPlatformMat = playerPlatform.GetComponent<MeshRenderer>().material;
        _playerPlatformMat.mainTextureScale = new Vector2(1, 1);
    } 

    private void Update()
    {
        GetInputs();
        Move();

        ChangePlayerTransforms();
    }

    private void GetInputs()
    {
        if (!isTurning)
        {

#if UNITY_EDITOR
            _velocityX = Input.GetAxisRaw("Horizontal");

#else
            if (_touch.tapCount > 0)
                {
                    _velocity.x = _touch.deltaPosition.x;
                }
#endif

        }
        else
            _velocityX = 0;

    }


    private void Move()
    {
        _velocity = transform.forward + transform.right * _velocityX;

        if (!GameManager.instance.isGameStarted)
            return;

        if (!playerPlatform.activeSelf || GameManager.instance.isLevelSucces)
        {
            _rigidBody.velocity = Vector3.zero;
            return;
        }

        _rigidBody.velocity = _velocity.normalized * playerSpeed;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, newDirection, 0), Time.deltaTime * 3.5f);
    }

    private void ScaleControl()
    {
        if (_newPlatformScale.y <= 0)
        {
            playerPlatform.SetActive(false);
            _newPlatformScale = new Vector3(0, 0, 0);
            _newSkinPos = new Vector3(0, -.5f, 0);
            animator.SetTrigger("Defeat");
            GameManager.instance.isLevelFailed = true;
        }
    }

    public void CalculateBuffEffects(float amount)
    {
        _newPlatformScale += Vector3.up * amount;
        _newPlatformPos += Vector3.up * (amount / 2);
        _newSkinPos += Vector3.up * amount;

        if (amount < 0)
            ScaleControl();
    }

    private void ChangePlayerTransforms()
    {
        playerPlatform.transform.localScale = new Vector3(playerPlatform.transform.localScale.x, Mathf.SmoothDamp(playerPlatform.transform.localScale.y, _newPlatformScale.y,
                ref _platformCurrentScaleRef, .2f), playerPlatform.transform.localScale.z);

        playerPlatform.transform.localPosition = new Vector3(playerPlatform.transform.localPosition.x, Mathf.SmoothDamp(playerPlatform.transform.localPosition.y, _newPlatformPos.y,
            ref _platformCurrentPosRef, .2f), playerPlatform.transform.localPosition.z);

        _playerSkin.transform.localPosition = new Vector3(_playerSkin.transform.localPosition.x, Mathf.SmoothDamp(_playerSkin.transform.localPosition.y, _newSkinPos.y,
            ref _playerCurrentPosRef, .2f), _playerSkin.transform.localPosition.z);

        _playerPlatformMat.mainTextureScale = new Vector2(_playerPlatformMat.mainTextureScale.y, Mathf.SmoothDamp(_playerPlatformMat.mainTextureScale.y, playerPlatform.transform.localScale.y,
            ref _platformMaterialTilingRef, .1f));
    }

    private IEnumerator PermissonToChange(float time)
    {
        _isChanging = true;
        yield return new WaitForSeconds(time);
        _isChanging = false;
    }
    public IEnumerator PermissonToInputs()
    {
        isTurning = true;
        yield return new WaitForSeconds(1f);
        isTurning = false;
    }
}
