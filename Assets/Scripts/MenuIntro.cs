using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class MenuIntro : MonoBehaviour
{
    int dernierScore;
    [SerializeField] TMP_Text txtScores;
    void Start()
    {
        //si la cle esxiste on enregistre sinon on met ine valeur par default
        if (PlayerPrefs.HasKey("score"))
        {
            dernierScore = PlayerPrefs.GetInt("score");
        }
        else
        {
            dernierScore = 0;
        }
        txtScores.text = $"Dernier score: {dernierScore} coups";

    }
    public void Demarrer()
    {
        SceneManager.LoadScene("Jeu");
    }
}
