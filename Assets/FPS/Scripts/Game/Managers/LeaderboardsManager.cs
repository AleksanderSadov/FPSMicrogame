using UnityEngine;
using LootLocker.Requests;
using TMPro;

public class LeaderboardsManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickname;
    [SerializeField] private int score;
    [SerializeField] private int leaderboardID;
    [SerializeField] private GameObject leadersListParent;
    [SerializeField] private GameObject leadersListItem;

    private void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                return;
            }
        });
    }

    public void SubmitScore()
    {
        LootLockerSDKManager.SubmitScore(nickname.text, score, leaderboardID, (response) =>
        {
            if (response.success)
            {

                GetSurroundingScore(nickname.text);
            }
            else
            {

            }
        });
    }

    public void GetSurroundingScore(string memberID)
    {
        LootLockerSDKManager.GetMemberRank(leaderboardID, memberID, (responseMemberRank) =>
        {
            if (responseMemberRank.success)
            {
                int memberRank = responseMemberRank.rank;
                int count = 5;
                int after = memberRank < 3 ? 0 : memberRank - 3;

                LootLockerSDKManager.GetScoreList(leaderboardID, count, after, (responseScoreList) =>
                {
                    if (responseScoreList.success)
                    {
                        if (leadersListParent.transform.childCount > 0)
                        {
                            foreach (Transform child in leadersListParent.transform)
                            {
                                GameObject.Destroy(child.gameObject);
                            }
                        }

                        for (int i = 0; i < responseScoreList.items.Length; i++)
                        {
                            var scoreItem = responseScoreList.items[i];

                            GameObject listItem = Instantiate(leadersListItem, leadersListParent.transform);
                            listItem.transform.localPosition = new Vector3(0, i * -50, 0);

                            TMP_Text rankText = listItem.transform.GetChild(0).GetComponent<TMP_Text>();
                            rankText.text = "#" + scoreItem.rank;

                            TMP_Text nicknameText = listItem.transform.GetChild(1).GetComponent<TMP_Text>();
                            nicknameText.text = scoreItem.member_id;

                            TMP_Text scoreText = listItem.transform.GetChild(2).GetComponent<TMP_Text>();
                            scoreText.text = scoreItem.score.ToString();

                            if (scoreItem.rank == memberRank)
                            {
                                rankText.fontStyle      = FontStyles.Bold | FontStyles.Underline;
                                nicknameText.fontStyle  = FontStyles.Bold | FontStyles.Underline;
                                scoreText.fontStyle     = FontStyles.Bold | FontStyles.Underline;
                            }
                        }
                    }
                    else
                    {

                    }
                });
            }
            else
            {

            }
        });
    }
}
