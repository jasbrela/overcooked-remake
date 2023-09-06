using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private Animator borderFeedback;
    [SerializeField] private TextMeshProUGUI display;
    private float _playerScore;
    private int _streak;
    
    private static readonly int Positive = Animator.StringToHash("Positive");
    private static readonly int Feedback = Animator.StringToHash("Feedback");

    public void Score(int value)
    {
        _playerScore += value;
        _streak++;
        
        borderFeedback.SetBool(Positive, true);
        UpdateDisplay();
    }

    public void Miss()
    {
        _streak = 0;
        
        borderFeedback.SetBool(Positive, false);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        display.text = $"{_playerScore} ({_streak})";
        borderFeedback.SetTrigger(Feedback);
    }
}
