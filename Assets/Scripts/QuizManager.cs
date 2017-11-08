using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour {

    public Question[] questions;
    [Range(0, 1)]
    public float passingGrade;
    
    //lrs params
    float grade;
    bool passedQuiz;

   
    [HideInInspector]
    public int questionsAnsweredCorrectly = 0;
    [HideInInspector]
    public int totalQuestions;

    int questionIndex;

    Text questionText;
    Text passFailText;
    Text resultsText;
    Text attemptsText;

    GameObject multipleChoiceResponses;
    GameObject checkAllThatApplyResponses;
    GameObject currentResponseObject;
    GameObject submitResponseButton;

    Question currentQuestion;

	// Use this for initialization
	void Start () {
        //increment attempts integer
        if(FindObjectOfType<LrsCommunicator>() != null){
            FindObjectOfType<LrsCommunicator>().attempts++;
        }

        multipleChoiceResponses = GameObject.Find("Multiple Choice Responses");
        checkAllThatApplyResponses = GameObject.Find("Check All That Apply Responses");
        questionText = GameObject.Find("Question").GetComponent<Text>();
        submitResponseButton = GameObject.Find("SubmitButton");
        passFailText = GameObject.Find("Pass/Fail Text").GetComponent<Text>();
        resultsText = GameObject.Find("Results Text").GetComponent<Text>();
        attemptsText = GameObject.Find("Attempts Text").GetComponent<Text>();
        
        passFailText.gameObject.SetActive(false);
        resultsText.gameObject.SetActive(false);
        attemptsText.gameObject.SetActive(false);

        totalQuestions = questions.Length;
		GenerateQuestion();

	}

    void Update(){
        if(HasAnswered(currentResponseObject)){
            submitResponseButton.GetComponent<Button>().interactable = true;
        } else {
            submitResponseButton.GetComponent<Button>().interactable = false;
        }
    }

    void GenerateQuestion(){
        //set the current question equal to the question in questions array
        currentQuestion = questions[questionIndex];

        //set the question
        questionText.text = currentQuestion.question;

        //display multiple choice or check all that apply responses
        if(currentQuestion.isMultipleChoice){
            multipleChoiceResponses.SetActive(true);
            checkAllThatApplyResponses.SetActive(false);
            currentResponseObject = multipleChoiceResponses;
            GenerateResponses(multipleChoiceResponses);
        } else if (!currentQuestion.isMultipleChoice){
            multipleChoiceResponses.SetActive(false);
            checkAllThatApplyResponses.SetActive(true);
            currentResponseObject = checkAllThatApplyResponses;
            GenerateResponses(checkAllThatApplyResponses);
        }

    }

    void GenerateResponses(GameObject responseContainer){
        for(int i = 0; i < responseContainer.transform.childCount; i ++){
            responseContainer.transform.GetChild(i).Find("Label").GetComponent<Text>().text = currentQuestion.responses[i];
        }
    }

    public void SubmitResponse(){
        //validate user response based on correct response key
        if(HasAnsweredCorrectly(currentResponseObject)){
            questionsAnsweredCorrectly++;
        }

        //increment question index and generate next question
        if(questionIndex < questions.Length - 1){
            questionIndex ++;
            //turn off toggles in preparation for next question
            for(int i = 0; i < currentResponseObject.transform.childCount; i++){
                currentResponseObject.transform.GetChild(i).GetComponent<Toggle>().isOn = false;
            }
            GenerateQuestion();
        } else {
            //enter code for when you complete the quiz!
            FinishQuiz();
            if(FindObjectOfType<LrsCommunicator>() != null){
                FindObjectOfType<LrsCommunicator>().score = grade;
                FindObjectOfType<LrsCommunicator>().ReportScore();
                FindObjectOfType<LrsCommunicator>().ReportArTime();
                FindObjectOfType<LrsCommunicator>().ReportNonArTime();
                FindObjectOfType<LrsCommunicator>().wasSuccessful = passedQuiz;
            }
        }
    } 

    void FinishQuiz(){
        questionText.gameObject.SetActive(false);
        currentResponseObject.SetActive(false);
        submitResponseButton.SetActive(false);

        passFailText.gameObject.SetActive(true);
        resultsText.gameObject.SetActive(true);
        attemptsText.gameObject.SetActive(true);

        //display pass/fail text
        grade = (float)questionsAnsweredCorrectly/(float)totalQuestions;
        if(grade >= passingGrade){
            passFailText.text = "Congratulations! You passed!";
            passedQuiz = true;
        } else if (grade < passingGrade){
            passFailText.text = "You must retake the quiz.";
            passedQuiz = false;
        }
        
        //display results text
        if(questionsAnsweredCorrectly != 1){
            resultsText.text = "You answered " + questionsAnsweredCorrectly + " questions correctly out of " + totalQuestions + ".";
        } else if (questionsAnsweredCorrectly == 1){
            resultsText.text = "You answered " + questionsAnsweredCorrectly + " question correctly out of " + totalQuestions + ".";
        }

        //display attempts text
        if(FindObjectOfType<LrsCommunicator>() != null){
            if(FindObjectOfType<LrsCommunicator>().attempts != 1){
                attemptsText.text = "You've attempted this quiz " + FindObjectOfType<LrsCommunicator>().attempts + " times.";
            } else if (FindObjectOfType<LrsCommunicator>().attempts == 1){
                attemptsText.text = "You've attempted this quiz " + FindObjectOfType<LrsCommunicator>().attempts + " time.";
            }
        } else {
            attemptsText.text = "You've attempted this quiz 1 time.";
        }
    }

    bool HasAnswered(GameObject responseContainer){
        for(int i = 0; i < responseContainer.transform.childCount; i ++){
            if(responseContainer.transform.GetChild(i).GetComponent<Toggle>().isOn){
                return true;
            }
        }
        return false;
    }

    //Check user responses with the correct response key
    bool HasAnsweredCorrectly(GameObject responseContainer){
        for(int i = 0; i < responseContainer.transform.childCount; i++){
            if(responseContainer.transform.GetChild(i).GetComponent<Toggle>().isOn && currentQuestion.correctResponseKey[i]){
                continue;
            } else if(!responseContainer.transform.GetChild(i).GetComponent<Toggle>().isOn && currentQuestion.correctResponseKey[i] || responseContainer.transform.GetChild(i).GetComponent<Toggle>().isOn && !currentQuestion.correctResponseKey[i]){
                Debug.Log("answered incorrectly!");
                return false;
            }
        }
        Debug.Log("answered correctly!");
        return true;
    }
    

    [System.Serializable]
    public class Question{
        public string question;
        public string[] responses;
        public bool isMultipleChoice;
        public bool[] correctResponseKey;
    }
}
