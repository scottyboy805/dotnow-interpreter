using UnityEngine;

namespace SpaceDemo
{
    public class GameManager : MonoBehaviour
    {
        // Private
        private float spawnTimer = 0;

        // Public
        public Bounds worldBounds = new Bounds(Vector3.zero, new Vector3(20, 16, 0));
        public Ship ship;
        public Asteroid[] asteroids;
        public float spawnFrequency = 2;

        // Methods
        public void Update()
        {
            if(Time.time > spawnTimer + spawnFrequency)
            {
                // Spawn an item
                SpawnAsteroid();

                // Reset timer
                spawnTimer = Time.time;
            }
        }

        private void SpawnAsteroid()
        {
            // Select random asteroid
            Asteroid spawned = Instantiate(asteroids[Random.Range(0, asteroids.Length)]);

            // Random chance of spawn location
            float spawnSide = Random.Range(0f, 1f);

            // Set position
            if(spawnSide < 0.25f)
            {
                spawned.transform.position = new Vector3(Random.Range(worldBounds.min.x, worldBounds.max.x), worldBounds.max.y);
            }
            else if(spawnSide < 0.5f)
            {
                spawned.transform.position = new Vector3(Random.Range(worldBounds.min.x, worldBounds.max.x), worldBounds.min.y);
            }
            else if(spawnSide < 0.75f)
            {
                spawned.transform.position = new Vector3(worldBounds.min.x, Random.Range(worldBounds.min.y, worldBounds.max.y));
            }
            else
            {
                spawned.transform.position = new Vector3(worldBounds.max.x, Random.Range(worldBounds.min.y, worldBounds.max.y));
            }

            // Get direction to ship
            Vector3 direction = ((ship.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f)) - spawned.transform.position));

            // Set random direction
            spawned.Direction = direction.normalized;

            // Randomize movement 
            spawned.Randomize();
        }
    }
}
