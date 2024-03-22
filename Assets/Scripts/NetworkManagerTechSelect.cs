using UnityEngine;
using Mirror;

public class NetworkManagerTechSelect : NetworkManager
{
    public static new NetworkManagerTechSelect singleton => (NetworkManagerTechSelect)NetworkManager.singleton;

    private TechData _techData;

    public override void Awake()
    {
        _techData = TechData.techDataSingleton;
        if (_techData == null)
        {
            Debug.Log("Add CharacterData prefab singleton into the scene.");
            return;
        }

        base.Awake();
    }

    public struct CreateTechMessage : NetworkMessage
    {
        public int techNumber;
        public string playerNickname;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreateTechMessage>(OnCreateTech);
    }

    void OnCreateTech(NetworkConnectionToClient conn, CreateTechMessage message)
    {
        Transform startPos = GetStartPosition();

        if (message.playerNickname == "")
            message.playerNickname = "Игрок: " + UnityEngine.Random.Range(100, 1000);

        Debug.Log(_techData != null);

        GameObject playerObject = startPos != null
            ? Instantiate(_techData.techPrefabs[message.techNumber], startPos.position, startPos.rotation)
            : Instantiate(_techData.techPrefabs[message.techNumber]);


        TechSelection techSelection = playerObject.GetComponent<TechSelection>();
        techSelection.techNumber = message.techNumber;
        techSelection.playerNickname = message.playerNickname;

        NetworkServer.AddPlayerForConnection(conn, playerObject);
    }

    public void CreateTech(CreateTechMessage message)
    {
        NetworkClient.Send(message);
    }
}