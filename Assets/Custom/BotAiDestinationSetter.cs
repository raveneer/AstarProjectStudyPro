using UnityEngine;
using System.Collections;

namespace Pathfinding
{
    class BotAiDestinationSetter : VersionedMonoBehaviour
    {
        public MeshRenderer BotMeshRender;
        GameController gameController;
        /// <summary>The object that the AI should move to</summary>
        public Vector3 destination;
        IAstarAI ai;

        void OnEnable()
        {
            ai = GetComponent<IAstarAI>();
            ai.canSearch = true;
            if (ai != null) ai.onSearchPath += Update;
            gameController = FindObjectOfType<GameController>();
        }

        void OnDisable()
        {
            if (ai != null) ai.onSearchPath -= Update;
        }

        void Start()
        {
            SetDestination();
        }

        /// <summary>Updates the AI's destination every frame. only recalc when destination reached (for performnace)</summary>
        void Update()
        {
            if (ai.reachedEndOfPath) 
            {
                ai.canSearch = true;
                SetDestination();
                BotMeshRender.material.color = Color.green;
            }
            else
            {
                ai.canSearch = false;
                BotMeshRender.material.color = Color.white;
            }
        }

        //빈칸으로 목표설정 한다.
        void SetDestination()
        {
            destination = gameController.OpenSpaces[Random.Range(0, gameController.OpenSpaces.Count)];
            ai.destination = destination;
        }

    }
}
