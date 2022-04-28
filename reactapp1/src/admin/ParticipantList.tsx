import React, { useEffect, useState } from "react";
import { Button, Col, Container, Row } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";
import { useParams } from "react-router-dom";
import agent from "../api/Agent";
import { ParticipantDto } from "../model/ParticipantDto";

export default function ParticipantList() {
    const { surveyId } = useParams();
    const [participantDtos, setParticipantDtos] = useState<ParticipantDto[]>();

    useEffect(() => {
        surveyId && agent.Surveys.participantList(surveyId)
            .then(participants => setParticipantDtos(participants));
    }, [surveyId]
    )
    return (
        <Container>
            <h3 className='mt-3'>Participants</h3>
            <LinkContainer to={`/results/${surveyId}`}>
                <Button variant='secondary' size='sm'>Back</Button>
            </LinkContainer>
            {participantDtos?.map(participantDto => {
                return (
                    <div key={participantDto.id} >
                        <Row>
                            <Col>
                                {/* link opens SelectedOptions component */}
                                <LinkContainer to={`/selectedoptions/${participantDto?.id}`}>
                                    <Button variant='link'>{participantDto.name}</Button>
                                </LinkContainer>
                            </Col>
                        </Row>
                    </div>
                )
            }
            )
            }
        </Container>
    )
}
