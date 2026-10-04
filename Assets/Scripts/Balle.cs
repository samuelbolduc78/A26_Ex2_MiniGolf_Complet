using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GestJeux;
public class Balle : MonoBehaviour
{


    // [Header("État de jeu")]
    Vector3 positionBalle;
    Rigidbody rigidbodyDeBalle;
    AudioSource audioSourceDelaBalle;
    // [Header("Paramètres de tir")]
    [SerializeField] float tirIntensite = 12f;
    [SerializeField] int nbCoup;
    [SerializeField] InputAction angleAction;

    [SerializeField] float angleVitesse;

    [SerializeField] float accumulateurForce = 7f;
    [SerializeField] TMP_Text txtcoup;
    [SerializeField] TMP_Text txtFin;
    [SerializeField] private CinemachineCamera FreeLookCamera;
    [SerializeField] private CinemachineCamera CameraFin;


    // [Header("Gauge de force")]

    [SerializeField] Slider jaugeForce;

    // [Header("Input Actions")]
    //definier une ou plusieur touche comme adeventlistener

[SerializeField] InputAction tirAction;



    // [Header("Composant")]
LineRenderer LineRendererdelaballe;
    [SerializeField] AudioClip sonErreur;
    [SerializeField] AudioClip sonFin;
    [SerializeField] AudioClip chansonFin;

    public bool peuxJouer;
    void Start()
    {
rigidbodyDeBalle = GetComponent<Rigidbody>();
LineRendererdelaballe = GetComponent<LineRenderer>();
 nbCoup = 0;
MettreAJourUI();
        txtFin.enabled = false;
        CameraFin.enabled = false;

        audioSourceDelaBalle = GetComponent<AudioSource>();
        peuxJouer = true;
        if (PlayerPrefs.HasKey("dernierPosition"))
        {
            string positionJson = JsonUtility.ToJson(transform.position);

            PlayerPrefs.SetString("dernierPosition", positionJson);

            transform.position = JsonUtility.FromJson<Vector3>(positionJson);


        }
    }

    void Update()
    {

       if (peuxJouer == true && GestJeux.instance.etat == etatJeu.jeu)
        {
            angleVitesse += angleAction.ReadValue<float>();
            Vector3 direction = Quaternion.Euler(0, angleVitesse, 0) * Vector3.forward;
            LineRendererdelaballe.SetPosition(0, transform.position);
            LineRendererdelaballe.SetPosition(1, transform.position + direction);

            if (tirAction.WasPressedThisFrame())
            {
                tirIntensite = 0;
                jaugeForce.value = tirIntensite;
                //...
            }
            ;



            if (tirAction.IsPressed())
            {
                tirIntensite += accumulateurForce;
                tirIntensite = Mathf.Clamp(tirIntensite, jaugeForce.minValue, jaugeForce.maxValue);
                jaugeForce.value = tirIntensite;


            }
            ;
            if (tirAction.WasReleasedThisFrame())
            {
                rigidbodyDeBalle.AddForce(direction * tirIntensite * Time.deltaTime, ForceMode.Impulse);
                tirIntensite = 12f;
                jaugeForce.value = tirIntensite;
                positionBalle = transform.position;
                nbCoup++;

                MettreAJourUI();
                string positionJson = JsonUtility.ToJson(transform.position);
                PlayerPrefs.SetString("dernierPosition", positionJson);
                StartCoroutine(atttendreFinCoup());
            }
        ;
        }

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
            PlayerPrefs.SetInt("score", nbCoup);

        }
    }

    void OnTriggerEnter(Collider collision)
    {
if (collision.gameObject.tag == "trou")
        { 
            rigidbodyDeBalle.linearVelocity = Vector3.zero;
            rigidbodyDeBalle.angularVelocity = Vector3.zero;
            StartCoroutine(FinJeux());
            //sert a arreter objet arrete tt force sur objet
            StartCoroutine(IntroDelai()); 
            transform.position = collision.transform.position;
            rigidbodyDeBalle.useGravity = false;
            audioSourceDelaBalle.PlayOneShot(sonFin);
            LineRendererdelaballe.enabled = false;
            PlayerPrefs.DeleteKey("dernierPosition");
            
            Debug.Log("Fin");
            GestJeux.instance.terminerJeu();
        }
    }

    // ===================
    IEnumerator atttendreFinCoup()
    {
        peuxJouer = false;
        LineRendererdelaballe.enabled = false;
        yield return new WaitForFixedUpdate(); //attend de calc physique ensuite calc vitesse est ce que vitesse descendu6 ?
        float vitesse = rigidbodyDeBalle.linearVelocity.magnitude; //dit longueur de vitesse et distance parcouru par seconde transforme en vitesse avec magnitude
        while (vitesse>0.1f)
        {
            vitesse = rigidbodyDeBalle.linearVelocity.magnitude; //recalc physique vitesse
            yield return null; //attend au prochain frame
        } //utiliser coroutine exo2
        yield return new WaitForSeconds(2);
       
        Debug.Log("Debut");
        Debug.Log("Fin");
        peuxJouer = true;
        LineRendererdelaballe.enabled = true;

    }
    IEnumerator FinJeux()
    {
        CameraFin.enabled = false;

        FreeLookCamera.enabled = true;


        txtFin.enabled = false;
        yield return new WaitForSeconds(3);
        txtFin.enabled = true;

        audioSourceDelaBalle.PlayOneShot(chansonFin);
        txtFin.text = "Bravo!";
        LineRendererdelaballe.enabled = false;
        FreeLookCamera.enabled = false;
        CameraFin.enabled = true;
    }
    IEnumerator IntroDelai()
    {
        yield return new WaitForSeconds(9);

        SceneManager.LoadScene("Intro");

    }
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
