using UnityEngine;
public interface IHittable{
    public void GetHit(Vector3 position, Vector3 direction, float damage);
}