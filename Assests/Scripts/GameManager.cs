using UnityEngine;
// using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMesh timeText;
    private float timeSpent = 0.0f;

    void Update()
    {
        timeSpent += Time.deltaTime;
        timeText.text = "Time: " + Mathf.FloorToInt(timeSpent).ToString();
    }
}