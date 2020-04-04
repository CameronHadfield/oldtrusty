using UnityEngine;

class TestHittable : MonoBehaviour, IHittable
{
    public void GetHit(Vector3 position, Vector3 direction, float damage)
    {
        Debug.Log("Alas, I am slain");
    }
}