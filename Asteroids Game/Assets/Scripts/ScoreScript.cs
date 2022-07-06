using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// this script is attached to ScoreText object in scene
public class ScoreScript : MonoBehaviour
{
    [HideInInspector, Min(0)]public int score = 0;

    public GameObject scorePopUpObj;

    private const float popUpTime = 0.7f;

    public TextMeshProUGUI highscore_TMPro;
    private TextMeshProUGUI score_TMPro; 
    private Animator score_anim;
    void Start()
    {
        highscore_TMPro.text = "High score:" + PlayerPrefs.GetInt("HighScore", 0).ToString();
        score_anim = GetComponent<Animator>();
        score_TMPro = GetComponent<TextMeshProUGUI>();
    }
    private void InstantiateScorePopUp(int score, Vector2 position)
    {
        GameObject scorePopUp = Instantiate(scorePopUpObj, position, Quaternion.identity, transform.parent);
        TextMeshProUGUI scorePopUP_TMPro = scorePopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        scorePopUP_TMPro.text = score.ToString();
        Destroy(scorePopUp, popUpTime);
    }
    public void ScoreFunction(Vector2 asteroidPosition, Vector2 playerPostion, int asteroidSize) // ta funkcja oblicza i aktualizuje score
    {
        int scoreToAdd = (int)Mathf.Ceil(10 * Vector2.Distance(asteroidPosition, playerPostion) / (asteroidSize + 1));
        AddAndPopUpScore(scoreToAdd, asteroidPosition);
    }
    public void AddAndPopUpScore(int scoreToAdd, Vector2 popUpPosition) 
    {
        InstantiateScorePopUp(scoreToAdd, popUpPosition);
        score += scoreToAdd;
        score_TMPro.text = "Score:" + score.ToString();
        score_anim.SetTrigger("gainScore");
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            SetHighScore();
        }
    }
    private void SetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", score);
        highscore_TMPro.text = "High score:" + score;
    }
}
