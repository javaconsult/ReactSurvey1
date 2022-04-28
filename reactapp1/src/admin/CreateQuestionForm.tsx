import { Form, Formik, FormikHelpers } from 'formik';
import { Button, Col, Container, Row } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';

import * as Yup from 'yup';
import { QuestionFormValues } from '../model/Question';
import { useEffect } from 'react';
import { useStore } from '../state/Store';
import MyTextInput from '../common/MyTextInput';


export default function CreateQuestionForm() {
  const { surveyStore } = useStore(); //DI
  const navigate = useNavigate();

  useEffect(() => {
    //if page refreshed, context containing survey is lost
    //return to home page if survey is undefined
    if (surveyStore.survey === undefined) navigate('/');
  }, [surveyStore, navigate])

  const questionSchema = Yup.object({
    heading: Yup.string().required(),
    option1: Yup.string().required(),
    option2: Yup.string().required()
  });

  const handleComplete = async (values: QuestionFormValues) => {
    let isQuestionValid = await questionSchema.isValid(values);
    if (isQuestionValid) {
      surveyStore.addQuestion(values)
    }
    //if survey is valid, upload to api
    await surveyStore.uploadSurvey();
    navigate('/surveyList')
  }

  const handleSubmit = (values: QuestionFormValues, formikBag: FormikHelpers<QuestionFormValues>) => {
    surveyStore.addQuestion(values)
    formikBag.resetForm({});
  }

  return (

    <Formik
      initialValues={new QuestionFormValues()}
      validationSchema={questionSchema}
      onSubmit={handleSubmit}    >

      {
        //function takes FormikProps argument and renders the content
        formikProps => (
          <Container>
            <Row>
              <Col xs={12} md={9} lg={6} xl={4}>
                <Form onSubmit={formikProps.handleSubmit} >
                  <p className="h5 text-center mt-4">{surveyStore.survey?.name} - Question {surveyStore.questionNumber}</p>

                  <MyTextInput placeholder='Enter question summary' name='heading' label='Heading' />
                  <MyTextInput placeholder='Enter question detail' name='text' label='Detail' />
                  <MyTextInput placeholder='Enter an option' name='option1' label='Option 1' />
                  <MyTextInput placeholder='Enter an option' name='option2' label='Option 2' />

                  {/* Display option 3 if option 2 text input has a value */}
                  {formikProps.values.option2 && <MyTextInput placeholder='Enter an option' name='option3' label='Option 3' />}

                  {formikProps.values.option3 && <MyTextInput placeholder='Enter an option' name='option4' label='Option 4' />}

                  {formikProps.values.option4 && <MyTextInput placeholder='Enter an option' name='option5' label='Option 5' />}

                  {formikProps.values.option5 && <MyTextInput placeholder='Enter an option' name='option6' label='Option 6' />}

                  <Button className='mt-2' disabled={!formikProps.isValid || !formikProps.dirty || formikProps.isSubmitting} variant='primary' type='submit'>Next Question</Button>

                  {' '}

                  {!formikProps.isSubmitting && <Button className='mt-2' onClick={() => handleComplete(formikProps.values)} variant="primary" >Complete Survey</Button>}
                  <p></p>
                </Form>

              </Col>
            </Row>
          </Container>
        )
      }

    </Formik>

  )
}


