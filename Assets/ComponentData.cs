using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ComponentData : MonoBehaviour
{
    [System.NonSerialized]
    private GameSetupContrller _GSC;
    [System.NonSerialized]
    private JouleDisplayController _JDC;
    [System.NonSerialized]
    private PhotonView _GameSetupPhotonView;

    private void OnEnable()
    {
        _GSC = GameObject.Find("GameSetup").GetComponent<GameSetupContrller>();
        _JDC = GameObject.Find("UI").transform.GetChild(2).GetComponent<JouleDisplayController>();
        _GameSetupPhotonView = GSC.gameObject.GetComponent<PhotonView>();
    }

    public GameSetupContrller GSC
    {
        get
        {
            return _GSC;
        }
    }

    public JouleDisplayController JDC
    {
        get
        {
            return _JDC;
        }
    }

    public PhotonView GameSetupPhotonView
    {
        get
        {
            return _GameSetupPhotonView;
        }
    }
}
