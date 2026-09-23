using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;
public class Balle : MonoBehaviour
{


    // [Header("État de jeu")]
    Vector3 positionBalle;
    Rigidbody rigidbodyDeBalle;
    AudioSource audioSourceDelaBalle;
    // [Header("Paramètres de tir")]
    [SerializeField] float tirIntensite;
    [SerializeField] int nbCoup;
    [SerializeField] InputAction angleAction;

    [SerializeField] float angleVitesse;

    [SerializeField] float accumulateurForce = 0.1f;
    [SerializeField] TMP_Text txtcoup;

    // [Header("Gauge de force")]

[SerializeField] Slider jaugeForce;

    // [Header("Input Actions")]
    //definier une ou plusieur touche comme adeventlistener

[SerializeField] InputAction tirAction;



    // [Header("Composant")]
LineRenderer LineRendererdelaballe;
    [SerializeField] AudioClip sonErreur;
    [SerializeField] AudioClip sonFin;


    void Start()
    {
rigidbodyDeBalle = GetComponent<Rigidbody>();
LineRendererdelaballe = GetComponent<LineRenderer>();
        nbCoup = 0;
        MettreAJourUI();
        audioSourceDelaBalle.GetComponent<AudioSource>();
    }

    void Update()
    {
        angleVitesse += angleAction.ReadValue<float>();
        Vector3 direction = Quaternion.Euler(0, angleVitesse, 0) * Vector3.forward;
        LineRendererdelaballe.SetPosition(0, transform.position);
        LineRendererdelaballe.SetPosition(1, transform.position + direction);
        
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
rigidbodyDeBalle.AddForce(direction * tirIntensite * Time.deltaTime, ForceMode.Impulse);
                tirIntensite = 0;
                jaugeForce.value = tirIntensite;
positionBalle = transform.position;
            nbCoup++;

            MettreAJourUI();


        }
        ;

}

    void OnCollisionEnter(Collision collision)
    {
if (collision.gameObject.tag == "horsParcours")
        {
            //sert a arreter objet arrete tt force sur objet

            rigidbodyDeBalle.linearVelocity = Vector3.zero;
            rigidbodyDeBalle.angularVelocity = Vector3.zero;
            audioSourceDelaBalle.PlayOneShot(sonErreur);
            transform.position = positionBalle;

        }
    }

    void OnTriggerEnter(Collider collision)
    {
if (collision.gameObject.tag == "trou")
        {
            //sert a arreter objet arrete tt force sur objet
            
            rigidbodyDeBalle.linearVelocity = Vector3.zero;
            rigidbodyDeBalle.angularVelocity = Vector3.zero;
            transform.position = collision.transform.position;
            rigidbodyDeBalle.useGravity = false;
            Debug.Log("Fin");
            audioSourceDelaBalle.PlayOneShot(sonFin);

        }
    }

    // ===================
    void FrapperBalle()
    {

    }

    void MettreAJourUI()
    {
        txtcoup.text = $"{nbCoup} coup(s)";
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
angleAction.Enable();
    }

    void OnDisable()
    {
tirAction.Disable();
angleAction.Disable();

    }
}
