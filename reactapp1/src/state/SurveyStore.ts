import agent from "../api/Agent";
import { Question, QuestionFormValues } from "../model/Question";
import { Survey } from "../model/Survey";

export interface SurveyFormValues {
    name: string;
    description: string;
}

export default class SurveyStore {

    //view state in Components tab in Chrome (select Context.Provider)
    survey: Survey | undefined; // state
    questionNumber:number = 1;
    
    createSurvey = (surveyFormValues: SurveyFormValues) => {
        //build Survey instance from form values
        this.survey = new Survey(surveyFormValues)
    }

    addQuestion = (questionFormValues: QuestionFormValues) => {
        //https://www.w3schools.com/js/js_debugging.asp
        let question = new Question(questionFormValues, this.questionNumber);
        this.survey?.questions?.push(question); //push question onto survey object's array of questions
        this.questionNumber+=1;
    }

     uploadSurvey = async () => {
        let isSurveyValid = !(this.survey === undefined || this.survey.questions === undefined || this.survey.questions.length < 1);
        isSurveyValid && await agent.Surveys.create(this.survey);
        return isSurveyValid;          
    }
}
