using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class OnlineTextCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _onlineText;

    private List<GameObject> _playerList = new List<GameObject>();

    private void FixedUpdate()
    {
        _playerList = GameObject.FindGameObjectsWithTag("Vehicles").ToList();

        _onlineText.text = $"Текущий онлайн: {_playerList.Count} / 10";
    }
}