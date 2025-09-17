using UnityEngine;
using TMPro;

public class Connecting : MonoBehaviour
{
    public TextMeshProUGUI DotText;
//a
    void Update()
    {
        int dotCount = (int)(Time.time % 6);
        DotText.text = new string('.', dotCount);
    }
}
