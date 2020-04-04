using UnityEngine;
public interface IHittable{
    void GetHit(Vector3 position, Vector3 direction, float damage);
}