using System;
using System.Threading;
using System.Threading.Tasks;
using Swizzlebake.SimpleAbilitySystem.Abilities;
using Swizzlebake.SimpleAbilitySystem.Abilities.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Swizzlebake.SimpleAbilitySystem.Game
{
    /// <summary>
    /// GameManager is responsible for orchestrating the game flow,
    /// including initializing and managing game entities and coordinating
    /// with the WorldManager to set up the game world.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        private DataConfig[] _entityConfigs;
        
        private WorldManager _worldManager;
        private Entity[] _entities;
        private async void Start()
        {
            _worldManager = GetComponent<WorldManager>();
            
            var cts = new CancellationTokenSource();
            Application.quitting += () => cts.Cancel();

            try
            {
                await StartGame(cts.Token);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }

        private async Task StartGame(CancellationToken ct)
        {
            if (_entityConfigs.Length == 0)
            {
                Debug.LogError("No entity configs provided");
                return;
            }
            
            _entities = new Entity[GameConstants.EntityCount];
            for (int i = 0; i < GameConstants.EntityCount; i++)
            {
                var config = _entityConfigs[Random.Range(0, _entityConfigs.Length)];
               _entities[i] = CreateEntity(i, config);
               await Task.Delay(Random.Range(0, 150), ct);
            }
        }

        private Entity CreateEntity(int i, DataConfig config)
        {
            var prefab = config.GetPrefab();
            var go = new GameObject($"Entity {i}", typeof(Entity));
            Instantiate(prefab, go.transform);
            
            go.transform.position = new Vector3(Random.value * _worldManager.WorldSizeX, 0, Random.value * _worldManager.WorldSizeZ);

            var entity = go.GetComponent<Entity>();
            entity.Init(_worldManager, config);
            
            return entity;
        }

        private void Update()
        {
           UpdateAbilitySystems(Time.deltaTime);
        }

        private void UpdateAbilitySystems(float dt)
        {
            foreach (var entity in _entities)
            {
                entity?.AbilitySystem.Update(_worldManager, entity, dt);
            }
        }
        
        private void LateUpdate()
        {
            foreach (var entity in _entities)
            {
                entity?.AbilitySystem.LateUpdate();
            }
        }
    }
}