using Swizzlebake.SimpleAbilitySystem.Abilities.Data;
using Swizzlebake.SimpleAbilitySystem.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        private int _gridCell;
        private AbilitySystem _abilitySystem;
        public AbilitySystem AbilitySystem => _abilitySystem;
        private UnityEvent<GameTag> _tagEmitted = new();
        
        void Awake()
        {
            _abilitySystem = new AbilitySystem();
        }
        
        private WorldManager _worldManager;

        public void Init(WorldManager worldManager, DataConfig dataConfig)
        {
            _worldManager = worldManager;
           _gridCell = _worldManager.AddEntity(this);
           _abilitySystem.AddTraits(dataConfig.GetTraits());
           _tagEmitted.RemoveAllListeners();
        }

        public void OnTagEmitted(GameTag gameTag)
        {
            _tagEmitted.Invoke(gameTag);
        }

        public void RegisterForTagEmitted(UnityAction<GameTag> action)
        {
            _tagEmitted.AddListener(action);
        }
        
        private void Update()
        {
            var gridCell = _worldManager.GetGridCell(transform);
            if (gridCell != -1 && gridCell != _gridCell)
            {
                _worldManager.EntityMovedCell(this, _gridCell, gridCell);
                _gridCell = gridCell;
            }
        }
    }
}