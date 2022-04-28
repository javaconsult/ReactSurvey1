import { useEffect, useState } from "react";
import agent from "../api/Agent";
import { Survey } from "../model/Survey";
import { Container } from 'react-bootstrap';
import { useParams } from "react-router-dom";
import { ParticipantDto } from "../model/ParticipantDto";
import { SelectionDto } from "../model/SelectionDto";
import { useNavigate } from "react-router-dom";

export default function ParticipantForm() {
    const [survey, setSurvey] = useState<Survey>(); //survey retrieved from api in useEffect. Used to generate form with questions and options
    const { id } = useParams();
    const participantId = id as string;
    const [selections] = useState<Map<string, string>>(new Map<string, string>());
    const[submitDisabled, setSubmitDisabled] = useState(true);
    const navigate = useNavigate();
    //runs once after render - gets survey for participant
    useEffect(() => {
        const loadSurvey = async () => {
            try {
                //call api to get survey for completion by participant
                //this causes a 409 conflict if the participant has already completed the survey
                const surveyFromApi: Survey = await agent.Participant.get(participantId);

                //set state. This causes re-render of all components
                setSurvey(surveyFromApi);
            } catch (error: any) {
                const httpCode = error.response.status;
                let errorMessage: string;
                switch (httpCode) {
                    case 400: //api method not found due to malformed guid                        
                        errorMessage = 'This survey was not found';
                        break;
                    case 404: //participant not registered for survey
                        errorMessage = 'You have not been registered to participate in this survey';
                        break
                    case 409: //participant has previously completed survey
                        const surveyDate = new Date(error.response.data)
                        errorMessage = `You previously completed this survey on ${surveyDate.toDateString()} at ${surveyDate.getHours()}:${surveyDate.getMinutes() < 10 ? '0' : ''}${surveyDate.getMinutes()}`
                        break;
                    default:
                        errorMessage = error.response;
                }
                navigate(`/fail/${errorMessage}`)
            }
        }
        loadSurvey();
    }, [participantId, navigate]
    )

    const select = (questionId: string, optionId: string | undefined) => {
        selections?.set(questionId, optionId as string);
        console.log(selections);
            
        const questionCount = survey?.questions!== undefined ? survey?.questions?.length : 0;
        setSubmitDisabled(selections.size < questionCount);
    }

    async function handleSubmit(event: any) {
        event.preventDefault();
        try {
            //survey participant 
            const participant = new ParticipantDto();
            participant.id = participantId;

            //add selected options to participantDto
            selections.forEach((optionId, questionId) => participant.selections?.push(new SelectionDto(questionId, optionId)))

            //the agent uploads the participant's responses to the api
            await agent.Participant.upload(participant);
            //display confirm page, configured in App.tsx
            navigate('/confirm')
        } catch (error) {
            console.log(error)
            navigate(`/fail/${error}`)
        }
    }

    return (
        <Container>
            <h1>{survey?.name}</h1>
            <h4>{survey?.description}</h4>
            <form onSubmit={handleSubmit}>
                {survey?.questions?.map(question => {
                    //for each question
                    return (
                        <div key={question.id}>
                            <b>{question.heading}</b>
                            <br />{question.text}
                            {
                                question.options?.map(option => {
                                    //for each option
                                    return (<div key={option.id} >
                                        <input onChange={(e) => select(question.id, option.id)} name={question.id} id={option.id} value={option.id} type='radio' />
                                        <label htmlFor={option.id}>&nbsp; {option.text}</label>
                                    </div>)

                                })
                            }
                            <br />
                        </div>
                    )

                }
                )
                }
                <input type="submit" disabled={submitDisabled} />
            </form>
        </Container>
    )
}