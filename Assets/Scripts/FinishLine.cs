using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject[] _particles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var item in _particles)
                item.SetActive(true);

            GameManager.instance.playerController.animator.SetTrigger("Victory");
            Actions.OnLevelSuccesful.Invoke();
        }
            
    }
}
