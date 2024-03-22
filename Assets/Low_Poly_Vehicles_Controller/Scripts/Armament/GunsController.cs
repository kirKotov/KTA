using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GunsController : NetworkBehaviour
{
    [Header("Bullets Actions")]
	public bool gunsActive;
    public int bulletsLeft = 300;
    public float fireRate = 0.17f;
	
    [Header("Audio Actions")]
    public AudioClip shotSound;
    public AudioSource barrelSource;
    public float minPitch = .9f;
    public float maxPitch = 1.1f;
	
    [Header("Recoil Actions")]
    public float recoilForce;
    public Transform recoilPosition;
	
    [Header("Shot Actions")]
    public Vector3 randomRotation = new Vector3(.1f, .1f, .1f);
    public GameObject ShellPrefab;
    public float shellVelocity = 500f;
    public GameObject hitPrefab;
    public int hitDestroyTime = 10;
    public ParticleSystem muzzleFlash;
    public Transform ejectPoint;
	
    [Header("Barrel Recoil Actions")]
    public Transform recoilBarrelTransform;
    public Vector3 kickBackRecoilBarrel;
    public float kickBackSpeed = 8f;
    public float barrelReturnSpeed = 18f;

    private float fireTimer;
    private Vector3 positionRecoil;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireTimer = fireRate;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (!gunsActive)
            return;

        if (Input.GetKey(KeyCode.Mouse0) && gunsActive)
        {
            Shoot();
        }

        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        Debug.DrawRay(ejectPoint.position, ejectPoint.forward * 200);
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        if (recoilBarrelTransform)
            Recoil();
    }

    private void Recoil()
    {
        positionRecoil = Vector3.Slerp(positionRecoil, Vector3.zero, barrelReturnSpeed * Time.deltaTime);
        recoilBarrelTransform.localPosition = Vector3.Slerp(recoilBarrelTransform.localPosition, -positionRecoil, kickBackSpeed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        if (fireTimer < fireRate)
            return;

        positionRecoil += kickBackRecoilBarrel;

        CmdSpawnBullet();

        CmdPlayShotEffects();

        fireTimer = 0.0f;
    }

    [Command(requiresAuthority = false)]
    private void CmdSpawnBullet()
    {
        RpcSpawnBullet();
    }

    [ClientRpc]
    private void RpcSpawnBullet()
    {
        ejectPoint.localEulerAngles = new Vector3(Random.Range(-randomRotation.x, randomRotation.x), Random.Range(-randomRotation.y, randomRotation.y), Random.Range(-randomRotation.z, randomRotation.z));

        GameObject bullet = Instantiate(ShellPrefab, ejectPoint.position, ejectPoint.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = ejectPoint.forward * shellVelocity;

        rb.AddForceAtPosition(recoilPosition.forward * recoilForce, recoilPosition.position, ForceMode.Impulse);
    }

    [Command(requiresAuthority = false)]
    private void CmdPlayShotEffects()
    {
        RpcPlayShotEffects();
    }

    [ClientRpc]
    private void RpcPlayShotEffects()
    {
        if (muzzleFlash)
            muzzleFlash.Play();

        barrelSource.pitch = Random.Range(minPitch, maxPitch);
        barrelSource.PlayOneShot(shotSound);
    }
}