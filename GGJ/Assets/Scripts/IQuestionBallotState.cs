using UnityEngine;

namespace Assets.Scripts
{
    public abstract class IQuestionBallotState
    {
        protected GameObject myObject;
        protected float timeToNext;

        public abstract void TeacherIsAway(float deltaTime, Question question);
        public abstract void DefineTime(int green, int yellow, int red);
        public abstract void DefineGameObjects(GameObject green, GameObject yellow, GameObject red);

        
        public GameObject CurrentQuestionMark()
        {
            return myObject;
        }

        public void DeactivateGameObj()
        {
            if (myObject != null)
            {
                myObject.SetActive(false);
            }
            else
            {
                Debug.LogError("OOPS AGAIN Deactivate Game Obj NULL");
            }
        }

        public void UpdateSizeQuestion(float percentage)
        {
            myObject.transform.localScale = new Vector3(percentage, percentage, percentage);
        }
    }
}