using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Génère des ennemis à intervalles réguliers et les place dans la scène.
/// </summary>
public class GenerateurEnnemis : MonoBehaviour
{
    [SerializeField, Tooltip("Emplacement où les ennemis sont générés")]
    private GameObject emplacementGeneration;

    [SerializeField, Tooltip("Destination des ennemis")]
    private GameObject destinationEnnemis;

    [SerializeField, Tooltip("Prefab de l'ennemi à générer")]
    private Ennemi prefabEnnemi;

    [SerializeField, Tooltip("Délai entre chaque vague d'ennemis (en secondes)")]
    private float delaiEntreVague;

    [SerializeField, Tooltip("Nombre d'ennemis par vague")]
    private float tailleVague;

    [SerializeField ,Tooltip("Nombre de vagues à générer")]
    private int nombreVagues;

    [SerializeField, Tooltip("Délai entre chaque ennemi généré (en secondes)")]
    private float delaiEntreEnnemi;

    // Est-ce qu'une vague est en cours de génération
    private bool vagueEnCours;

    // Indice de la vague actuelle
    private int indiceVague;
}
