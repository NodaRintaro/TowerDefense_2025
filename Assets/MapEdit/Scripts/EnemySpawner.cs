using UnityEngine;

namespace TD.Game
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform _enemyRoot;

        private RouteRepository _routeRepo;

        public void Initialize(RouteRepository routeRepo)
        {
            _routeRepo = routeRepo;
        }

        public void Spawn(GameObject enemyPrefab, string routeId, int spawnIndex = -1, int goalIndex = -1)
        {
            if (!_routeRepo.TryGet(routeId, out var route))
            {
                Debug.LogError($"Route not found: {routeId}");
                return;
            }

            if (route.SpawnCount == 0 || route.GoalCount == 0)
            {
                Debug.LogError($"Route({routeId}) has no spawn/goal.");
                return;
            }

            // -1ならランダム
            if (spawnIndex < 0) spawnIndex = Random.Range(0, route.SpawnCount);
            if (goalIndex  < 0) goalIndex  = Random.Range(0, route.GoalCount);

            spawnIndex = Mathf.Clamp(spawnIndex, 0, route.SpawnCount - 1);
            goalIndex  = Mathf.Clamp(goalIndex,  0, route.GoalCount  - 1);

            var points = route.BuildPoints(spawnIndex, goalIndex);

            var go = Instantiate(enemyPrefab, _enemyRoot);
            var mover = go.GetComponent<EnemyMover>();
            if (mover == null) mover = go.AddComponent<EnemyMover>();

            mover.SetPoints(points);
        }


    }
}