
import { AxiosError } from "axios";
import { useEffect, useState } from "react";
import { Button, Col, Container, Row } from "react-bootstrap";
import { useNavigate } from "react-router";
import { LinkContainer } from "react-router-bootstrap";
import ReactTooltip from "react-tooltip";
import agent from "../api/Agent";
import SleepInDevMode from "../common/SleepInDevMode";
import LoadingComponent from "../layout/LoadingComponent";
import { Survey } from "../model/Survey";

export default function SurveyList() {
    const navigate = useNavigate();
    const [surveys, setSurveys] = useState<Survey[]>([]);
    const [deleteSurveyId, setDeleteSurveyId] = useState<string | undefined>();
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchSurveys = async () => {
            await SleepInDevMode();
            try {
                let surveysFromApi: Survey[] = await agent.Surveys.list()
                setSurveys(surveysFromApi)
            } catch (error) {
                const errorMessage = (error as AxiosError)?.response?.data?.title;
                setLoading(false);
                navigate(`/fail/${errorMessage}`)
            }
            setLoading(false);
        }
        fetchSurveys();
    }, [deleteSurveyId, navigate] //changing state causes page update   
    )

    const handleDelete = async () => {
        deleteSurveyId && await agent.Surveys.delete(deleteSurveyId);  //await delete before continuing
        setDeleteSurveyId(undefined);
    }

    const jsx = <Container>
        <LinkContainer to="/createSurveyForm">
            <Button className='mr-3 mt-3' variant="primary" size="sm" >
                New Survey
            </Button>
        </LinkContainer>
        <p />

        {deleteSurveyId !== undefined ?
            <>
                Are you sure you want to delete this survey?
                <Button onClick={handleDelete} className='mx-1' variant="danger" size="sm" >
                    Yes
                </Button>
                <Button onClick={() => setDeleteSurveyId(undefined)} variant="secondary" size="sm" >
                    No
                </Button>
            </>
            :
            <>
                {surveys.map(survey => (
                    <div key={survey.id}>
                        <Row >
                            <Col xs={6}>
                                {survey.name}
                            </Col>
                            <Col >
                                <LinkContainer to={`/invite/${survey.id}`}>
                                    {/* https://react-bootstrap.github.io/components/buttons/ */}
                                    <Button className='mb-1' variant="secondary" size="sm" data-tip="Invite a participant to complete the survey" >
                                        Invite
                                    </Button>
                                </LinkContainer>
                                {/* https://www.npmjs.com/package/react-tooltip */}
                                <ReactTooltip />
                                {' '}
                                {/* <LinkContainer to={`/results/${survey.id}`}> */}
                                <LinkContainer to={`/results/${survey.id}`}>
                                    <Button className='mb-1' variant="secondary" size="sm" >
                                        Results
                                    </Button>
                                </LinkContainer>
                                {' '}
                                <Button onClick={() => setDeleteSurveyId(survey.id)} className='mb-1' variant="secondary" size="sm" >
                                    Delete
                                </Button>
                            </Col>
                        </Row>
                        <hr />
                    </div>
                ))}
            </>
        }
    </Container>

    return (
        loading ? <LoadingComponent message='Loading surveys...' /> : jsx
    )

}