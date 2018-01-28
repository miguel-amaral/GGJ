using UnityEngine;

namespace Assets.Scripts
{
    public class YellowQuestion : IQuestionBallotState {
        public override void TeacherIsAway(float deltaTime, Question question) {
            timeToNext -= deltaTime;
            if (timeToNext <= 0) {
                myObject.SetActive(false);
                question.State = new RedQuestion();
                question.UpdateStateObjs();
            }
        }

        public override void DefineTime(int green, int yellow, int red) {
            timeToNext = yellow;
        }

        public override void DefineGameObjects(GameObject green, GameObject yellow, GameObject red) {
            myObject = yellow;
            myObject.SetActive(true);

        }
    }
}