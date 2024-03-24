using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerHealthBar : NetworkBehaviour
{
    [SerializeField] private Image _healthImage;

    private int _maxHealth;

    private TechSelection _techSelection;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        _techSelection = transform.root.GetComponent<TechSelection>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        _healthImage.fillAmount = (float)_techSelection.techHealth / (float)_techSelection.maxHealth;
    }
}