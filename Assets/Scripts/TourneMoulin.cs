using UnityEngine;

public class TourneMoulin : MonoBehaviour
{
    [SerializeField] float vitesseRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    // vert up, rouge forward, bleu right
    void Update()
    {
        transform.Rotate(Vector3.forward * vitesseRotation * Time.deltaTime, Space.Self);
    }
}
