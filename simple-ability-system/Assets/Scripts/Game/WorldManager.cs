using System;
using Swizzlebake.SimpleAbilitySystem.Abilities;
using UnityEngine;

namespace Swizzlebake.SimpleAbilitySystem.Game
{
    /// <summary>
    /// Represents a single cell within the world's grid structure, storing the entities contained within it.
    /// </summary>
    [Serializable]
    public struct WorldCell
    {
        public Entity[] Entities;
    }

    /// <summary>
    /// Manages the world grid system, handles entity placement, retrieval, and interactions within a defined world space.
    /// </summary>
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

        /// <summary>
        /// Adds an entity to the world at its current grid cell location.
        /// If the grid cell is full, the entity array is resized to accommodate the new entity.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to be added to the world grid system.</param>
        /// <returns>
        /// The index of the grid cell where the entity was added or -1 if no valid grid cell could be determined.
        /// </returns>
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

        /// <summary>
        /// Calculates the grid cell index for a given world position.
        /// </summary>
        /// <param name="position">The world position for which to determine the grid cell index.</param>
        /// <returns>
        /// The index of the grid cell corresponding to the provided position, or -1 if the position is outside the valid grid boundaries.
        /// </returns>
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

        /// <summary>
        /// Retrieves the index of the grid cell corresponding to the position of a specified Transform.
        /// </summary>
        /// <param name="systemTransform">The <see cref="Transform"/> whose position is used to determine the grid cell index.</param>
        /// <returns>
        /// The index of the grid cell that contains the specified Transform's position or -1 if no valid grid cell is found.
        /// </returns>
        public int GetGridCell(Transform systemTransform)
        {
            return GetGridCell(systemTransform.position);
        }

        /// <summary>
        /// Retrieves all entities within a specified range from a given entity.
        /// This method checks neighboring grid cells to optimize the search and filters results by distance.
        /// </summary>
        /// <param name="from">The <see cref="Entity"/> from which the range is calculated.</param>
        /// <param name="range">The maximum distance within which to search for entities.</param>
        /// <returns>
        /// An array of <see cref="Entity"/> instances that are located within the specified range.
        /// </returns>
        public Entity[] GetEntitiesInRange(Entity from, float range)
        {
            var cellX = Mathf.FloorToInt(from.transform.position.x / ( _worldGridSizeX));
            var cellZ = Mathf.FloorToInt(from.transform.position.z / (_worldGridSizeZ));
            var cellsInRangeX = Mathf.FloorToInt((from.transform.position.x + range)/WorldSizeX);
            var cellsInRangeZ = Mathf.FloorToInt((from.transform.position.z + range)/WorldSizeZ);
            var entities = new Entity[GameConstants.EntityCount];
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
                        if (!entitiesInCell[i] || entitiesInCell[i] == from)
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

        /// <summary>
        /// Retrieves all entities located in a specified grid cell.
        /// </summary>
        /// <param name="gridCell">The index of the grid cell from which to retrieve entities.</param>
        /// <returns>
        /// An array of <see cref="Entity"/> objects present in the specified grid cell.
        /// </returns>
        public Entity[] GetEntitiesInGridCell(int gridCell)
        {
            return _worldCells[gridCell].Entities;
        }

        /// <summary>
        /// Handles the movement of an entity from one grid cell to another within the world grid system.
        /// Removes the entity from the original grid cell and places it in the target cell.
        /// If the target cell is full, resizes the entity array in the cell to accommodate the addition.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that has moved between grid cells.</param>
        /// <param name="fromCell">The index of the grid cell the entity is moving from.</param>
        /// <param name="toCell">The index of the grid cell the entity is moving to.</param>
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

        /// <summary>
        /// Calculates a random position within a specified range of the owner's transform.
        /// Attempts to find a valid position up to a defined maximum number of attempts, ensuring it is within a valid grid cell in the world.
        /// </summary>
        /// <param name="ownerTransform">The transform of the entity requesting a new position in range.</param>
        /// <param name="range">The maximum distance from the owner's position to calculate the random position.</param>
        /// <returns>
        /// A valid random position within the specified range, or the owner's current position if no valid position is found after exhausting attempts.
        /// </returns>
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