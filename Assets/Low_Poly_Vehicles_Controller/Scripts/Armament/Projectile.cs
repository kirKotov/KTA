using Mirror;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int projectileDestroyTime = 25;
    public GameObject hitPrefab;
    public int hitDestroyTime = 10;

    private void Start()
    {
        //Destroy(gameObject, projectileDestroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.FromToRotation(hitPrefab.transform.up, transform.forward)) as GameObject;
        Destroy(hit, hitDestroyTime);
        Destroy(gameObject);

        if (collision.gameObject.CompareTag("Vehicles"))
        {
            if (collision.gameObject.transform.root.gameObject.GetComponent<TechSelection>().techHealth <= StaticZVariables.playerDamage)
                StaticZVariables.playerKills++;

            collision.gameObject.transform.root.gameObject.GetComponent<TechSelection>().TakeDamage(StaticZVariables.playerDamage);
        }
    }
}