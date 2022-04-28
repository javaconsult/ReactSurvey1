import { createContext, useContext } from "react";
import SurveyStore from "./SurveyStore";
import UserStore from "./UserStore";

interface Store {
    surveyStore: SurveyStore;
    userStore: UserStore;
}

//create an instance of the observable
export const store: Store = {
    surveyStore: new SurveyStore(),
    userStore: new UserStore()
}

//pass the observable into createContext
//The defaultValue argument is only used when a component 
//does not have a matching Provider above it in the tree. 
//https://reactjs.org/docs/context.html
export const StoreContext = createContext(store);

//consume the context
export function  useStore():Store {
    return useContext(StoreContext);    
}