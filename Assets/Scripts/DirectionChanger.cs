using UnityEngine;

public class DirectionChanger : MonoBehaviour
{
    [SerializeField] private float _directionY;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().newDirection += _directionY;
            StartCoroutine(GameManager.instance.playerController.PermissonToInputs());
        }
    }
}
