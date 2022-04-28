import { Container } from "react-bootstrap";
import { useParams } from "react-router-dom";

export default function FailPage() {
    const { message } = useParams(); //deconstruct message from params
    return (
        <Container>
            <h2 className='mt-3'>
                {message}
            </h2>
        </Container>
    )
}