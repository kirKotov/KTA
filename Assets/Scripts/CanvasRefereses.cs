using UnityEngine;
using TMPro;
using static NetworkManagerTechSelect;

public class CanvasRefereses : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldPlayerName;

    private TechSelection _techSelection;

    private TechData _techData;

    private void Start()
    {
        _techData = TechData.techDataSingleton;
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
        TechSelected(2);
    }

    public void SelectTank()
    {
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