using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using Match3.Scripts.Core;
using UnityEngine;

namespace Match3.Scripts.UI
{
    public class GameScene : MonoBehaviour
    {
		public static GameScene _instance;
        public GameField gameField;
        public GameUi gameUi;
        private bool gameStarted;
		private bool gameFinished;
        private bool boosterMode;

        private void Awake()
		{
			Assert.IsNotNull(gameField);
			Assert.IsNotNull(gameUi);
			_instance = this;
        }

		private void Start()
        {
            gameField.LoadLevel();
        }

        private void Update()
		{
			if (!gameStarted || gameFinished)
			{
				return;
			}

            if (boosterMode)
			{
			}
			else
			{
				gameField.fieldHandler.HandleInput();
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				StartCoroutine(gameField.fieldHandler.ProcessingChips());
			}
			if (Input.GetKeyDown(KeyCode.G))
			{
				gameField.fieldHandler.DestroyChips();
			}	
        }

        public void StartGame()
		{
			gameStarted = true;
			gameField.StartGame();
		}

        public void EndGame()
		{
			gameFinished = true;
			gameField.EndGame();
			Debug.Log("Game Over");
			GetComponent<SceneTransition>().PerformTransition();
		}

        public void RestartGame()
		{
            gameStarted = false;
            gameFinished = false;
			gameField.ReloadLevelData();
		}

        public void CheckEndGame()
        {
            if (gameFinished)
            {
				//Game is not over
                return;
            }

			if (gameField.currentLimit == 0)
            {
				//Time or Moves are over
                EndGame();
            }

            var goalsComplete = false;
			if (goalsComplete)
            {
				//Win with completed goals
				MatchManager.Instance.gameData.currentLevel++;
				MatchManager.Instance.SaveData();
				EndGame();
			}
			else
			{
				//Lose: Time or Moves are over and goals not complete
			}
        }
    }
}
