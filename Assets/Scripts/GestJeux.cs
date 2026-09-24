using Unity.VisualScripting;
using UnityEngine;

public class GestJeux : MonoBehaviour
{
    public static GestJeux instance;
    public enum etatJeu { jeu, fin };
    public etatJeu etat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pattern singleton pour garder l'instance de la classe GestJeux entre les scènes
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        etat = etatJeu.jeu;
    }

    public void terminerJeu()
    {
        etat = etatJeu.fin;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
