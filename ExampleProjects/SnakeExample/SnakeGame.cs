﻿using System.Collections.Generic;
using UnityEngine;

namespace SnakeExample
{
    public class SnakeGame : MonoBehaviour
    {
        // Types
        private enum TileType
        {
            Empty,
            Wall,
            Food,
        }

        private enum SnakeDirection
        {
            Up,
            Down,
            Left,
            Right,
        }

        // Private
        private TileType[,] map = null;
        private SnakeDirection currentDirection = SnakeDirection.Left;
        private List<Vector2Int> snakeBody = new List<Vector2Int>();
        private List<GameObject> snakeTiles = new List<GameObject>();
        private GameObject foodInstance = null;
        private float moveTimer = 0f;
        private bool gameOver = false;

        private GameObject snakeTile = null;
        private GameObject foodTile = null;
        private GameObject backgroundTile = null;
        private GameObject wallTile = null;

        // Public
        public const float updateRate = 0.15f;
        public const int gameWidth = 32;
        public const int gameHeight = 20;        

        // Methods
        public void Start()
        {
            // Load assets
            snakeTile = Resources.Load<GameObject>("Snake");
            foodTile = Resources.Load<GameObject>("food");
            backgroundTile = Resources.Load<GameObject>("background");
            wallTile = Resources.Load<GameObject>("wall");

            // Initialize map
            map = new TileType[gameWidth, gameHeight];

            BuildMap();
            BuildSnake();
            CreateFood();
        }

        public void Update()
        {
            if (gameOver == true)
                return;

            // Inputs
            if (Input.GetKey(KeyCode.UpArrow) == true) currentDirection = SnakeDirection.Up;
            else if (Input.GetKey(KeyCode.DownArrow) == true) currentDirection = SnakeDirection.Down;
            else if (Input.GetKey(KeyCode.LeftArrow) == true) currentDirection = SnakeDirection.Left;
            else if (Input.GetKey(KeyCode.RightArrow) == true) currentDirection = SnakeDirection.Right;

            // Move snake
            if (Time.time > moveTimer + updateRate)
            {
                MoveSnake(currentDirection);
                moveTimer = Time.time;
            }
        }

        private void MoveSnake(SnakeDirection direction)
        {
            Vector2Int offset = new Vector2Int();

            switch (direction)
            {
                case SnakeDirection.Up: offset = new Vector2Int(0, 1); break;
                case SnakeDirection.Down: offset = new Vector2Int(0, -1); break;
                case SnakeDirection.Left: offset = new Vector2Int(-1, 0); break;
                case SnakeDirection.Right: offset = new Vector2Int(1, 0); break;
            }

            // Get target index
            Vector2Int targetIndex = snakeBody[0] + offset;

            // Check for wall
            if (map[targetIndex.x, targetIndex.y] == TileType.Wall || snakeBody.Contains(targetIndex) == true)
            {
                gameOver = true;
                Debug.Log("Game Over!");
                return;
            }

            // Check for food
            if (map[targetIndex.x, targetIndex.y] == TileType.Food)
            {
                // Collect the food
                map[targetIndex.x, targetIndex.y] = TileType.Empty;
                Destroy(foodInstance);

                // Grow snake
                snakeBody.Add(snakeBody[snakeBody.Count - 1]);
                snakeTiles.Add(Instantiate(snakeTile, IndexToPosition(snakeBody[snakeBody.Count - 1]), Quaternion.identity, transform));

                CreateFood();
            }

            // Move snake       
            for (int i = snakeBody.Count - 1; i >= 1; i--)
            {
                snakeBody[i] = snakeBody[i - 1];
                snakeTiles[i].transform.position = IndexToPosition(snakeBody[i]);
            }

            snakeBody[0] += offset;
            snakeTiles[0].transform.position = IndexToPosition(snakeBody[0]);
        }

        private void CreateFood()
        {
            Vector2Int index = new Vector2Int();
            bool validIndex = false;

            while (validIndex == false)
            {
                index = new Vector2Int(Random.Range(0, gameWidth), Random.Range(0, gameHeight));

                if (map[index.x, index.y] == TileType.Empty && snakeBody.Contains(index) == false)
                    validIndex = true;
            }

            map[index.x, index.y] = TileType.Food;
            foodInstance = Instantiate(foodTile, IndexToPosition(index), Quaternion.identity, transform);
        }

        private void BuildMap()
        {
            for (int x = 0; x < gameWidth; x++)
            {
                for (int y = 0; y < gameHeight; y++)
                {
                    map[x, y] = TileType.Empty;

                    if (x == 0 || y == 0 || x == gameWidth - 1 || y == gameHeight - 1)
                        map[x, y] = TileType.Wall;

                    // Build tile
                    GameObject tile = Instantiate((map[x, y] == TileType.Wall) ? wallTile : backgroundTile);

                    // Set parent and position
                    tile.transform.SetParent(transform);
                    tile.transform.localPosition = IndexToPosition(new Vector2Int(x, y), 1f);
                }
            }
        }

        private void BuildSnake()
        {
            int x = gameWidth / 2;
            int y = gameHeight / 2;

            for (int i = 0; i < 5; i++)
            {
                snakeBody.Add(new Vector2Int(x, y));
                snakeTiles.Add(Instantiate(snakeTile, IndexToPosition(new Vector2Int(x, y)), Quaternion.identity, transform));
                x++;
            }
        }

        private Vector3 IndexToPosition(Vector2Int index, float z = 0)
        {
            return new Vector3(-(gameWidth / 2) + index.x, -(gameHeight / 2) + index.y, z);
        }
    }
}
