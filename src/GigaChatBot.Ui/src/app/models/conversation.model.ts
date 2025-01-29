export interface IConversation {
  id: string;
  messages: IMessage[];
}

export interface IMessage {
  id: number;
  content: string;
  createdOn: Date;
  type: MessageType;
  reaction: MessageReaction;
}

export enum MessageType {
  System,
  User,
  Assistant,
}

export enum MessageReaction {
  None,
  Like,
  Dislike,
}
