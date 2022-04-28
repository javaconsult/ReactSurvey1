import { ErrorMessage, Field, Form, Formik } from 'formik';
import { Button, Col, Container, FormGroup, FormLabel, Row } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import * as Yup from 'yup';
import { AxiosError } from 'axios';
import { useStore } from '../state/Store';
import { useState } from 'react';
import LoadingComponent from '../layout/LoadingComponent';

function LoginForm() {
    const { userStore } = useStore();
    const navigate = useNavigate();
    const [loading, setLoading] = useState<boolean>(false);

    const jsx = <Container>
        <Formik
            //initialValues={{ email: '', password: '' }} 
            initialValues={{ email: 'simondineen@gmail.com', password: 'Pa$$w0rd' }}
            onSubmit={(values, { setErrors, setSubmitting }) => {
                setLoading(true);
                userStore.login(values)
                    .then(() => navigate("/surveyList"))
                    .catch(error => {
                        const msg = (error as AxiosError)?.response?.data;
                        //api returns Unauthorized("Email not found") or Unauthorized("Incorrect password");
                        const obj = msg.includes("Email") ? { email: msg } : { password: msg };
                        setErrors(obj)
                        setSubmitting(false);
                        setLoading(false);
                    }
                    );
            }
            }

            validationSchema={Yup.object({
                email: Yup.string().email('Email is invalid').required('Email is required'),
                password: Yup.string().required('Password is required')
            })}
        >

            {/* The <Formik> component accepts a function as its children (a render prop).  */}
            {/* (props) => (Form) */}
            {/* ({deconstructed props}) => (Form) */}
            {({ handleSubmit, isSubmitting, errors, isValid, touched, dirty }) => (
                <Container>
                    <Row>
                        <Col xs={12} md={9} lg={6} xl={3}>
                            <Form onSubmit={handleSubmit} >
                                <p className="h5 text-center mt-4">Sign in</p>

                                {/* <MyTextInput placeholder='Email' name='email' label='Email' />
                            <MyTextInput placeholder='Password' name='password' type='password' label='Password' /> */}

                                <FormGroup className='my-3'>
                                    <FormLabel>Email</FormLabel>
                                    <Field name="email" type="email" placeholder="Email"
                                        className={'form-control' + (errors.email && touched.email ? ' is-invalid' : '')} />
                                    <ErrorMessage name="email" component="div" className="invalid-feedback" />
                                </FormGroup>

                                <FormGroup className='mb-3'>
                                    <FormLabel>Password</FormLabel>
                                    <Field name="password" type="password" placeholder="Password"
                                        className={'form-control' + (errors.password && touched.password ? ' is-invalid' : '')} />
                                    <ErrorMessage name="password" component="div" className="invalid-feedback" />
                                </FormGroup>

                                <Button disabled={!isValid || isSubmitting} variant='primary' type='submit'>Submit</Button>

                            </Form>
                        </Col>
                    </Row>
                </Container>
            )}
        </Formik>
    </Container>

    return (
        loading ? <LoadingComponent message='Logging in...' /> : jsx
    )
}

export default LoginForm;