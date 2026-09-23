using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Permet de déplacer la cible de la caméra en fonction des entrées du joueur.
/// </summary>
public class CibleCamera : MonoBehaviour
{
    [SerializeField]
    private PlayerInput controles;

    [SerializeField]
    private float vitesseDeplacement;

    private Vector3 deplacement;

    [SerializeField, Tooltip("Le volume dans lequel la caméra peut se déplacer.")]
    private BoxCollider volumeCarte;

    [SerializeField, Tooltip("Vitesse de rotation de la caméra en degrés par seconde.")]
    private float vitesseRotation;

    private float rotation;

    [SerializeField, Tooltip("Vitesse d'inclinaison de la caméra en degrés par seconde.")]
    private float vitesseInclinaison;

    [SerializeField, Tooltip("Limites d'inclinaison de la caméra en degrés. Doit être plus petit que X ou plus grand que Y")]
    private Vector2 limitesInclinaison;

    private float inclinaison;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputAction actionDeplacer = controles.actions["Player/DeplacerCamera"];
        actionDeplacer.performed += CommencerDeplacement;
        actionDeplacer.canceled += ArreterDeplacement;

        InputAction actionRotation = controles.actions["Player/RotationCamera"];
        actionRotation.performed += CommencerRotation;
        actionRotation.canceled += ArreterRotation;

        InputAction actionInclinaison = controles.actions["Player/InclinaisonCamera"];
        actionInclinaison.performed += CommencerInclinaison;
        actionInclinaison.canceled += ArreterInclinaison;

        InputAction actionZoom = controles.actions["Player/ZoomCamera"];
        actionZoom.performed += CommencerZoom;
        actionZoom.canceled += ArreterZoom;
    }

    private void OnDestroy()
    {
        if (controles == null)
            return;

        InputAction actionDeplacer = controles.actions["Player/DeplacerCamera"];
        actionDeplacer.performed -= CommencerDeplacement;
        actionDeplacer.canceled -= ArreterDeplacement;

        InputAction actionRotation = controles.actions["Player/RotationCamera"];
        actionRotation.performed -= CommencerRotation;
        actionRotation.canceled -= ArreterRotation;

        InputAction actionInclinaison = controles.actions["Player/InclinaisonCamera"];
        actionInclinaison.performed -= CommencerInclinaison;
        actionInclinaison.canceled -= ArreterInclinaison;

        InputAction actionZoom = controles.actions["Player/ZoomCamera"];
        actionZoom.performed -= CommencerZoom;
        actionZoom.canceled -= ArreterZoom;
    }

    private void Update()
    {
        AppliquerDeplacement();
        AppliquerRotation();
        AppliquerInclinaison();
        AppliquerZoom();
    }

    #region Deplacement
    private void CommencerDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = vitesseDeplacement * contexte.ReadValue<Vector2>().normalized;
    }

    private void ArreterDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = Vector3.zero;
    }

    private void AppliquerDeplacement()
    {
        if (deplacement.sqrMagnitude > 0f)
        {
            Vector3 prochainDeplacement = transform.right * deplacement.x +
            transform.forward * deplacement.y;
            prochainDeplacement *= Time.deltaTime;

            Vector3 nouvellePosition = transform.position + prochainDeplacement;
            if (volumeCarte.bounds.Contains(nouvellePosition))
            {
                transform.position = nouvellePosition;
            }
        }
    }
    #endregion

    #region Rotation
    private void CommencerRotation(InputAction.CallbackContext contexte)
    {
        rotation = contexte.ReadValue<float>();
    }

    private void ArreterRotation(InputAction.CallbackContext contexte)
    {
        rotation = 0f;
    }

    private void AppliquerRotation()
    {
        if (!Mathf.Approximately(rotation, 0f))
        {
            transform.Rotate(Vector3.up * rotation * vitesseRotation * Time.deltaTime, Space.World);
        }
    }
    #endregion

    #region Inclinaison
    private void CommencerInclinaison(InputAction.CallbackContext contexte)
    {
        inclinaison = contexte.ReadValue<float>();
    }

    private void ArreterInclinaison(InputAction.CallbackContext contexte)
    {
        inclinaison = 0f;
    }

    private void AppliquerInclinaison()
    {
        if (!Mathf.Approximately(inclinaison, 0f))
        {
            float angle = (transform.localEulerAngles.x + inclinaison * vitesseInclinaison * Time.deltaTime) % 360;
            //Debug.Log($"Angle actuel: {transform.localEulerAngles.x}, Angle après inclinaison: {angle}, Limites: {limitesInclinaison.x} - {limitesInclinaison.y}");
            if (angle < limitesInclinaison.x || angle > limitesInclinaison.y)
            {
                transform.Rotate(new Vector3(inclinaison * vitesseInclinaison * Time.deltaTime, 0.0f, 0.0f), Space.Self);
            }
        }
    }
    #endregion

    #region Zoom

    private float zoom;

    [SerializeField]
    private float vitesseZoom;

    [SerializeField]
    private CinemachineCamera camera;

    [SerializeField]
    private Vector2 limiteZoom;

    private void CommencerZoom(InputAction.CallbackContext contexte)
    {
        zoom = contexte.ReadValue<float>() * vitesseZoom;
    }

    private void ArreterZoom(InputAction.CallbackContext contexte)
    {
        zoom = 0f;
    }

    private void AppliquerZoom()
    {
        CinemachinePositionComposer positionComposer = camera.GetComponent<CinemachinePositionComposer>();
        Vector3 offset = positionComposer.TargetOffset;

        Vector3 zoomApplique = offset.normalized * zoom;
        float distanceOffset = (positionComposer.TargetOffset + zoomApplique).magnitude;

        if (distanceOffset < limiteZoom.x || distanceOffset > limiteZoom.y)
        {
            return;
        }
        
        positionComposer.TargetOffset += zoomApplique;        
    }

    #endregion

}

