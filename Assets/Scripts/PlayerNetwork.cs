using UnityEngine;
using Unity.Netcode;
public class PlayerNetwork : NetworkBehaviour {
    private readonly NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(writePerm: NetworkVariableWritePermission.Owner);

    // Update is called once per frame
    void Update()
    {
       if (IsOwner) {
            networkPosition.Value = transform.position;
            networkRotation.Value = transform.rotation;
       } 
       else {
            transform.position = networkPosition.Value;
            transform.rotation = networkRotation.Value; 
       }
    }
}
