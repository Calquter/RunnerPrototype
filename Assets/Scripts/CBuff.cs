using UnityEngine;

public class CBuff : Collectable
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            OnItemCollected(other.gameObject);
    }

    protected override void OnItemCollected(GameObject obj)
    {
        obj.GetComponent<PlayerController>().CalculateBuffEffects(base.effectRate);
        gameObject.SetActive(false);
    }
}
