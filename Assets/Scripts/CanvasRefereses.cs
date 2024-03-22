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
        _currentInstantiatedTech = Instantiate(_techData.techPrefabs[techNumber]);
        _techSelection = _currentInstantiatedTech.GetComponent<TechSelection>();

        SetupPlayerName();
    }

    public void SetupPlayerName()
    {
        _techSelection.playerNickname = StaticZVariables.playerNickname;
        _techSelection.AssignName();
    }
}