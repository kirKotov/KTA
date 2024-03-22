using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDamage : MonoBehaviour
{
    public Transform spawnPoint;
    public TanksDamageController.Tracks track;
    private TanksDamageController damage;

    private void Start()
    {
        damage = GetComponentInParent<TanksDamageController>();
    }
}
