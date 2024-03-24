using UnityEngine;
using TMPro;
using Mirror;

public class TechSelection : NetworkBehaviour
{
    public Transform floatingInfo;

    [SyncVar]
    public int techNumber = 0;

    [SyncVar]
    public int techHealth = 0;

    public TextMeshPro textMeshName;

    [SyncVar(hook = nameof(HookSetName))]
    public string playerNickname = "";


    //private []
    private GameObject _playerLookPos;

    private void Start()
    {
        if (isLocalPlayer)
        {
            StaticZVariables.playerCamera = GetComponentInChildren<Camera>();

            StaticZVariables.playerTechSelection = gameObject.GetComponent<TechSelection>();

            floatingInfo.GetChild(0).gameObject.SetActive(false);

            AssignName();
        }
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        //if (techHealth < 0)
    }

    void HookSetName(string _old, string _new)
    {
        AssignName();
    }

    public void AssignName()
    {
        textMeshName.text = playerNickname;
    }
}