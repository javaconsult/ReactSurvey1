import { useEffect, useState } from "react";
import { Button, Col, Container, Row } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import agent from "../api/Agent";
import SleepInDevMode from "../common/SleepInDevMode";
import LoadingComponent from "../layout/LoadingComponent";
import { Survey } from "../model/Survey";

export default function Results() {
    const { surveyId } = useParams(); //deconstruct from params
    const [survey, setSurvey] = useState<Survey>();
    const [loading, setLoading] = useState<boolean>(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchSurvey = async () => {
            if (surveyId) {
                await SleepInDevMode();
                try {
                    let s = await agent.Surveys.get(surveyId);
                    setSurvey(s);
                } catch (error) {
                    navigate( '/fail/Error loading results' )
                }
            }
            setLoading(false);
        }
        fetchSurvey();
    }, [surveyId, navigate]
    )

    const jsx = <Container>
        <h3 className='mt-3'>{survey?.name}</h3>
        <div className='font-weight-bold mb-1'>{survey?.description}</div>

        {survey?.participantCount !== undefined && survey?.participantCount === 0 ? 'No participants' :
        <LinkContainer to={`/participantlist/${survey?.id}`}>
            <Button variant='link'>{survey?.participantCount} participant{survey?.participantCount !== undefined && survey?.participantCount !== 1 && 's'} </Button>
        </LinkContainer>}

        {survey?.participantCount !== undefined && survey?.participantCount > 0 && survey?.questions?.map(question => {
            return (<div key={question.id} className='mt-3' >
                <Row >
                    <Col>
                        <b>{question.heading}</b>
                    </Col>
                </Row>

                {
                    question.text &&
                    <Row >
                        <Col>
                            {question.text}
                        </Col>
                    </Row>
                }
                {
                    question.options?.map(option => {
                        return (
                            <Row key={option.id}>
                                <Col xs="auto" >{option.text}</Col>
                                <Col>{option.percent}%</Col>
                            </Row>
                        )
                    })
                }
            </div>
            )
        })}
        <br />
    </Container>

    return (
        loading ? <LoadingComponent message='Loading results...' /> : jsx
    )
}


//https://stackblitz.com/edit/nested-object-map-function
