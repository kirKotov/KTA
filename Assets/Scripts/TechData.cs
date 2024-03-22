using UnityEngine;

public class TechData : MonoBehaviour
{
    public static TechData techDataSingleton { get; private set; }

    public GameObject[] techPrefabs;
    public string[] techTitles;
    public int[] techHealths;

    public void Awake()
    {
        techDataSingleton = this;
    }
}