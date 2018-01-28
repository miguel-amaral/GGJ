﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {


    public class Question : MonoBehaviour {
        public IQuestionBallotState State { get; set; } 
        private int _nearTeacher = 0;

        private float RemainingTimeForDisappear;
        public float MaxTimeQuestionDisappear;



        //public GameObject gameManagerOBJ;
        private GameManager gameManager;

        public GameObject GREEN_QUESTION;
        public GameObject YELLOW_QUESTION;
        public GameObject RED_QUESTION;

        public float speed;

        public int TimeGreenToYellow = 3;
        public int TimeYellowToRed   = 3;
        public int TimeRedToBust     = 3;

//        public int TimeToRemoveDoubt 2 = 2;
        private bool _questioning = false;

        public AudioSource [] sfx = new AudioSource [2];

        void Start()
        {
            State = new NoQuestion();
            this.UpdateStateObjs();

            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null) {
                Debug.LogError("Question " + this.gameObject.name + "Without Game Manager");
            }

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


        private void UpdateVisualQuestion()
        {
            var percentage = RemainingTimeForDisappear / MaxTimeQuestionDisappear;
            this.gameObject.transform.localScale = new Vector3(percentage,percentage,percentage);
            //State.UpdateSizeQuestion(percentage);

            this.gameObject.transform.Rotate(Vector3.up, speed * Time.deltaTime);
                
        }
        private void TeacherIsHelping(float deltaTime)
        {
            
            RemainingTimeForDisappear -= deltaTime;
            if (_questioning && RemainingTimeForDisappear < 0 )
            {
                QuestionSolved();
            }
          
        }
        private void TeacherIsAway(float deltaTime)
        {
            RemainingTimeForDisappear += (deltaTime/3);
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
            this.transform.localScale = new Vector3(1, 1, 1);
            gameManager.ActivateQuestion();

  
            sfx[0].Play();
        }

        private void QuestionSolved()
        {
            this.gameManager.QuestionSolved();
            this.StopQuestion();
            //throw new NotImplementedException();
        }

        private void StopQuestion()
        {
            this.State.DeactivateGameObj();
            this.State = new NoQuestion();
            _questioning = false;
            gameManager.StopQuestions();
            //throw new NotImplementedException();
           
            sfx[1].Play();
        }

        public void QuestionTimeUp()
        {
            this.gameManager.QuestionTimeUp();
            this.StopQuestion();
            
            //throw new NotImplementedException();
        }
        public void OnTriggerEnter(Collider col) {
            if (col.gameObject.tag.Equals("RangeAnswerQuestionCollider")) {
                _nearTeacher++;
            }
        }

        public void OnTriggerExit(Collider col) {
            if (col.gameObject.tag.Equals("RangeAnswerQuestionCollider")) {
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
            return !_questioning && gameManager.CanQuestion();
        }

        public bool Questioning()
        {
            return _questioning;
        }
    }
}
