interface Reducer<T> {
    (state: T, action: Action): T;
}

interface Action {
    type: string;
    payload?: any;
}

export interface ThreadsEntities {
    [id: string]: Action;
 }