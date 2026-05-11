using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;

public class TitleScreenHover : MonoBehaviour
{   
    [SerializeField] TextMeshProUGUI phrase;
    public string[] dickDescs = new string[]
    {
        "Dick likes the ocean.",
        "Dick likes peanut butter and jelly.",
        "Dick pronounces it 'gif'."
    };

    void OnEnable()
    {
        int index = Random.Range(0, dickDescs.Length);

        Debug.Log("it picked " + dickDescs[index]);
        
        phrase.text = dickDescs[index];

    }

}
