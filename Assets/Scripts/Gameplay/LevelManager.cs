using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vampire
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelBlueprint levelBlueprint;
        [SerializeField] private Character playerCharacter;
        [SerializeField] private EntityManager entityManager;
        [SerializeField] private AbilityManager abilityManager;
        [SerializeField] private AbilitySelectionDialog abilitySelectionDialog;
        [SerializeField] private InfiniteBackground infiniteBackground;
        [SerializeField] private Inventory inventory;
        [SerializeField] private StatsManager statsManager;
        [SerializeField] private GameOverDialog gameOverDialog;
        [SerializeField] private GameTimer gameTimer;
        private float levelTime = 0;
        private float timeSinceLastMonsterSpawned;
        private float timeSinceLastChestSpawned;
        private bool miniBossSpawned = false;
        private bool finalBossSpawned = false;

        public void Init(LevelBlueprint levelBlueprint)
        {
            this.levelBlueprint = levelBlueprint;
            levelTime = 0;

            // Bắt đầu session mới với PlayerDataManager
            PlayerDataManager.Instance.StartNewSession();

            // Initialize the entity manager
            entityManager.Init(this.levelBlueprint, playerCharacter, inventory, statsManager, infiniteBackground, abilitySelectionDialog);
            // Initialize the ability manager
            abilityManager.Init(this.levelBlueprint, entityManager, playerCharacter, abilityManager);
            abilitySelectionDialog.Init(abilityManager, entityManager, playerCharacter);
            // Initialize the character
            playerCharacter.Init(entityManager, abilityManager, statsManager);
            playerCharacter.OnDeath.AddListener(GameOver);
            // Spawn initial gems
            entityManager.SpawnGemsAroundPlayer(this.levelBlueprint.initialExpGemCount, this.levelBlueprint.initialExpGemType);
            // Spawn a singular chest
            entityManager.SpawnChest(levelBlueprint.chestBlueprint);
            // Initialize the infinite background
            infiniteBackground.Init(this.levelBlueprint.backgroundTexture, playerCharacter.transform);
            // Initialize inventory
            inventory.Init();
        }

        // Start is called before the first frame update
        void Start()
        {
            Init(levelBlueprint);
        }

        // Update is called once per frame
        void Update()
        {
            // Time
            levelTime += Time.deltaTime;
            gameTimer.SetTime(levelTime);

            // Cập nhật session time và score cho PlayerDataManager
            PlayerDataManager.Instance.UpdateSessionTime(levelTime);
            PlayerDataManager.Instance.UpdateSessionScore(CalculateCurrentScore());

            // Monster spawning timer
            if (levelTime < levelBlueprint.levelTime)
            {
                timeSinceLastMonsterSpawned += Time.deltaTime;
                float spawnRate = levelBlueprint.monsterSpawnTable.GetSpawnRate(levelTime / levelBlueprint.levelTime);
                float monsterSpawnDelay = spawnRate > 0 ? 1.0f / spawnRate : float.PositiveInfinity;
                if (timeSinceLastMonsterSpawned >= monsterSpawnDelay)
                {
                    (int monsterIndex, float hpMultiplier) = levelBlueprint.monsterSpawnTable.SelectMonsterWithHPMultiplier(levelTime / levelBlueprint.levelTime);
                    (int poolIndex, int blueprintIndex) = levelBlueprint.MonsterIndexMap[monsterIndex];
                    MonsterBlueprint monsterBlueprint = levelBlueprint.monsters[poolIndex].monsterBlueprints[blueprintIndex];
                    entityManager.SpawnMonsterRandomPosition(poolIndex, monsterBlueprint, monsterBlueprint.hp * hpMultiplier);
                    timeSinceLastMonsterSpawned = Mathf.Repeat(timeSinceLastMonsterSpawned, monsterSpawnDelay);
                }
            }
            // Boss spawning
            if (!miniBossSpawned && levelTime > levelBlueprint.miniBosses[0].spawnTime)
            {
                miniBossSpawned = true;
                entityManager.SpawnMonsterRandomPosition(levelBlueprint.monsters.Length, levelBlueprint.miniBosses[0].bossBlueprint);
            }
            // Boss spawning
            if (!finalBossSpawned && levelTime > levelBlueprint.levelTime)
            {
                //entityManager.KillAllMonsters();
                finalBossSpawned = true;
                Monster finalBoss = entityManager.SpawnMonsterRandomPosition(levelBlueprint.monsters.Length, levelBlueprint.finalBoss.bossBlueprint);
                finalBoss.OnKilled.AddListener(LevelPassed);
            }
            // Chest spawning timer
            timeSinceLastChestSpawned += Time.deltaTime;
            if (timeSinceLastChestSpawned >= levelBlueprint.chestSpawnDelay)
            {
                for (int i = 0; i < levelBlueprint.chestSpawnAmount; i++)
                {
                    entityManager.SpawnChest(levelBlueprint.chestBlueprint);
                }
                timeSinceLastChestSpawned = Mathf.Repeat(timeSinceLastChestSpawned, levelBlueprint.chestSpawnDelay);
            }
        }

        /// <summary>
        /// Tính điểm hiện tại dựa trên các yếu tố game
        /// </summary>
        private int CalculateCurrentScore()
        {
            // Công thức tính điểm: enemies killed * 100 + survival time * 10 + coins * 5
            return statsManager.MonstersKilled * 100 +
                   Mathf.FloorToInt(levelTime * 10) +
                   statsManager.CoinsGained * 5;
        }

        public void GameOver()
        {
            Time.timeScale = 0;

            // Kết thúc session và lưu dữ liệu
            SessionResult sessionResult = PlayerDataManager.Instance.EndSession(statsManager);
            gameOverDialog.Open(false, statsManager, sessionResult);
        }

        public void LevelPassed(Monster finalBossKilled)
        {
            Time.timeScale = 0;

            // Kết thúc session và lưu dữ liệu
            SessionResult sessionResult = PlayerDataManager.Instance.EndSession(statsManager);
            gameOverDialog.Open(true, statsManager, sessionResult);
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
