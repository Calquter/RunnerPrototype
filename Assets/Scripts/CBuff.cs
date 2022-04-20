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
        GameManager.instance.CalculateGainedCoin();
        GameManager.instance.PlayGatherParticle(transform.position);
        gameObject.SetActive(false);
        
    }
}
