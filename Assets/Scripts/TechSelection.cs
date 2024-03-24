using UnityEngine;
using TMPro;
using Mirror;
using Unity.VisualScripting;

public class TechSelection : NetworkBehaviour
{
    public Transform floatingInfo;

    [SyncVar]
    public int techNumber = 0;

    [SyncVar]
    public int techHealth = 0;

    [SyncVar]
    public int techDamage = 0;

    public TextMeshPro textMeshName;

    [SyncVar(hook = nameof(HookSetName))]
    public string playerNickname = "";

    private GameObject _mainMenuCamera;

    private GameObject _playerCanvas;

    private PlayerHealthBar _playerHealthBar;

    private void Start()
    {
        if (isLocalPlayer)
        {
            _mainMenuCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _mainMenuCamera.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StaticZVariables.playerCamera = GetComponentInChildren<Camera>();
            StaticZVariables.playerTechSelection = gameObject.GetComponent<TechSelection>();

            floatingInfo.GetChild(0).gameObject.SetActive(false);

            AssignName();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isLocalPlayer)
            return;

        techHealth -= damage;
        StaticZVariables.playerHealth -= damage;

        if (techHealth <= 0)
        {
            StaticZVariables.techNumber = 0;
            StaticZVariables.playerHealth = 0;
            StaticZVariables.playerDamage = 0;

            StaticZVariables.playerCamera = null;
            StaticZVariables.playerTechSelection = null;

            _mainMenuCamera.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            CmdDeletePlayer(gameObject);
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

    [Command(requiresAuthority = false)]
    private void CmdDeletePlayer(GameObject player)
    {
        if (isServer && player != null)
        {
            NetworkServer.UnSpawn(player);
            NetworkServer.Destroy(player);
        }
    }
}