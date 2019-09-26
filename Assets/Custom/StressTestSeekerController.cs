using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Pathfinding
{
    public class StressTestSeekerController : MonoBehaviour
    {
        private Seeker seeker;
        public TextMeshProUGUI ResultText;

        private int _totalRun;
        private float _totalRunMs;

        // Start is called before the first frame update
        void Start()
        {
            seeker = GetComponent<Seeker>();
        }

        public void ForceSeek(Vector3 start, Vector3 end)
        {
            seeker.StartPath(start, end, Callback);
        }

        private void Callback(Path p)
        {
            _totalRun++;
            _totalRunMs += p.duration;
            ResultText.text = $"total run {_totalRun} times. avg time is {_totalRunMs / _totalRun} ms! ";
        }
    }


}
