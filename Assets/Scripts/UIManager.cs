using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Button powerUpButton;
    public Button tapToStartButton;
    public Image continuePanelRef;
    public Image retryPanelRef;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _gainedGoldText;
    [SerializeField] private Animator _powerUpAnimator;

    private Vector3 panelCurrentPos;

    private void Start()
    {
        _levelText.text = $"Level-{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Substring(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Length - 1)}";
        Actions.OnLevelSuccesful += VisualizeGainedCoin;
    }

    public void DragPanelToScreen(Image panel)
    {
        panel.rectTransform.anchoredPosition = Vector3.SmoothDamp(panel.rectTransform.anchoredPosition, Vector3.zero, ref panelCurrentPos, .3f);
    }

    public void UpdateGoldText()
    {
        _goldText.text = PlayerPrefs.GetInt("myGold").ToString();
    }

    public void VisualizeGainedCoin() 
    {
        _gainedGoldText.text = GameManager.instance.gainedCoins.ToString();
    }

    public void StartAnimations()
    {
        _powerUpAnimator.SetTrigger("Start");
    }

}
