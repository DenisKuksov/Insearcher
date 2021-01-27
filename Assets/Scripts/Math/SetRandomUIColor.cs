using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class SetRandomUIColor : MonoBehaviour
{
    [SerializeField, Range(0f,1f)] float _min, _max;

    [SerializeField] private Image[] _images;
    [SerializeField] private Text[] _texts;


    void Start()
    {
        Color c = new Color(Random.Range(_min, _max), Random.Range(_min, _max), Random.Range(_min, _max));
        foreach (var image in _images)
            image.color = c;
        foreach (var text in _texts)
            text.color = c;
    }

}
