using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    [SerializeField] private GameObject _playerSkin;
    public GameObject playerPlatform;
    public Animator animator;
    public ParticleSystem gatherParticle;

    private Rigidbody _rigidBody;
    private Material _playerPlatformMat;
    private Touch _touch;

    private Vector3 _newPlatformPos;
    private Vector3 _newPlatformScale;
    private Vector3 _newSkinPos;

    [HideInInspector] public bool isTurning;

    private Vector3 _velocity;
    private float _velocityX;
    [HideInInspector] public float newDirection = 0;

    

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
            if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);

                if (_touch.phase == TouchPhase.Moved)
                {
                    if (_touch.deltaPosition.x > 0.01f)
                        _velocityX = 1;
                    else if (_touch.deltaPosition.x < -0.01f)
                        _velocityX = -1;
                }
                else
                    _velocityX = 0;
            }
            else
                _velocityX = 0;
#endif


        }
        else
            _velocityX = 0;
    }

    private void Move()
    {
        if (GameManager.instance.levelStatus == LevelStatus.UnDetermined)
            return;

        if (!playerPlatform.activeSelf || GameManager.instance.levelStatus == LevelStatus.Success)
        {
            _rigidBody.velocity = Vector3.zero;
            return;
        }

        _velocity = transform.forward + transform.right * _velocityX;

        _rigidBody.velocity = _velocity.normalized * playerSpeed;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, newDirection, 0), Time.deltaTime * 3.5f);
    }

    private void ScaleControl()
    {
        if (_newPlatformScale.y <= 0)
        {
            playerPlatform.SetActive(false);
            _newPlatformScale = Vector3.zero;
            _newSkinPos = new Vector3(0, -.5f, 0);
            animator.SetTrigger("Defeat");
            GameManager.instance.levelStatus = LevelStatus.Fail;
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
    public IEnumerator PermissonToInputs()
    {
        isTurning = true;
        yield return new WaitForSeconds(1f);
        isTurning = false;
    }
}
