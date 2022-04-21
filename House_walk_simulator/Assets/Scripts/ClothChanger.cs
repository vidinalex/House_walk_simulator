using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothChanger : NetworkBehaviour
{
    private bool needUpdate = false;
    private GameObject localPlayer;
    public void ChangeCloth(int id)
    {
        needUpdate = !needUpdate;
        localPlayer.GetComponent<CharacterBrain>().CallCloth(localPlayer, id, needUpdate);
    }

    public void SetOwner(GameObject player)
    {
        localPlayer = player;
    }
}
