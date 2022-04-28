
//returned from API as UserDto with these properties
export interface User {
    username: string;
    displayName: string;
    token: string;
    email:string;
}

//same interface for login and register forms
export interface UserFormValues {
    email: string;
    password: string;
    displayName?: string;
    username?: string;
}

