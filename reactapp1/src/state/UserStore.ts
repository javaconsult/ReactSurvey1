import {  makeAutoObservable, reaction, runInAction } from "mobx";
import agent from "../api/Agent";
import { User, UserFormValues } from "../model/user";

export default class UserStore {
    //view state in Components tab in Chrome (select Context.Provider)
    user: User | null = null;
    token: string | null = window.localStorage.getItem('jwt'); //retrieved on page refresh

    constructor() {
        makeAutoObservable(this);

        //https://mobx.js.org/reactions.html#reaction
        //runs when token changes
        reaction(
            () => this.token,
            //if token changed to null, remove from local storage
            //if token is set to a value, add to local storage
            //view local storage in Application tab in chrome
            token => {
                token? window.localStorage.setItem('jwt', token) : window.localStorage.removeItem('jwt');
            }
        )
    }

    //called from App useEffect 
    //caused by change in userStore dependency 
    //page refresh causes loss of mobx state
    //token field is initialised with jwt from localstorage, which is retained after page refresh
    //user is set with response from api
    getUserFromApi = async ()=> {        
        try {
            const user = await agent.Account.current();
            runInAction(() => this.user = user);
        } catch (error) {
            console.log(error);
        }
    }
    

    //used to switch between login and logout on NavBar 
    get isLoggedIn() {
        return !!this.user; //cast as boolean
    }

    register = async (creds: UserFormValues) => {
        try {
            const user = await agent.Account.register(creds);
            //modifying observable needs to be inside an action
            runInAction(() => {
                this.user = user
                this.token = user.token;
            });
        }
        catch (error) {
            // const err = error as AxiosError
            // console.log(err?.response?.data);
            throw error;
        }
    }

    login = async (creds: UserFormValues) => {
        try {
            const user:User = await agent.Account.login(creds);
            //modifying observable needs to be inside an action
            runInAction(() => {
                this.user = user
                this.token = user.token;
            });
        }
        catch (error) {
            throw error;
        }
    }

    logout = () => {
        this.user = null;
        this.token = null;
    }
}

