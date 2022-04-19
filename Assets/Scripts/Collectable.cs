using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public float effectRate = .25f;

    protected abstract void OnItemCollected(GameObject obj);
}
