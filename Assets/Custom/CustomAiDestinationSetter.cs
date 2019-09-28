using UnityEngine;
using System.Collections;

namespace Pathfinding
{
    internal class CustomAiDestinationSetter : VersionedMonoBehaviour
    {
        private GameController gameController;
        public static int Amount;
        public GameObject HighIQIndicator;

        //에이전트가 어딘가에 막히거나 등 이런 저런 이유로 현재의 패스를 따라가는데 문제가 있음을 의미.
        private bool _isPathHasTrouble;

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

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log($"triggerEnter! {other.gameObject.name}");
            _isPathHasTrouble = true;
        }

        private void Start()
        {
            SetDestination();
        }

        //목적지에 도달하면 안전상태가 된다. 안전상태는 ai가 실시간 검색을 하지 않으므로 부하가 적다.
        //어딘가에 부딪히거나 하면 비 안전상태가 되는데, 이때 ai는 목적지에 도달할 때 까지 실시간 검색을 켠다. (지능이 올라감)
        //이렇게 하여 부하를 줄이면서 지능적인 경로를 따라갈 수 있다.
        private void Update()
        {
            if (ai.reachedEndOfPath)
            {
                ai.canSearch = true;
                _isPathHasTrouble = false;
                SetDestination();
            }
            else if (_isPathHasTrouble)
            {
                ai.canSearch = true;
            }
            else
            {
                ai.canSearch = false;
            }

            //고급 인공지능 표시 기능
            HighIQIndicator.SetActive(ai.canSearch);
        }

        private void SetDestination()
        {
            destination = gameController.OpenSpaces[Random.Range(0, gameController.OpenSpaces.Count)];
            ai.destination = destination;
        }
    }
}