export class SelectionDto {
    questionId?: number;
    selectedOptionId?: number;
    constructor(questionId: string, seletedOptionId: string) {
        if (questionId && seletedOptionId) {
            this.questionId = parseInt(questionId);
            this.selectedOptionId = parseInt(seletedOptionId);
        }
    }
}