using System;
using Swizzlebake.SimpleAbilitySystem.Abilities;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Game
{
    [Serializable]
    public struct WorldCell
    {
        public Entity[] Entities;
    }
    
    public class WorldManager : MonoBehaviour
    {
        private const int _worldGridSizeX = 5;
        private const int _worldGridSizeZ = 5;

        private const int _worlGridCountX = 10;
        private const int _worlGridCountZ = 10;

        [SerializeField]
        private WorldCell[] _worldCells;

        public float WorldSizeX => _worldGridSizeX * _worlGridCountX;
        public float WorldSizeZ => _worldGridSizeZ * _worlGridCountZ;

        private void Awake()
        {
            _worldCells = new WorldCell[_worlGridCountX * _worlGridCountZ];
            for (int i = 0; i < _worlGridCountX*_worlGridCountZ; i++)
            {
                _worldCells[i] = new WorldCell() { Entities = Array.Empty<Entity>() };   
            }
        }

        public int AddEntity(Entity entity)
        {
            var gridCell = GetGridCell(entity.transform);
            if (gridCell < 0 || gridCell >= _worldCells.Length)
            {
                return -1;
            }
            
            _worldCells[gridCell].Entities ??= new Entity[2];
            var foundSpace = false;
            for (int i = 0; i < _worldCells[gridCell].Entities.Length; i++)
            {
                var entityInCell = _worldCells[gridCell].Entities[i];
                if (!entityInCell)
                {
                    _worldCells[gridCell].Entities[i] = entity;
                    foundSpace = true;
                }
            }

            if (foundSpace)
            {
                var length = _worldCells[gridCell].Entities.Length;
                Array.Resize(ref _worldCells[gridCell].Entities, _worldCells[gridCell].Entities.Length * 2);
                _worldCells[gridCell].Entities[length] = entity;
            }
            
            return gridCell;
        }

        public int GetGridCell(Vector3 position)
        {
            var x = Mathf.FloorToInt(position.x / ( _worldGridSizeX));
            var z = Mathf.FloorToInt(position.z / (_worldGridSizeZ));

            if (x < 0 || z < 0 || x >= _worlGridCountX || z >= _worlGridCountZ)
            {
                return -1;
            }
            
            return x + z * _worlGridCountX;
        }
        
        public int GetGridCell(Transform systemTransform)
        {
            return GetGridCell(systemTransform.position);
        }

        public Entity[] GetEntitiesInRange(Entity from, float range)
        {
            var cell = GetGridCell(from.transform);
            var cellX = Mathf.FloorToInt(from.transform.position.x / ( _worldGridSizeX));
            var cellZ = Mathf.FloorToInt(from.transform.position.z / (_worldGridSizeZ));
            var cellsInRangeX = Mathf.FloorToInt((from.transform.position.x + range)/WorldSizeX);
            var cellsInRangeZ = Mathf.FloorToInt((from.transform.position.z + range)/WorldSizeZ);
            var entities = new Entity[40];
            var entityIndex = 0;
            for (int x = cellX-cellsInRangeX; x < cellX+cellsInRangeX; x++)
            {
                for (int z = cellZ-cellsInRangeZ; z < cellZ+cellsInRangeZ; z++)
                {
                    if (x < 0 || z < 0 || x >= _worlGridCountX || z >= _worlGridCountZ)
                    {
                        continue;
                    }
                    
                    var entitiesInCell = GetEntitiesInGridCell(x + z * _worlGridCountX);
                    for (int i = 0; i < entitiesInCell.Length; i++)
                    {
                        if (entitiesInCell[i] == null || entitiesInCell[i] == from)
                        {
                            continue;
                        }
                        
                        entities[entityIndex++] = entitiesInCell[i];
                    }
                }
            }
            var resultIndex = 0;
            var results = new Entity[entities.Length];
            for (int i = 0; i < entities.Length; i++)
            {
                if (!entities[i])
                {
                    continue;
                }

                if (Vector3.Distance(from.transform.position, entities[i].transform.position) < range)
                {
                    results[resultIndex++] = entities[i];
                }
            }
            Array.Resize(ref results, resultIndex);
            return results;
        }
        
        public Entity[] GetEntitiesInGridCell(int gridCell)
        {
            return _worldCells[gridCell].Entities;
        }

        public void EntityMovedCell(Entity entity, int fromCell, int toCell)
        {
            for (int i = 0; i < _worldCells[fromCell].Entities.Length; i++)
            {
                if (_worldCells[fromCell].Entities[i] == entity)
                {
                    _worldCells[fromCell].Entities[i] = null;
                }
            }

            var foundCell = false;
            for (int i = 0; i < _worldCells[toCell].Entities.Length; i++)
            {
                if (!_worldCells[toCell].Entities[i])
                {
                    _worldCells[toCell].Entities[i] = entity;
                    foundCell = true;
                    break;
                }
            }
            
            if(!foundCell)
            {
                var length = _worldCells[toCell].Entities.Length;
                Array.Resize(ref _worldCells[toCell].Entities, Mathf.Max(_worldCells[toCell].Entities.Length * 2, 2));
                _worldCells[toCell].Entities[++length] = entity;
            }
        }

        public Vector3 GetPositionInRange(Transform ownerTransform, float range)
        {
            const int maxAttempts = 10;
            var i = 0;
            while (i < maxAttempts)
            {
                var randomInCircle = UnityEngine.Random.insideUnitCircle * range;
                var randomPosition = new Vector3(randomInCircle.x, 0, randomInCircle.y) + ownerTransform.position;
                var gridCell = GetGridCell(randomPosition);
                if (gridCell > -1)
                {
                    return randomPosition;
                }
                i++;
            }

            Debug.LogWarning("Failed to find a valid position in range!");
            return ownerTransform.position;
        }
    }
}