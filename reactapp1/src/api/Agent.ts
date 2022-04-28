import axios, { AxiosResponse } from "axios";
import { CreateParticipantResponse } from "../model/CreateParticipantResponse";
import { ParticipantDto } from "../model/ParticipantDto";
import { SelectedOptionDto } from "../model/SelectedOptionDto";
import { Survey } from "../model/Survey";
import { User, UserFormValues } from "../model/user";
import { store } from "../state/Store";


axios.defaults.baseURL = process.env.REACT_APP_API_URL;
//axios.defaults.baseURL = 'https://localhost:7073/api';
//axios.defaults.baseURL = 'https://dinwebapp.azurewebsites.net/api';
//add jwt to request
axios.interceptors.request.use(config => {
    const token = store.userStore.token;
    if (token) config.headers!.Authorization = `Bearer ${token}`
    return config;
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const Surveys = {
    list: () => axios.get<Survey[]>('/survey').then(responseBody),
    create: (survey: Survey | undefined) => axios.post<string>('/survey', survey).then(responseBody),
    get: (id: string) => axios.get<Survey>(`/survey/${id}`).then(responseBody),
    delete: (id: string) => axios.delete<boolean>(`/survey/${id}`).then(responseBody),

    participantList: (surveyId: string) => axios.get<ParticipantDto[]>(`/survey/participants/${surveyId}`).then(responseBody),
    selectedOptionsForParticipant: (participantId: string) => axios.get<SelectedOptionDto[]>(`/survey/selections/${participantId}`).then(responseBody)
}

const Participant = {

    //gets survey with questions and options for completion by participant.
    get: (participantId: string) => axios.get<Survey>(`/participant/${participantId}`).then(responseBody),

    //creates a Participant and returns a CreateParticipantResponse, which has a ParticipantId property
    //Returns Conflict id participant previously completed the survey or NotFound if survey not found    
    create: (participantDto: ParticipantDto) => axios.post<CreateParticipantResponse>('/participant',participantDto).then(responseBody),

    //upload completed survey
    upload: (participantDto: ParticipantDto) => axios.put<void>('/participant', participantDto).then(responseBody)

}

const Account = {
    //calls GetCurrentUser method in AccountController which UserDto
    //UserDto has same properties as ts User, which is returned as a Promise
    current: () => axios.get<User>('/account').then(responseBody),

    //calls login method in AccountController, which takes LoginDto argument
    //UserFormValues has email and password, which are LoginDto's properties
    //API returns UserDto, which has same properties as ts User, which is returned as a Promise
    login: (user: UserFormValues) => axios.post<User>('/account/login', user).then(responseBody),

    register: (user: UserFormValues) => axios.post<User>('/account/register', user).then(responseBody)
}

const agent = {
    Surveys,
    Account,
    Participant
}

export default agent;