using UnityEngine;

namespace Assets.Scripts
{
    public class NoQuestion : IQuestionBallotState
    {
        public override void TeacherIsAway(float deltaTime, Question question){}
        public override void DefineTime(int green, int yellow, int red){}

        public override void DefineGameObjects(GameObject green, GameObject yellow, GameObject red)
        {
            myObject = green; 
        }
    }
}