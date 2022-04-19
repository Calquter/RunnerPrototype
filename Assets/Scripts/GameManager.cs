using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UIManager uiManager;
    public PlayerController playerController;

    private int _remainedPowerUps;
    [HideInInspector] public bool isLevelDone;
    private void Awake() => instance = this;

    private void Start()
    {
        _remainedPowerUps = 5;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = gameObject.GetComponent<UIManager>();

        Actions.OnLevelSuccesful += GiveCoinToPlayer;
        Actions.OnLevelSuccesful += SaveProgression;
        Actions.OnLevelSuccesful += FinishSuccesfuly;
    }
    public void UsePowerUp()
    {
        if (_remainedPowerUps > 0)
        {
            _remainedPowerUps--;
            StartCoroutine(PowerUp());
            uiManager.powerUpButton.interactable = false;
        }
    }

    private IEnumerator PowerUp()
    {
        playerController.playerSpeed *= 2;
        yield return new WaitForSeconds(5f);
        playerController.playerSpeed /= 2;

        uiManager.powerUpButton.interactable = _remainedPowerUps > 0 ? true : false;
    }

    private void SaveProgression()
    {

    }

    private void GiveCoinToPlayer()
    {
        int coinAmount = Mathf.RoundToInt(playerController.gameObject.transform.localScale.y * 10);
    }

    private void FinishSuccesfuly()
    {
        isLevelDone = true;
        playerController.animator.SetTrigger("Victory");
    }
}
