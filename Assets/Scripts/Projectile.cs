using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Cible que le projectile doit atteindre
    private Ennemi cible;

    [SerializeField, Tooltip("Vitesse du projectile en m/s")]
    private float vitesse = 5.0f;

    [SerializeField, Tooltip("Temps de vie maximal du projectile en secondes")]
    private float tempsVieMaximal = 5.0f;

    private float tempsVie;

    private void Awake()
    {
        tempsVie = 0.0f;
    }

    /// <summary>
    /// Initialise le projectile avec la cible à atteindre.
    /// </summary>
    /// <param name="cible">La cible que le projectile doit atteindre</param>
    public void Initialiser(Ennemi cible, Vector3 position, Quaternion rotation)
    {
        this.cible = cible;
        transform.SetPositionAndRotation(position, rotation);
    }

    private void Update()
    {
        // Détruire le projectile s'il dépasse son temps de vie maximal
        if (tempsVie > tempsVieMaximal)
        {
            Destroy(gameObject);
        }

        if (cible != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, cible.transform.position, vitesse * Time.deltaTime);
        }
        else
        {
            // Si la cible n'existe plus, le projectile continue tout droit
            transform.position += transform.forward * vitesse * Time.deltaTime;
        }

        // Mettre à jour le temps de vie du projectile
        tempsVie += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider autre)
    {
        if (autre.gameObject == cible.gameObject)
        {
            // Logique de dégâts ou d'effet sur la cible
            Debug.Log("Projectile a touché la cible : " + cible.name);
            PoolProjectiles.Instance.RecyclerProjectile(this);
        }
    }
}
