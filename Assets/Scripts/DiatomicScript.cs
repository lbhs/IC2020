using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DiatomicScript : MonoBehaviour
{
    public GameObject DissociationProduct;
    public int BondDissociationEnergy;

    [SerializeField]
    private BadgePrefabs BadgeData;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PhotonView>().RPC("ApplyBadge", RpcTarget.All, 0, GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    private void ApplyBadge(int BadgePrefabIndex, int RecipientPVID)
    {
        GameObject BadgeRecipient = PhotonView.Find(RecipientPVID).gameObject;
        GameObject Badge = Instantiate(BadgeData.Badges[BadgePrefabIndex], BadgeRecipient.transform);
        // offsets the badge by the height of the recipient
        Badge.transform.localPosition = new Vector3(Badge.transform.localPosition.x,
                                                    Badge.transform.localPosition.y,
                                                    Badge.transform.localPosition.z + Mathf.Abs(BadgeRecipient.transform.position.z));
        Badge.transform.localPosition = new Vector3(-1.2f, 1f);
        Badge.transform.Rotate(0, 0, -BadgeRecipient.transform.rotation.eulerAngles.z);
    }
}
