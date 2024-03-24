using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnlineTextCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _onlineText;

    [SerializeField] private GameObject _settingsMenu;

    [SerializeField] private Slider _volumeSlider;

    [SerializeField] private TurretController _turretController;
    [SerializeField] private GunsController _gunsController;
    [SerializeField] private CameraOrbit _cameraOrbit;

    private AudioSource _audioSource;

    private List<GameObject> _playerList = new List<GameObject>();

    private bool _isSettingOpen = false;

    private void Start()
    {
        _audioSource = GameObject.FindGameObjectWithTag("MusicObj").GetComponent<AudioSource>();

        _volumeSlider.onValueChanged.AddListener(ChangeVolume);

        _audioSource.volume = StaticZVariables.musicVolume;

        _volumeSlider.value = StaticZVariables.musicVolume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isSettingOpen == false)
            {
                _settingsMenu.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                _isSettingOpen = true;

                _cameraOrbit.MouseSensitivity = 0;
                _gunsController.enabled = false;
                _turretController.enabled = false;
            }
            else
            {
                _settingsMenu.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                _isSettingOpen = false;

                _cameraOrbit.MouseSensitivity = 6;
                _gunsController.enabled = true;
                _turretController.enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        _playerList = GameObject.FindGameObjectsWithTag("Vehicles").ToList();

        _onlineText.text = $"Текущий онлайн: {_playerList.Count} / 10";
    }

    private void ChangeVolume(float volume)
    {
        StaticZVariables.musicVolume = volume;

        _audioSource.volume = StaticZVariables.musicVolume;
    }
}