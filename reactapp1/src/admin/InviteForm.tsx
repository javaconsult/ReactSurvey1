import { Formik, FormikHelpers, FormikProps } from 'formik';
import { Col, Container, Row, Form, Button } from 'react-bootstrap';
import { useNavigate, useParams } from 'react-router-dom';
import ReactTooltip from "react-tooltip";
import { ParticipantDto } from '../model/ParticipantDto';
import * as Yup from 'yup';
import { AxiosError } from 'axios';
import agent from '../api/Agent';
import MyTextInput from '../common/MyTextInput';

interface Values {
    name: string;
    email: string;
}

function InviteForm() {
    const navigate = useNavigate();
    const { surveyId } = useParams(); //deconstruct from params

    return (
        <Container>
            <Formik
                initialValues={{ name: '', email: '' }}

                //https://formik.org/docs/examples/async-submission
                onSubmit={async (values: Values, formikHelpers: FormikHelpers<Values>) => {
                    try {
                        const participant = new ParticipantDto(surveyId, values.name, values.email);
                        const createParticipantResponse = await agent.Participant.create(participant)
                        const link = `${process.env.REACT_APP_WEBSITE_URL}/participant/${createParticipantResponse.participantId}`

                        //open email client
                        const body = encodeURIComponent(`Please click this link ${link} to participate in the survey`)
                        window.location.href = `mailto:${values.email}?subject=Survey invitation&body=${body}`;

                    } catch (error: any) {
                        const axiosError: AxiosError = error && (error as AxiosError);
                        //https://axios-http.com/docs/handling_errors
                        if (axiosError.response?.status === 409)
                            formikHelpers.setErrors({ email: 'This participant has already completed the survey' })
                        else
                            navigate(`/fail/${axiosError.response?.data.title}`);
                    }
                }
                }

                validationSchema={Yup.object({
                    email: Yup.string().email('Email is invalid').required('Email is required'),
                    name: Yup.string().required('Name is required')
                })}
            >

                {/* render props pattern */}
                {(formikProps: FormikProps<any>) => (
                    <Row>
                        <Col xs={12} md={9} lg={6} xl={3}>
                            <Form onSubmit={formikProps.handleSubmit} >
                                <p className="h5 text-center mt-4">Invite a participant</p>

                                <MyTextInput placeholder='Name' name='name' type='text' label='Name' />
                                <MyTextInput placeholder='Email' name='email' type='text' label='Email' />

                                <Button disabled={!formikProps.isValid || formikProps.isSubmitting || !formikProps.dirty} variant='primary' type='submit' data-tip='email participant'>
                                    Invite
                                </Button>
                                <ReactTooltip />
                            </Form>
                        </Col>
                    </Row>
                )}
            </Formik>
        </Container>
    )
}

export default InviteForm;

