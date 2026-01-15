using System;
using UnityEngine;

[Serializable]
public class PropMeta {
    public short PrefabName;
    public Vector3 Position, Rotation, Scale, SelfGravityVector;
    public bool CanDelete, CanGravity, IsKinematic, HasGravity, HasSelfGravity;

    public void ApplyToGameObject(GameObject obj)
    {
        obj.transform.position = Position;
        obj.transform.eulerAngles = Rotation;
        obj.transform.localScale = Scale;
        
        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.useGravity = HasGravity;
            rb.isKinematic = IsKinematic;
        }
    }
}