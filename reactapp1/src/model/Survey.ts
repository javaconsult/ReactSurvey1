import { Question } from "./Question";
//string properties will not bind to guid properties in controller method if they are not set as a guid
import { NIL } from 'uuid'; //https://www.npmjs.com/package/uuid 
import { SurveyFormValues } from "../state/SurveyStore";

export class Survey  {
    id: string = NIL;
    name?: string;
    description?:string;
    questions?: Question[] = [];
    participantCount?: number;
    participantEmail?: string;

    constructor(surveyFormValues?: SurveyFormValues){
        //copy properties from surveyFormValues to current object
        Object.assign(this,surveyFormValues);
        //debugger;
    }
}



//npm install uuid
//npm i --save-dev @types/uuid

