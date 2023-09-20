using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.EntityNS.Follower
{
    public class FollowersManager : MonoBehaviour
    {
        public static FollowersManager Instance { get; private set; }

        public List<FollowersEntities> AllFollowers { get; private set; }

        public float ViewRadius
        {
            get
            {
                return _viewRadius * _viewRadius;
            }
        }

        public float ArriveRadius
        {
            get
            {
                return _arriveRadius * _arriveRadius;
            }
        }

        public float SeparationRadius
        {
            get
            {
                return _separationRadius * _separationRadius;
            }
        }

        public float AlignmentRadius
        {
            get
            {
                return _alignmentRadius * _alignmentRadius;
            }
        }

        public float CohesionRadius
        {
            get
            {
                return _cohesionRadius * _cohesionRadius;
            }
        }

        [SerializeField] float _viewRadius;
        [SerializeField] float _arriveRadius;
        [SerializeField] float _separationRadius;
        [SerializeField] float _alignmentRadius;
        [SerializeField] float _cohesionRadius;

        [field: SerializeField, Range(0f, 2.5f)]
        public float SeparationWeight { get; private set; }

        [field: SerializeField, Range(0f, 2.5f)]
        public float AlignmentWeight { get; private set; }

        [field: SerializeField, Range(0f, 2.5f)]
        public float CohesionWeight { get; private set; }

        void Awake()
        {
            Instance = this;

            AllFollowers = new List<FollowersEntities>();
        }

        public void RegisterNewFollower(FollowersEntities newFollower)
        {
            if (!AllFollowers.Contains(newFollower))
                AllFollowers.Add(newFollower);
        }

        public void RemoveFollower(FollowersEntities followerToRemove)
        {
            if (AllFollowers.Contains(followerToRemove))
                AllFollowers.Remove(followerToRemove);
        }
    }
}

