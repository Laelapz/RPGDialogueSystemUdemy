using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant _playerConversant;

        [SerializeField] TextMeshProUGUI _nameText;
        [SerializeField] TextMeshProUGUI _aiText;
        [SerializeField] Button _nextButton;
        [SerializeField] Button _quitButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject _choicePrefab;

        void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            _playerConversant.onConversationUpdated += UpdateUI;
            _nextButton.onClick.AddListener(() => _playerConversant.Next());
            _quitButton.onClick.AddListener(() => _playerConversant.Quit());
            UpdateUI();
        }

        void UpdateUI()
        {
            gameObject.SetActive(_playerConversant.IsActive());
            if (!_playerConversant.IsActive()) return;
            _nameText.text = _playerConversant.GetCurrentConversantName();
            AIResponse.SetActive(!_playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(_playerConversant.IsChoosing());

            if (_playerConversant.IsChoosing())
            {
                BuildChoiceList();

            }
            else
            {
                _aiText.text = _playerConversant.GetText();
                _nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }

        }

        private void BuildChoiceList()
        {
            foreach (Transform child in choiceRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueNode choice in _playerConversant.GetChoices())
            {
                GameObject buttonPrefab = Instantiate(_choicePrefab, choiceRoot);
                buttonPrefab.GetComponentInChildren<TextMeshProUGUI>().text = choice.GetDialogueText();
                Button button = buttonPrefab.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    _playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}