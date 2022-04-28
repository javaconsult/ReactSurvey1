import ParticipantForm from "../participant/ParticipantForm";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import HomePage from "./HomePage";
import FailPage from "../common/FailPage";
import ConfirmPage from "../participant/ConfirmPage";
import SurveyList from "../admin/SurveyList";
import { store, StoreContext, useStore } from "../state/Store";
import { useEffect } from "react";
import LoginForm from "../identity/LoginForm";
import CreateSurveyForm from "../admin/CreateSurveyForm";
import NavBar from "./NavBar";
import RegisterForm from "../identity/RegisterForm";
import Test from "../common/Test";
import CreateQuestionForm from "../admin/CreateQuestionForm";
import InviteForm from "../admin/InviteForm";
import Results from "../admin/Results";
import ParticipantList from "../admin/ParticipantList";
import SelectedOptions from "../admin/SelectedOptions";

// Initiate Context
//export const UserContext = createContext({ name: '', auth: false });

function App() {

  const { userStore } = useStore();
  //page refresh causes change in userStore dependency
  useEffect(() => {
    if (userStore.token) {
      userStore.getUserFromApi();      
      //context is lost on page refresh
    }
  }, [userStore])

  return (
    <StoreContext.Provider value={store}>
      <BrowserRouter>
        <NavBar />
        <Routes>
          <Route path='/' element={<HomePage />} />
          <Route path='/participant/:id' element={<ParticipantForm />} />
          <Route path='/confirm' element={<ConfirmPage />} />
          <Route path='/fail/:message' element={<FailPage />} />
          <Route path='/login' element={<LoginForm />} />
          <Route path='/register' element={<RegisterForm />} />
          <Route path='/surveyList' element={<SurveyList />} />
          <Route path='/createSurveyForm' element={<CreateSurveyForm />} />
          <Route path='/createquestionForm' element={<CreateQuestionForm />} />
          <Route path='/invite/:surveyId' element={<InviteForm />} />
          <Route path='/results/:surveyId' element={<Results />} />
          <Route path='/participantlist/:surveyId' element={<ParticipantList />} />
          <Route path='/selectedoptions/:participantId' element={<SelectedOptions />} />
          <Route path='/test' element={<Test />} />
        </Routes>
      </BrowserRouter>
    </StoreContext.Provider>
  );
}

export default App;