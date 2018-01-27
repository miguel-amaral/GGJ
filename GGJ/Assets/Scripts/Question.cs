using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {


    public class Question : MonoBehaviour {
        public IQuestionBallotState State { get; set; } 
        private int _nearTeacher = 0;

        private float RemainingTimeForDisappear;
        private float MaxTimeQuestionDisappear;

        public GameObject GREEN_QUESTION;
        public GameObject YELLOW_QUESTION;
        public GameObject RED_QUESTION;

        public int TimeGreenToYellow = 3;
        public int TimeYellowToRed   = 3;
        public int TimeRedToBust     = 3;

        public int TimeToRemoveDoubt = 2;
        private bool _questioning = false;

        public Question(float timeForQuestionDisappear)
        {
            RemainingTimeForDisappear = timeForQuestionDisappear;
            MaxTimeQuestionDisappear = timeForQuestionDisappear;
        }

        public void Start()
        {
            State = new NoQuestion();
            this.UpdateStateObjs();
        }
        public void Update()
        {
            var deltaTime = Time.deltaTime;
            if (_nearTeacher > 0)
            {
                this.TeacherIsHelping(deltaTime);
            }
            else
            {
                this.TeacherIsAway(deltaTime);
            }
            UpdateVisualQuestion();
        }



        private void TeacherIsHelping(float deltaTime)
        {
            
            RemainingTimeForDisappear -= deltaTime;
            if (RemainingTimeForDisappear < 0)
            {
                QuestionSolved();
            }
        }
        private void TeacherIsAway(float deltaTime)
        {
            RemainingTimeForDisappear += deltaTime;
            if (RemainingTimeForDisappear > MaxTimeQuestionDisappear)
            {
                RemainingTimeForDisappear = MaxTimeQuestionDisappear;
            }
            State.TeacherIsAway(deltaTime, this);
        }

        public void ActivateQuestion()
        {
            _questioning = true;
            RemainingTimeForDisappear = MaxTimeQuestionDisappear;
            State = new GreenQuestion();
            UpdateStateObjs();
        }
        private void UpdateVisualQuestion() {
            //Scale
        }
        private void QuestionSolved() {
            Debug.Log("Question Solved");
            this.StopQuestion();
            //throw new NotImplementedException();
        }

        private void StopQuestion()
        {
            this.State.DeactivateGameObj();
            this.State = new NoQuestion();
            _questioning = false;
            //throw new NotImplementedException();
        }

        public void QuestionTimeUp()
        {
            Debug.Log("Question Time Up");
            this.StopQuestion();
            //throw new NotImplementedException();
        }
        public void OnTriggerEnter(Collider col) {
            if (col.gameObject.tag.Equals("Professor")) {
                _nearTeacher++;
            }
        }

        public void OnTriggerExit(Collider col) {
            if (col.gameObject.tag.Equals("Professor")) {
                _nearTeacher--;
            }
        }

        public void UpdateStateObjs()
        {
            State.DefineTime(TimeGreenToYellow, TimeYellowToRed, TimeRedToBust);
            State.DefineGameObjects(GREEN_QUESTION, YELLOW_QUESTION, RED_QUESTION);
        }

        public bool CanQuestion()
        {
            return !_questioning;
        }
    }
}
