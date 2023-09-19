using IA_I.Entity.Manegeable;
using UnityEngine;

namespace IA_I.Inputs
{
    public class Inputs : MonoBehaviour
    {
        [SerializeField] Camera holographicCamera;
        [SerializeField] ManageableEntities myEntityLeftClick;
        [SerializeField] ManageableEntities myEntityRightClick;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetClickPosition(myEntityLeftClick);
            }

            if (Input.GetMouseButtonDown(1))
            {
                GetClickPosition(myEntityRightClick);
            }
        }

        private void GetClickPosition(ManageableEntities entityToMove)
        {
            Ray ray = holographicCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                entityToMove.UpdateTargetPosition(targetPosition);
            }
        }
    }
}

