import { Form, Formik, FormikHelpers } from 'formik';
import { Button, Col, Container, Row } from 'react-bootstrap';
import * as Yup from 'yup';
import { AxiosError } from 'axios';
import { useNavigate } from "react-router-dom";
import { useStore } from '../state/Store';
import MyTextInput from '../common/MyTextInput';

interface Values {
    displayName: string;
    username: string;
    email: string;
    password: string;
}

function RegisterForm() {
    const { userStore } = useStore();
    const navigate = useNavigate();
    return (
        <Container>
            <Formik
                initialValues={{ displayName: '', username: '', email: '', password: '' }}
                //initialValues={{ displayName: '', username: 'sdineen', email: 'simondineen@gmail.com', password: 'Pa$$w0rd', error: null }}

                onSubmit={(values:Values, formikHelpers:FormikHelpers<Values>) => {
                    userStore.register(values)
                        .then(() => navigate('/surveyList'))
                        .catch(error => {
                            const msg = (error as AxiosError)?.response?.data;
                            console.log(msg)
                            //api returns BadRequest("Email Taken") or BadRequest("Username Taken");
                            const obj = msg.includes("Email") ? { email: msg } : { username: msg };
                            formikHelpers.setErrors(obj)
                            formikHelpers.setSubmitting(false);
                        }
                        );
                }
                }

                validationSchema={Yup.object({
                    displayName: Yup.string().required('Please enter your name'),
                    username: Yup.string().required('Please enter your username'),
                    email: Yup.string().email('Email is invalid').required('Email is required'),
                    password: Yup.string().required('Password is required').matches(
                        /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})/,
                        "Must Contain 8 Characters, One Uppercase, One Lowercase, One Number and One Special Case Character"
                    )
                })}
            >
                {/* render props pattern. deconstruct property */}
                {props => (
                    <Container>
                        <Row>
                            <Col xs={12} md={9} lg={6} xl={3}>
                                <Form onSubmit={props.handleSubmit} autoComplete='off'>
                                    <p className="h5 text-center mt-4">Sign up</p>

                                    <MyTextInput placeholder='Name' name='displayName' type='text' label='Name' />
                                    <MyTextInput placeholder='Username' name='username' type='text' label='Username' />
                                    <MyTextInput placeholder='Email' name='email' type="email" label='Email' />
                                    <MyTextInput placeholder='Password' name='password' type='password' label='Password' />

                                    <Button disabled={!props.isValid || !props.dirty || props.isSubmitting} variant='primary' type='submit'>Submit</Button>
                                </Form>
                            </Col>
                        </Row>
                    </Container>
                )}
            </Formik>
        </Container>
    )
}

export default RegisterForm;