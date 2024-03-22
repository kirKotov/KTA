using Mirror.Examples.CharacterSelection;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRefereses : MonoBehaviour
{
    [SerializeField] private InputField _inputFieldPlayerName;

    private GameObject _currentInstantiatedTech;
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

        _currentInstantiatedTech = Instantiate(_techData.techPrefabs[techNumber]);
        _techSelection = _currentInstantiatedTech.GetComponent<TechSelection>();

        SetupPlayer();
    }

    public void SetupPlayer()
    {
        _techSelection.techNumber = StaticZVariables.techNumber;

        _techSelection.playerNickname = StaticZVariables.playerNickname;
        _techSelection.AssignName();
    }

    public void LoadData()
    {
        if (StaticZVariables.playerNickname != "")
            _inputFieldPlayerName.text = StaticZVariables.playerNickname;
    }
}