
export class ResponseDto<T> {
  responseData?: T;
  messageToClient?: string;
}

export class User {
  username?: string;
  tlfnumber?: number;
  email?: string;
  password?: string;
}
