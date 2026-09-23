using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Lanceur : MonoBehaviour
{
    private List<Ennemi> ciblesPotentielles;

    [SerializeField, Tooltip("Delais entre chaque attaque (en secondes)")]
    private float delaiAttaque;

    private Ennemi cibleActive;

    private void Awake()
    {
        ciblesPotentielles = new List<Ennemi>();
        cibleActive = null;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ennemi ennemi))
        {
            ciblesPotentielles.Add(ennemi);

            if (cibleActive == null)
            {
                cibleActive = ciblesPotentielles[0];
                StartCoroutine(CoroutineAttaque());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Ennemi ennemi))
        {
            ciblesPotentielles.Remove(ennemi);

            if (cibleActive == ennemi)
            {
                if (ciblesPotentielles.Count > 0)
                {
                    cibleActive = ciblesPotentielles[0];
                }
                else
                {
                    cibleActive = null;
                }
            }
        }
    }

    /// <summary>
    /// Déclenche l'attaque de la tour en instanciant un projectile et en le dirigeant vers la cible active.
    /// </summary>
    private void Attaquer()
    {
        Projectile projectile = PoolProjectiles.Instance.GetProjectile();
        projectile.Initialiser(cibleActive, transform.position, Quaternion.identity);
    }

    private IEnumerator CoroutineAttaque()
    {
        while (cibleActive != null)
        {
            Attaquer();
            yield return new WaitForSeconds(delaiAttaque);
        }
    }
}
