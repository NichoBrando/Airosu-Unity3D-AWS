using UnityEngine;
using UnityEngine.UI;
public class UpdateScoreBoard : MonoBehaviour
{
    [SerializeField]
    private Image teamA;

    [SerializeField]
    private Image teamB;
    public void UpdateScoreForTeam(int teamIndex, int score) {
        Image targetTeam = teamIndex == 0 ? teamA : teamB;
        targetTeam.fillAmount = score / 4f;
    }
}
