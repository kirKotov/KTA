using UnityEngine;
using TMPro;
using Mirror;

public class TechSelection : NetworkBehaviour
{
    public Transform floatingInfo;

    [SyncVar]
    public int techNumber = 0;

    public TextMeshPro textMeshName;

    [SyncVar(hook = nameof(HookSetName))]
    public string playerNickname = "";

    private void Start()
    {
        if (isLocalPlayer)
        {
            StaticZVariables.playerCamera = GetComponentInChildren<Camera>();

            floatingInfo.GetChild(0).gameObject.SetActive(false);

            AssignName();
        }
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