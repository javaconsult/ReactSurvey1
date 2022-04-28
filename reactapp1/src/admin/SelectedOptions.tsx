import { useEffect, useState } from "react";
import { Button, Col, Container, Row } from "react-bootstrap";
import { useParams } from "react-router";
import { useNavigate } from "react-router-dom";
import agent from "../api/Agent";
import { SelectedOptionDto } from "../model/SelectedOptionDto";

export default function SelectedOptions() {
    const { participantId } = useParams();
    const [selectedOptionDtos, setSelectedOptionDtos] = useState<SelectedOptionDto[]>();
    const navigate = useNavigate();

    useEffect(() => {
        participantId && agent.Surveys.selectedOptionsForParticipant(participantId)
            .then(selections => setSelectedOptionDtos(selections))
            .catch(e => navigate('/fail/Error loading selections')); 
    }, [participantId, navigate] 
    )
    return (
        <Container>
            <h3 className='mt-3'>Selections</h3>
            <Button variant='secondary' size='sm' onClick={() => navigate(-1)}>Back</Button>
            {selectedOptionDtos?.map(selection => {
                return (
                    <div key={selection.questionText} className='mt-3'  >
                        <Row>
                            <Col>{selection.questionText}</Col>
                        </Row>
                        <Row>
                            <Col className='font-weight-bold'>{selection.optionText}</Col>
                        </Row>
                    </div>
                )
            }
            )
            }
        </Container>
    )
} 