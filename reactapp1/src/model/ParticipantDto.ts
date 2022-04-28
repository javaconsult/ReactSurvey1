import { SelectionDto } from "./SelectionDto";

//string properties will not bind to guid properties in controller method if they are not set as a guid
import { NIL } from 'uuid'; //https://www.npmjs.com/package/uuid

export class ParticipantDto {
    id?:string = NIL; 
    surveyId?:string = NIL;
    name?:string;
    email?: string;
    constructor(surveyId?:string, name?:string, email?: string){
        this.surveyId = surveyId;
        this.name=name;       
        this.email=email;
    }
    selections?:SelectionDto[] = [];
}  