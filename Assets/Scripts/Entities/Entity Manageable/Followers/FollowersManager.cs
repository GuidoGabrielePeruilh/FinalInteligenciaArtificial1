using IA_I.EntityNS.Manegeable;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.EntityNS.Follower
{
    public class FollowersManager : Singleton<FollowersManager>
    {
        public List<FollowersEntities> AllFollowers { get; private set; }

        public float ViewRadius => _viewRadius * _viewRadius;
        public float ArriveRadius => _arriveRadius * _arriveRadius;
        public float SeparationRadius => _separationRadius * _separationRadius;
        public float AlignmentRadius => _alignmentRadius * _alignmentRadius;
        public float CohesionRadius => _cohesionRadius * _cohesionRadius;

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

        new private void Awake()
        {
            itDestroyOnLoad = true;
            base.Awake();
            AllFollowers = new List<FollowersEntities>();
        }

        public void RegisterNewFollower(FollowersEntities newFollower, ManageableEntities leaderOwner)
        {
            if (!AllFollowers.Contains(newFollower))
            {
                AllFollowers.Add(newFollower);
                leaderOwner.AddFollower(newFollower);
            }
        }

        public void RemoveFollower(FollowersEntities followerToRemove, ManageableEntities leaderOwner)
        {
            if (AllFollowers.Contains(followerToRemove))
            {
                AllFollowers.Remove(followerToRemove);
                leaderOwner.RemoveFollower(followerToRemove);
            }
        }
    }
}

