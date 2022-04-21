using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelStatus
{
    UnDetermined,
    Started,
    Success,
    Fail
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UIManager uiManager;
    public PlayerController playerController;

    private int _remainedPowerUps = 5;
    public int gainedCoins = 0;

    public LevelStatus levelStatus;

    private void Awake() => instance = this;

    private void Start()
    {
        if (PlayerPrefs.GetInt("myLevelProgression") > SceneManager.sceneCount)
            PlayerPrefs.SetInt("myLevelProgression", 0);
        
        if (SceneManager.GetActiveScene().buildIndex != PlayerPrefs.GetInt("myLevelProgression") )
            SceneManager.LoadScene(PlayerPrefs.GetInt("myLevelProgression"));

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiManager = gameObject.GetComponent<UIManager>();
        uiManager.UpdateGoldText();

        Actions.OnLevelSuccesful += SaveProgression;
        Actions.OnLevelSuccesful += FinishSuccesfuly;
    }

    private void Update()
    {
        if (levelStatus == LevelStatus.Success)
            uiManager.DragPanelToScreen(uiManager.continuePanelRef);
        else if(levelStatus == LevelStatus.Fail)
            uiManager.DragPanelToScreen(uiManager.retryPanelRef);
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
        PlayerPrefs.SetInt("myGold", PlayerPrefs.GetInt("myGold") + gainedCoins);
        PlayerPrefs.SetInt("myLevelProgression", PlayerPrefs.GetInt("myLevelProgression") + 1);
    }

    public void CalculateGainedCoin()
    {
        gainedCoins = Mathf.RoundToInt(playerController.playerPlatform.transform.localScale.y * 10);
    }

    private void FinishSuccesfuly()
    {
        levelStatus = LevelStatus.Success;
    }

    public void PlayGatherParticle(Vector3 pos)
    {
        playerController.gatherParticle.gameObject.transform.position = pos;
        playerController.gatherParticle.Clear();
        playerController.gatherParticle.Play();
    }

    public void StartLevel()
    {
        uiManager.tapToStartButton.gameObject.SetActive(false);
        playerController.animator.SetTrigger("GameStarted");
        uiManager.StartAnimations();
        levelStatus = LevelStatus.Started;
    }
    public void ContiuneLevel()
    {
        int sceneBuildIndex = PlayerPrefs.GetInt("myLevelProgression") >= 2 ? 0 : PlayerPrefs.GetInt("myLevelProgression");
        PlayerPrefs.SetInt("myLevelProgression", sceneBuildIndex);
        print(sceneBuildIndex);
        SceneManager.LoadScene(sceneBuildIndex);
    }
    public void RetryLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

}
