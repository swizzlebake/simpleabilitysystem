using Swizzlebake.SimpleAbilitySystem.Abilities;
using Swizzlebake.SimpleAbilitySystem.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Swizzlebake.SimpleAbilitySystem.Visuals
{
    public class EntityVisuals : MonoBehaviour
    {
        [SerializeField]
        private Material _sharedMaterial;

        [SerializeField] 
        private UnityEvent _onAttack;
        
        [SerializeField] 
        private UnityEvent _onDamage;
        
        private Entity _entity;
        private Material _material;

        private int _materialFlashParameter = Shader.PropertyToID("_FlashStart");
        private void Awake()
        {
            _material = Instantiate(_sharedMaterial);
            GetComponent<MeshRenderer>().material = _material;
        }

        private void Start()
        {
            _entity = GetComponentInParent<Entity>();
            _entity.AbilitySystem.RegisterAttributeChange(OnAttributeChanged);
            _entity.RegisterForTagEmitted(OnTagEmitted);
        }

        private void OnTagEmitted(GameTag gameTag)
        {
            if (gameTag == GameConstants.TagDamaged)
            {
                _onDamage?.Invoke();
            }

            if (gameTag == GameConstants.TagAttacked)
            {
                _onAttack?.Invoke();;
                Debug.Log(transform.root.name + " Attack at " + Time.time);
            }
        }

        public void OnAttributeChanged(AttributeSetParams setParams)
        {
            if (setParams.AttributeName.Equals("Health"))
            {
                _material.SetFloat(_materialFlashParameter, Time.time);
                Debug.Log(transform.root.name + " Damage at " + Time.time);
            }
        }
    }
}