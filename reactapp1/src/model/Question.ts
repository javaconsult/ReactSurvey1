import { Option } from "./Option";

export class Question {
    id:string='';
    questionNumber?: number;
    heading?: string;
    text?: string;    
    options?: Option[] = [];

    constructor(questionFormValues: QuestionFormValues, questionNumber: number) {
        console.log(questionFormValues.option1);
        this.id='0';
        this.questionNumber = questionNumber;
        this.heading = questionFormValues.heading;
        this.text = questionFormValues.text;
        if (questionFormValues?.option1)
            this.options?.push(new Option(questionFormValues.option1))
        if (questionFormValues?.option2)
            this.options?.push(new Option(questionFormValues.option2))
        if (questionFormValues?.option3)
            this.options?.push(new Option(questionFormValues.option3))
        if (questionFormValues?.option4)
            this.options?.push(new Option(questionFormValues.option4))
        if (questionFormValues?.option5)
            this.options?.push(new Option(questionFormValues.option5))
        if (questionFormValues?.option6)
            this.options?.push(new Option(questionFormValues.option6))
    }
}


export class QuestionFormValues {
    heading?: string = '';
    text?: string = '';
    option1?: string = '';
    option2?: string = '';
    option3?: string = '';
    option4?: string = '';
    option5?: string = '';
    option6?: string = '';
}