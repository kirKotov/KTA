using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static NetworkManagerTechSelect;

public class CanvasRefereses : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldPlayerName;

    [SerializeField] private Image _lockedBTR;
    [SerializeField] private Image _lockedTank;

    private TechData _techData;

    private void Start()
    {
        _techData = TechData.techDataSingleton;
    }

    private void FixedUpdate()
    {
        if (StaticZVariables.playerKills >= 20)
        {
            _lockedBTR.gameObject.SetActive(false);

            if (StaticZVariables.playerKills >= 60)
                _lockedTank.gameObject.SetActive(false);
        }
    }

    public void InputFieldChangedPlayerName()
    {
        StaticZVariables.playerNickname = _inputFieldPlayerName.text;
    }

    public void SelectJeep()
    {
        TechSelected(1);
    }

    public void SelectBTR()
    {
        if (StaticZVariables.playerKills >= 2)
            TechSelected(2);
    }

    public void SelectTank()
    {
        if (StaticZVariables.playerKills >= 6)
            TechSelected(3);
    }

    private void TechSelected(int techNumber)
    {
        StaticZVariables.techNumber = techNumber;
        StaticZVariables.playerHealth = _techData.techHealths[techNumber];
        StaticZVariables.playerDamage = _techData.techDamage[techNumber];

        CreateTechMessage _characterMessage = new CreateTechMessage
        {
            techNumber = StaticZVariables.techNumber,
            playerNickname = StaticZVariables.playerNickname,
            playerHealth = StaticZVariables.playerHealth,
            playerDamage = StaticZVariables.playerDamage
        };

        NetworkManagerTechSelect.singleton.CreateTech(_characterMessage);
    }

    public void LoadData()
    {
        if (StaticZVariables.playerNickname != "")
            _inputFieldPlayerName.text = StaticZVariables.playerNickname;
    }
}