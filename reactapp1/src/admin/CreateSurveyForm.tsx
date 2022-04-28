import { ErrorMessage, Field, Form, Formik } from 'formik';
import { Button, Col, Container, FormGroup, FormLabel, Row } from 'react-bootstrap';
import * as Yup from 'yup';
import { useNavigate } from "react-router-dom";
import { useStore } from '../state/Store';

function CreateSurveyForm() {
  const { surveyStore } = useStore();
  const navigate = useNavigate();
  return (

    <Formik
      initialValues={{name:'',description:''}}

      onSubmit={(values) => {
          surveyStore.createSurvey(values)
          navigate('/createQuestionForm')
        }
      }

      validationSchema={Yup.object({
        name: Yup.string().required('Name is required'),
        description: Yup.string().required('Description is required')
      })}   >

      {/* render props pattern. deconstruct property */}
      {(formikProps) => (
        <Container>
          <Row>
            <Col xs={12} md={9} lg={6} xl={3}>
              <Form onSubmit={formikProps.handleSubmit} >
                <p className="h5 text-center mt-4">New Survey</p>

                <FormGroup className='my-3'>
                  <FormLabel>Name</FormLabel>
                  <Field name="name" type="text" placeholder="Name"
                    className={'form-control' + (formikProps.errors.name && formikProps.touched.name ? ' is-invalid' : '')} />
                  <ErrorMessage name="name" component="div" className="invalid-feedback" />
                </FormGroup>

                <FormGroup className='mb-3'>
                  <FormLabel>Description</FormLabel>
                  <Field name="description" type="text" placeholder="Description"
                    className={'form-control' + (formikProps.errors.description && formikProps.touched.description ? ' is-invalid' : '')} />
                  <ErrorMessage name="description" component="div" className="invalid-feedback" />
                </FormGroup>

                <Button disabled={!formikProps.isValid || !formikProps.dirty || formikProps.isSubmitting} variant='primary' type='submit'>Submit</Button>
              </Form>
            </Col>
          </Row>
        </Container>
      )}
    </Formik>
  )
}

export default CreateSurveyForm;