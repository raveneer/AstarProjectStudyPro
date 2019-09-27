using UnityEngine;
using System.Collections;

namespace Pathfinding
{
    //ai 마다 1개씩 붙어야 함. 따라서 ai 개수와 같다.
    internal class CustomAiDestinationSetter : VersionedMonoBehaviour
    {
        private GameController gameController;
        public static int CalculatingNotFinishedPaths;
        public static int CustomAiDestinationSetterAmount;

        /// <summary>The object that the AI should move to</summary>
        public Vector3 destination;

        private IAstarAI ai;

        private void OnEnable()
        {
            ai = GetComponent<IAstarAI>();
            ai.canSearch = true;
            if (ai != null) ai.onSearchPath += Update;
            gameController = FindObjectOfType<GameController>();

            CustomAiDestinationSetterAmount++;
        }

        private void OnDisable()
        {
            if (ai != null) ai.onSearchPath -= Update;

            CustomAiDestinationSetterAmount--;
        }

        private void Start()
        {
            SetDestination();
        }

        /// <summary>Updates the AI's destination every frame. only recalc when destination reached (for performnace)</summary>
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

        //빈칸으로 목표설정 한다.
        private void SetDestination()
        {
            destination = gameController.OpenSpaces[Random.Range(0, gameController.OpenSpaces.Count)];
            ai.destination = destination;
        }
    }
}