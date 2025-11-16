using Swizzlebake.SimpleAbilitySystem.Abilities.Data;
using Swizzlebake.SimpleAbilitySystem.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Swizzlebake.SimpleAbilitySystem.Abilities
{
    /// <summary>
    /// The <c>Entity</c> class represents an in-game entity that can possess abilities, exist within a game world,
    /// and interact with other game systems such as emitting tags for event handling.
    /// </summary>
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

        /// <summary>
        /// Initializes the entity within the game world and sets up its ability system with the provided configuration.
        /// This method links the entity to a <c>WorldManager</c>, assigns it a grid cell, applies traits from the
        /// <c>DataConfig</c>, and ensures the tag-emission events are cleared.
        /// </summary>
        /// <param name="worldManager">The <c>WorldManager</c> instance managing the game world and entities.</param>
        /// <param name="dataConfig">The <c>DataConfig</c> providing traits and data for the entity's ability system.</param>
        public void Init(WorldManager worldManager, DataConfig dataConfig)
        {
            _worldManager = worldManager;
           _gridCell = _worldManager.AddEntity(this);
           _abilitySystem.AddTraits(dataConfig.GetTraits());
           _tagEmitted.RemoveAllListeners();
        }

        /// <summary>
        /// Invokes the associated event when a game tag is emitted by the entity, allowing listeners to react to the tag.
        /// This method is typically called internally by abilities or other systems that emit tags on behalf of the entity.
        /// </summary>
        /// <param name="gameTag">The <c>GameTag</c> that has been emitted.</param>
        public void OnTagEmitted(GameTag gameTag)
        {
            _tagEmitted.Invoke(gameTag);
        }

        /// <summary>
        /// Registers an action to be invoked whenever a <c>GameTag</c> is emitted by this entity.
        /// The provided action will be added as a listener to the internal tag-emitted event system,
        /// enabling external components or systems to respond to tag emissions.
        /// </summary>
        /// <param name="action">The callback to execute when a <c>GameTag</c> is emitted. This action receives the emitted <c>GameTag</c> as a parameter.</param>
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