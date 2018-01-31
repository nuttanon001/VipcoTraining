export interface IUser {
    UserName: string;
    PassWord: string;
}

export class User implements User{
    UserName: string;
    PassWord: string;
}