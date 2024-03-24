using System.Collections;
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
    [SyncVar]
    public int techDamage = 0;

    public TextMeshPro textMeshName;

    [SyncVar(hook = nameof(HookSetName))]
    public string playerNickname = "";

    public int maxHealth;

    private GameObject _mainMenuCamera;
    private GameObject _playerCanvas;

    private PlayerHealthBar _playerHealthBar;

    private Coroutine regenCoroutine;
    private Coroutine resetTookDamageCoroutine;

    private bool tookDamage = false;

    private const float RegenInterval = 2f;
    private const int RegenAmount = 200;

    private void Start()
    {
        if (isLocalPlayer)
        {
            maxHealth = techHealth;

            _mainMenuCamera = GameObject.FindGameObjectWithTag("MainCamera");
            _mainMenuCamera.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StaticZVariables.playerCamera = GetComponentInChildren<Camera>();
            StaticZVariables.playerTechSelection = gameObject.GetComponent<TechSelection>();

            floatingInfo.GetChild(0).gameObject.SetActive(false);

            AssignName();

            StartRegenerationCoroutine();
        }
    }

    private void StartRegenerationCoroutine()
    {
        regenCoroutine = StartCoroutine(RegenerateHealth());
    }

    private void StopRegenerationCoroutine()
    {
        StopCoroutine(regenCoroutine);
        regenCoroutine = null;
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(RegenInterval);

            if (!tookDamage && techHealth < maxHealth)
            {
                Debug.Log(tookDamage);

                if (techHealth + RegenAmount > maxHealth)
                {
                    techHealth += maxHealth - techHealth;
                    StaticZVariables.playerHealth += maxHealth - techHealth;
                }
                else
                {
                    techHealth += RegenAmount;
                    StaticZVariables.playerHealth += RegenAmount;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isLocalPlayer)
            return;

        techHealth -= damage;
        StaticZVariables.playerHealth -= damage;

        tookDamage = true;

        StopResetTookDamageCoroutine();
        resetTookDamageCoroutine = StartCoroutine(ResetTookDamage());

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

    private IEnumerator ResetTookDamage()
    {
        yield return new WaitForSeconds(3f);
        tookDamage = false;
    }

    private void StopResetTookDamageCoroutine()
    {
        if (resetTookDamageCoroutine != null)
        {
            StopCoroutine(resetTookDamageCoroutine);
            resetTookDamageCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        StopRegenerationCoroutine();
        StopResetTookDamageCoroutine();
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