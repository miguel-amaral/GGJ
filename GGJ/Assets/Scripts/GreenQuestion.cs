using UnityEngine;

namespace Assets.Scripts
{
    public class GreenQuestion : IQuestionBallotState
    {
        public override void TeacherIsAway(float deltaTime, Question question)
        {
            timeToNext -= deltaTime;
            if (timeToNext <= 0)
            {
                myObject.SetActive(false);
                question.State = new YellowQuestion();
                question.UpdateStateObjs();
                
            }
        }

        public override void DefineTime(int green, int yellow, int red)
        {
            timeToNext = green;
        }

        public override void DefineGameObjects(GameObject green, GameObject yellow, GameObject red)
        {
            myObject = green;
            myObject.SetActive(true);
        }
    }
}