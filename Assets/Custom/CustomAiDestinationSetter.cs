using UnityEngine;
using System.Collections;

namespace Pathfinding
{
    internal class CustomAiDestinationSetter : VersionedMonoBehaviour
    {
        private GameController gameController;
        public static int Amount;

        /// <summary>The object that the AI should move to</summary>
        public Vector3 destination;

        private IAstarAI ai;

        private void OnEnable()
        {
            ai = GetComponent<IAstarAI>();
            ai.canSearch = true;
            if (ai != null) ai.onSearchPath += Update;
            gameController = FindObjectOfType<GameController>();

            Amount++;
        }

        private void OnDisable()
        {
            if (ai != null) ai.onSearchPath -= Update;

            Amount--;
        }

        private void Start()
        {
            SetDestination();
        }

        private void Update()
        {
            if (ai.reachedEndOfPath)
            {
                ai.canSearch = true;
                SetDestination();
            }
            else
            {
                ai.canSearch = false;
            }
        }

        private void SetDestination()
        {
            destination = gameController.OpenSpaces[Random.Range(0, gameController.OpenSpaces.Count)];
            ai.destination = destination;
        }
    }
}