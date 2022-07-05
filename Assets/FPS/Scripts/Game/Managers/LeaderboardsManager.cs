using UnityEngine;
using LootLocker.Requests;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace Unity.FPS.Game
{
    public class LeaderboardsManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nicknameInput;
        [SerializeField] private Button submitButton;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private int score;
        [SerializeField] private string leaderboardKey;
        [SerializeField] private GameObject leadersListParent;
        [SerializeField] private GameObject leadersListItem;
        [SerializeField] private bool isScoreSubmitted = false;

        private void Start()
        {
            InitPlayerData();
            formatNicknameInput();
            LootLockerStartGuestSession();
        }

        private void Update()
        {
            if (string.IsNullOrEmpty(nicknameInput.text) || isScoreSubmitted)
            {
                submitButton.interactable = false;
            }
            else
            {
                submitButton.interactable = true;
            }
        }

        public void SubmitScore()
        {
            LootLockerSDKManager.SubmitScore(nicknameInput.text, score, leaderboardKey, (response) =>
            {
                if (response.success)
                {
                    isScoreSubmitted = true;
                    SaveUsedNickname();
                    GetSurroundingScore(nicknameInput.text);
                }
            });
        }

        public void GetSurroundingScore(string memberID)
        {
            LootLockerSDKManager.GetMemberRank(leaderboardKey, memberID, (responseMemberRank) =>
            {
                if (responseMemberRank.success)
                {
                    int memberRank = responseMemberRank.rank;
                    int count = 5;
                    int after = memberRank < 3 ? 0 : memberRank - 3;

                    LootLockerSDKManager.GetScoreList(leaderboardKey, count, after, (responseScoreList) =>
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
                                    rankText.fontStyle = FontStyles.Bold | FontStyles.Underline;
                                    nicknameText.fontStyle = FontStyles.Bold | FontStyles.Underline;
                                    scoreText.fontStyle = FontStyles.Bold | FontStyles.Underline;
                                }
                            }
                        }
                    });
                }
            });
        }

        private void InitPlayerData()
        {
            score = DataPersistenceManager.Instance.playerScore;
            scoreText.text = score.ToString();

            string lastUsedNickname = DataPersistenceManager.Instance.currentSettings.playerLastUsedNickname;
            if (!string.IsNullOrEmpty(lastUsedNickname))
            {
                nicknameInput.text = lastUsedNickname;
            }
        }

        private void formatNicknameInput()
        {
            nicknameInput.onValueChanged.AddListener((text) =>
            {
                Regex rgx = new Regex("[^a-zA-Z0-9]");
                string formattedText = rgx.Replace(text, "");
                nicknameInput.text = formattedText;
            });
        }

        private void LootLockerStartGuestSession()
        {
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (!response.success)
                {
                    return;
                }
            });
        }

        private void SaveUsedNickname()
        {
            DataPersistenceManager dataManager = DataPersistenceManager.Instance;
            DataPersistenceManager.SettingsSaveData settings = dataManager.currentSettings;

            settings.playerLastUsedNickname = nicknameInput.text;
            dataManager.SaveSettings(settings);
        }
    }
}
