using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
public class Balle : MonoBehaviour
{


    // [Header("État de jeu")]

Rigidbody rigidbodyDeBalle;

    // [Header("Paramètres de tir")]
[SerializeField] float tirIntensite;

[SerializeField] float accumulateurForce = 0.1f;


    // [Header("Gauge de force")]

[SerializeField] Slider jaugeForce;

    // [Header("Input Actions")]
    //definier une ou plusieur touche comme adeventlistener

[SerializeField] InputAction tirAction;



    // [Header("Composant")]


    void Start()
    {
rigidbodyDeBalle = GetComponent<Rigidbody>();
    }

    void Update()
    {
if(tirAction.WasPressedThisFrame()){
            tirIntensite = 0;
            jaugeForce.value = tirIntensite;
        };

    

if(tirAction.IsPressed()){
            tirIntensite += accumulateurForce;
            tirIntensite = Mathf.Clamp(tirIntensite, jaugeForce.minValue, jaugeForce.maxValue);
            jaugeForce.value = tirIntensite;


    };
if (tirAction.WasReleasedThisFrame()){
rigidbodyDeBalle.AddForce(Vector3.forward * tirIntensite * Time.deltaTime, ForceMode.Impulse);
                tirIntensite = 0;
                jaugeForce.value = tirIntensite;


        };

}

    void OnCollisionEnter(Collision collision)
    {

    }

    void OnTriggerEnter(Collider collision)
    {

    }

    // ===================
    void FrapperBalle()
    {

    }

    void MettreAJourUI()
    {
    }

    // IEnumerator FinJeu()
    // {

    // }

    void SauvegarderScore()
    {

    }

    //=================================
    // Gestion des inputs actions
    void OnEnable()
    {
tirAction.Enable();
    }

    void OnDisable()
    {
tirAction.Disable();
    }
}
